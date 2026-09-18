namespace HospitalManagement.Web.Dashboard;

/// <summary>
/// Instantané des indicateurs affichés sur le tableau de bord.
/// </summary>
public sealed class DashboardSnapshot
{
    public DateTime GeneratedAt { get; init; } = DateTime.Now;

    public int TotalPatients { get; init; }
    public int ActivePatients { get; init; }
    public int TodayAppointments { get; init; }
    public int UpcomingAppointments { get; init; }
    public int OngoingAdmissions { get; init; }
    public int ActiveStaff { get; init; }
    public int PendingAnalyses { get; init; }
    public int LowStockMedicines { get; init; }
    public int UnpaidInvoices { get; init; }

    public int OccupiedBeds { get; init; }
    public int TotalBeds { get; init; }

    public decimal MonthRevenue { get; init; }
    public decimal OutstandingAmount { get; init; }

    public IReadOnlyList<MonthlyConsultations> ConsultationsByMonth { get; init; } = [];
    public IReadOnlyList<BedOccupancy> BedOccupancy { get; init; } = [];
    public IReadOnlyList<AgendaItem> NextAppointments { get; init; } = [];
    public IReadOnlyList<StockAlertItem> StockAlerts { get; init; } = [];

    /// <summary>Taux d'occupation des lits en pourcentage.</summary>
    public int OccupancyRate =>
        TotalBeds == 0 ? 0 : (int)Math.Round(OccupiedBeds * 100d / TotalBeds);

    public int FreeBeds => Math.Max(0, TotalBeds - OccupiedBeds);

    /// <summary>Indique si au moins une consultation a été enregistrée sur la période suivie.</summary>
    public bool HasConsultationActivity => ConsultationsByMonth.Any(point => point.Count > 0);

    /// <summary>Instant de référence lorsque la base n'a pas pu être interrogée.</summary>
    public static DashboardSnapshot Empty { get; } = new();
}

/// <summary>Nombre de consultations pour un mois donné.</summary>
public sealed record MonthlyConsultations(string Label, int Count);

/// <summary>Répartition des lits par statut.</summary>
public sealed record BedOccupancy(string Label, int Count, int Percent, string Tone);

/// <summary>Rendez-vous à venir affiché dans l'agenda du tableau de bord.</summary>
public sealed record AgendaItem(
    string TimeLabel,
    string DayLabel,
    string PatientName,
    string PractitionerName,
    string Motif,
    string StatusLabel,
    string StatusTone);

/// <summary>Médicament dont le stock a atteint le seuil d'alerte.</summary>
public sealed record StockAlertItem(string Name, int Stock, int Threshold);
