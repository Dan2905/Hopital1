using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Dashboard;

/// <summary>
/// Agrège les indicateurs du tableau de bord à partir de la base de données.
/// </summary>
public sealed class DashboardService
{
    private const int TrendMonthCount = 6;
    private const int AgendaSize = 5;
    private const int StockAlertSize = 4;

    private static readonly string[] MonthLabels =
    [
        "Jan", "Fév", "Mar", "Avr", "Mai", "Juin",
        "Juil", "Août", "Sep", "Oct", "Nov", "Déc"
    ];

    private readonly ApplicationDbContext db;

    public DashboardService(ApplicationDbContext db) => this.db = db;

    public async Task<DashboardSnapshot> GetSnapshotAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        var now = DateTime.Now;
        var tomorrow = today.AddDays(1);
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        var trendStart = firstDayOfMonth.AddMonths(-(TrendMonthCount - 1));

        var totalPatients = await db.Patients.CountAsync(cancellationToken);
        var activePatients = await db.Patients
            .CountAsync(patient => patient.Statut == StatutPatient.Actif, cancellationToken);

        var todayAppointments = await db.RendezVous
            .CountAsync(rdv => rdv.DateHeure >= today && rdv.DateHeure < tomorrow, cancellationToken);
        var upcomingAppointments = await db.RendezVous
            .CountAsync(rdv => rdv.DateHeure >= now
                && rdv.Statut != StatutRendezVous.Annule
                && rdv.Statut != StatutRendezVous.Termine, cancellationToken);

        var ongoingAdmissions = await db.Admissions
            .CountAsync(admission => admission.Statut == StatutAdmission.EnCours, cancellationToken);
        var activeStaff = await db.Personnels
            .CountAsync(personnel => personnel.EstActif, cancellationToken);
        var pendingAnalyses = await db.Analyses
            .CountAsync(analyse => analyse.Statut == StatutAnalyse.EnAttente
                || analyse.Statut == StatutAnalyse.EnCours, cancellationToken);
        var lowStockMedicines = await db.Medicaments
            .CountAsync(medicament => medicament.EstActif
                && medicament.QuantiteStock <= medicament.SeuilAlerte, cancellationToken);

        var unbilledStatuses = new[] { StatutFacture.Annulee, StatutFacture.Payee };
        var unpaidInvoices = await db.Factures
            .CountAsync(facture => !unbilledStatuses.Contains(facture.Statut), cancellationToken);
        var monthRevenue = await db.Factures
            .Where(facture => facture.DateEmission >= firstDayOfMonth
                && facture.Statut != StatutFacture.Annulee)
            .SumAsync(facture => (decimal?)facture.MontantPaye, cancellationToken) ?? 0m;
        var outstandingAmount = await db.Factures
            .Where(facture => !unbilledStatuses.Contains(facture.Statut))
            .SumAsync(facture => (decimal?)(facture.MontantTotal - facture.MontantPaye), cancellationToken) ?? 0m;

        var bedOccupancy = await GetBedOccupancyAsync(cancellationToken);
        var occupiedBeds = bedOccupancy
            .Where(item => item.Label == "Occupés")
            .Sum(item => item.Count);

        return new DashboardSnapshot
        {
            GeneratedAt = now,
            TotalPatients = totalPatients,
            ActivePatients = activePatients,
            TodayAppointments = todayAppointments,
            UpcomingAppointments = upcomingAppointments,
            OngoingAdmissions = ongoingAdmissions,
            ActiveStaff = activeStaff,
            PendingAnalyses = pendingAnalyses,
            LowStockMedicines = lowStockMedicines,
            UnpaidInvoices = unpaidInvoices,
            OccupiedBeds = occupiedBeds,
            TotalBeds = bedOccupancy.Sum(item => item.Count),
            MonthRevenue = monthRevenue,
            OutstandingAmount = outstandingAmount,
            ConsultationsByMonth = await GetMonthlyConsultationsAsync(trendStart, cancellationToken),
            BedOccupancy = bedOccupancy,
            NextAppointments = await GetNextAppointmentsAsync(today, now, cancellationToken),
            StockAlerts = await GetStockAlertsAsync(cancellationToken)
        };
    }

    private async Task<IReadOnlyList<MonthlyConsultations>> GetMonthlyConsultationsAsync(
        DateTime trendStart,
        CancellationToken cancellationToken)
    {
        var consultationDates = await db.Consultations
            .Where(consultation => consultation.DateConsultation >= trendStart)
            .Select(consultation => consultation.DateConsultation)
            .ToListAsync(cancellationToken);

        return Enumerable.Range(0, TrendMonthCount)
            .Select(offset => trendStart.AddMonths(offset))
            .Select(month => new MonthlyConsultations(
                MonthLabels[month.Month - 1],
                consultationDates.Count(date => date.Year == month.Year && date.Month == month.Month)))
            .ToList();
    }

    private async Task<IReadOnlyList<BedOccupancy>> GetBedOccupancyAsync(CancellationToken cancellationToken)
    {
        var bedCounts = await db.Lits
            .GroupBy(lit => lit.Statut)
            .Select(group => new { Status = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        var total = bedCounts.Sum(row => row.Count);

        return
        [
            BuildRow("Occupés", StatutLit.Occupe, "blue"),
            BuildRow("Disponibles", StatutLit.Libre, "green"),
            BuildRow("Réservés", StatutLit.Reserve, "orange"),
            BuildRow("Maintenance", StatutLit.EnMaintenance, "purple")
        ];

        BedOccupancy BuildRow(string label, StatutLit status, string tone)
        {
            var count = bedCounts.FirstOrDefault(row => row.Status == status)?.Count ?? 0;
            var percent = total == 0 ? 0 : (int)Math.Round(count * 100d / total);
            return new BedOccupancy(label, count, percent, tone);
        }
    }

    private async Task<IReadOnlyList<AgendaItem>> GetNextAppointmentsAsync(
        DateTime today,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var appointments = await db.RendezVous
            .AsNoTracking()
            .Include(rdv => rdv.Patient)
            .Include(rdv => rdv.Personnel)
            .Where(rdv => rdv.DateHeure >= now && rdv.Statut != StatutRendezVous.Annule)
            .OrderBy(rdv => rdv.DateHeure)
            .Take(AgendaSize)
            .ToListAsync(cancellationToken);

        return appointments
            .Select(rdv => new AgendaItem(
                rdv.DateHeure.ToString("HH:mm"),
                DescribeDay(rdv.DateHeure, today),
                FormatName(rdv.Patient?.Prenom, rdv.Patient?.Nom, "Patient inconnu"),
                FormatName(rdv.Personnel?.Prenom, rdv.Personnel?.Nom, "Praticien non affecté"),
                string.IsNullOrWhiteSpace(rdv.Motif) ? "Consultation" : rdv.Motif,
                GetStatusLabel(rdv.Statut),
                GetStatusTone(rdv.Statut)))
            .ToList();
    }

    private async Task<IReadOnlyList<StockAlertItem>> GetStockAlertsAsync(CancellationToken cancellationToken)
    {
        var rows = await db.Medicaments
            .AsNoTracking()
            .Where(medicament => medicament.EstActif
                && medicament.QuantiteStock <= medicament.SeuilAlerte)
            .OrderBy(medicament => medicament.QuantiteStock)
            .Take(StockAlertSize)
            .Select(medicament => new
            {
                medicament.Nom,
                medicament.QuantiteStock,
                medicament.SeuilAlerte
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new StockAlertItem(row.Nom, row.QuantiteStock, row.SeuilAlerte))
            .ToList();
    }

    private static string DescribeDay(DateTime date, DateTime today)
    {
        if (date.Date == today)
        {
            return "Aujourd'hui";
        }

        if (date.Date == today.AddDays(1))
        {
            return "Demain";
        }

        return date.ToString("dd/MM/yyyy");
    }

    private static string FormatName(string? firstName, string? lastName, string fallback)
    {
        var name = $"{firstName} {lastName}".Trim();
        return string.IsNullOrWhiteSpace(name) ? fallback : name;
    }

    private static string GetStatusLabel(StatutRendezVous status) => status switch
    {
        StatutRendezVous.Planifie => "Planifié",
        StatutRendezVous.Confirme => "Confirmé",
        StatutRendezVous.Annule => "Annulé",
        StatutRendezVous.Termine => "Terminé",
        StatutRendezVous.NoShow => "Absent",
        _ => "Planifié"
    };

    private static string GetStatusTone(StatutRendezVous status) => status switch
    {
        StatutRendezVous.Confirme => "success",
        StatutRendezVous.Termine => "info",
        StatutRendezVous.NoShow => "warning",
        _ => "info"
    };
}
