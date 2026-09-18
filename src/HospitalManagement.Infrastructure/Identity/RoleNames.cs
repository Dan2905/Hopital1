using System.Security.Claims;

namespace HospitalManagement.Infrastructure.Identity;

/// <summary>
/// Rôles applicatifs et regroupements utilisés par la navigation et les autorisations.
/// </summary>
public static class RoleNames
{
    /// <summary>Rôle administrateur : accès complet, y compris la gestion des comptes.</summary>
    public const string Administrateur = "Admin";

    public const string Medecin = "Medecin";
    public const string Infirmier = "Infirmier";
    public const string Receptionniste = "Receptionniste";
    public const string Pharmacien = "Pharmacien";
    public const string Laborantin = "Laborantin";

    /// <summary>Compte administrateur créé automatiquement au premier démarrage.</summary>
    public const string DefaultAdminEmail = "admin@hopital.local";

    /// <summary>Ancien mot de passe par défaut, réinitialisé automatiquement car trop faible.</summary>
    public const string LegacyDefaultPassword = "Admin@123";

    /// <summary>Tous les rôles, dans l'ordre d'affichage.</summary>
    public static IReadOnlyList<string> All { get; } =
    [
        Administrateur,
        Medecin,
        Infirmier,
        Receptionniste,
        Pharmacien,
        Laborantin
    ];

    /// <summary>Rôles habilités à gérer les comptes et les rôles.</summary>
    public static IReadOnlyList<string> AdministrationOnly { get; } = [Administrateur];

    /// <summary>Rôles soignants (suivi médical du patient).</summary>
    public static IReadOnlyList<string> Soins { get; } =
        [Administrateur, Medecin, Infirmier];

    /// <summary>Rôles autorisés au parcours administratif du patient (accueil, séjours).</summary>
    public static IReadOnlyList<string> ParcoursPatient { get; } =
        [Administrateur, Medecin, Infirmier, Receptionniste];

    /// <summary>Rôles autorisés sur le laboratoire.</summary>
    public static IReadOnlyList<string> Laboratoire { get; } =
        [Administrateur, Medecin, Infirmier, Laborantin];

    /// <summary>Rôles autorisés sur la pharmacie.</summary>
    public static IReadOnlyList<string> Pharmacie { get; } =
        [Administrateur, Medecin, Pharmacien];

    /// <summary>Rôles autorisés sur la facturation.</summary>
    public static IReadOnlyList<string> Facturation { get; } =
        [Administrateur, Receptionniste];

    /// <summary>Rôles autorisés à consulter les rapports.</summary>
    public static IReadOnlyList<string> Rapports { get; } =
        [Administrateur, Medecin, Receptionniste];

    /// <summary>Libellé lisible d'un rôle technique.</summary>
    public static string GetLabel(string? role) => role switch
    {
        Administrateur => "Administrateur",
        Medecin => "Médecin",
        Infirmier => "Infirmier",
        Receptionniste => "Accueil",
        Pharmacien => "Pharmacien",
        Laborantin => "Laborantin",
        _ => "Utilisateur"
    };

    /// <summary>Chaîne utilisée par l'attribut <c>[Authorize(Roles = ...)]</c>.</summary>
    public static string Join(IEnumerable<string> roles) => string.Join(",", roles);

    /// <summary>Indique si le principal porte au moins un des rôles fournis.</summary>
    public static bool IsInAnyRole(this ClaimsPrincipal? principal, IReadOnlyList<string> roles)
        => principal?.Identity?.IsAuthenticated == true
            && roles.Any(role => principal.IsInRole(role));
}
