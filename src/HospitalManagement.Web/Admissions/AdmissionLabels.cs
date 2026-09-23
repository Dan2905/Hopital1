using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Web.Admissions;

/// <summary>
/// Libellés et nuances d'affichage des statuts de séjour hospitalier.
/// </summary>
public static class AdmissionLabels
{
    /// <summary>Statuts proposés dans les filtres, dans l'ordre d'affichage.</summary>
    public static IReadOnlyList<StatutAdmission> AllStatuses { get; } =
    [
        StatutAdmission.EnCours,
        StatutAdmission.Sorti,
        StatutAdmission.Transfere,
        StatutAdmission.Annulee
    ];

    /// <summary>Statuts qu'un séjour peut prendre depuis le statut « En cours ».</summary>
    public static IReadOnlyList<StatutAdmission> ClosingStatuses { get; } =
    [
        StatutAdmission.Sorti,
        StatutAdmission.Transfere,
        StatutAdmission.Annulee
    ];

    public static string GetLabel(StatutAdmission statut) => statut switch
    {
        StatutAdmission.EnCours => "En cours",
        StatutAdmission.Sorti => "Sorti",
        StatutAdmission.Transfere => "Transféré",
        StatutAdmission.Annulee => "Annulée",
        _ => "Inconnu"
    };

    /// <summary>Classe CSS de pastille associée au statut.</summary>
    public static string GetToneClass(StatutAdmission statut) => statut switch
    {
        StatutAdmission.EnCours => "stay-ongoing",
        StatutAdmission.Sorti => "stay-closed",
        StatutAdmission.Transfere => "transferred",
        StatutAdmission.Annulee => "cancelled",
        _ => "stay-ongoing"
    };

    /// <summary>Indique si le séjour occupe encore un lit.</summary>
    public static bool OccupiesBed(StatutAdmission statut) => statut == StatutAdmission.EnCours;
}
