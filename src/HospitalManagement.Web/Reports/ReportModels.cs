using System.Globalization;

namespace HospitalManagement.Web.Reports;

/// <summary>
/// Période d'analyse d'un rapport (un mois civil complet).
/// </summary>
public sealed class ReportPeriod
{
    private static readonly CultureInfo FrenchCulture = CultureInfo.GetCultureInfo("fr-FR");

    public const int MinYear = 2000;
    public const int MaxYear = 2100;

    public ReportPeriod(int month, int year)
    {
        Year = year is < MinYear or > MaxYear ? DateTime.Today.Year : year;
        Month = month is < 1 or > 12 ? DateTime.Today.Month : month;
    }

    public int Month { get; }
    public int Year { get; }

    public DateTime Start => new(Year, Month, 1);
    public DateTime End => Start.AddMonths(1);
    public DateTime EndInclusive => End.AddDays(-1);

    /// <summary>Libellé long, par exemple « septembre 2026 ».</summary>
    public string Label => Start.ToString("MMMM yyyy", FrenchCulture);

    /// <summary>Libellé court utilisé dans les noms de fichiers exportés.</summary>
    public string FileToken => Start.ToString("yyyy-MM", CultureInfo.InvariantCulture);

    /// <summary>Période du mois courant.</summary>
    public static ReportPeriod Current => new(DateTime.Today.Month, DateTime.Today.Year);
}

/// <summary>Indicateur affiché sous forme de tuile.</summary>
public sealed record ReportIndicator(string Label, string Icon, string Tone, string Value, string Caption);

/// <summary>Ligne de répartition (nombre + part relative en pourcentage).</summary>
public sealed record ReportBreakdownRow(string Label, int Count, int Share, string Tone);

/// <summary>Recette mensuelle (facturé / encaissé).</summary>
public sealed record RevenueRow(string Label, decimal Invoiced, decimal Collected);

/// <summary>Analyse en attente de résultat.</summary>
public sealed record PendingAnalysisRow(string Patient, string Exam, string RequestedOn, string Status);

/// <summary>Médicament sous son seuil d'alerte.</summary>
public sealed record StockAlertRow(string Name, int Stock, int Threshold, string? Expiry);

/// <summary>Instantané complet des rapports pour une période donnée.</summary>
public sealed class ReportSnapshot
{
    public DateTime GeneratedAt { get; init; } = DateTime.Now;

    public ReportPeriod Period { get; init; } = ReportPeriod.Current;

    public int TotalBeds { get; init; }
    public int OccupiedBeds { get; init; }
    public int FreeBeds { get; init; }
    public int OccupancyRate { get; init; }

    public int TotalPatients { get; init; }
    public int NewPatientsInPeriod { get; init; }

    public int AppointmentsToday { get; init; }
    public int AppointmentsInPeriod { get; init; }
    public int AppointmentsCancelledInPeriod { get; init; }
    public int AppointmentsNoShowInPeriod { get; init; }
    public int ConsultationsInPeriod { get; init; }

    public int AdmissionsInPeriod { get; init; }
    public int DischargesInPeriod { get; init; }
    public int OngoingAdmissions { get; init; }

    public int PendingAnalyses { get; init; }
    public int CompletedAnalysesInPeriod { get; init; }

    public int LowStockMedicines { get; init; }

    public decimal InvoicedInPeriod { get; init; }
    public decimal CollectedInPeriod { get; init; }
    public decimal OutstandingAmount { get; init; }
    public int UnpaidInvoices { get; init; }

    public IReadOnlyList<ReportIndicator> Indicators { get; init; } = [];
    public IReadOnlyList<ReportBreakdownRow> AppointmentBreakdown { get; init; } = [];
    public IReadOnlyList<ReportBreakdownRow> AdmissionBreakdown { get; init; } = [];
    public IReadOnlyList<RevenueRow> RevenueTrend { get; init; } = [];
    public IReadOnlyList<PendingAnalysisRow> PendingAnalysisList { get; init; } = [];
    public IReadOnlyList<StockAlertRow> StockAlertList { get; init; } = [];

    public bool HasActivity =>
        AppointmentsInPeriod > 0
        || AdmissionsInPeriod > 0
        || ConsultationsInPeriod > 0
        || CollectedInPeriod > 0m;

    public static ReportSnapshot Empty { get; } = new();
}
