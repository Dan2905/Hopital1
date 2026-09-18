using System.Security.Claims;
using HospitalManagement.Infrastructure.Identity;

namespace HospitalManagement.Web.Navigation;

/// <summary>
/// Décrit une entrée de menu de l'application.
/// </summary>
/// <param name="Label">Libellé affiché dans les menus.</param>
/// <param name="Url">Route Blazor du module.</param>
/// <param name="Icon">Nom de l'icône Material utilisée.</param>
/// <param name="Description">Phrase courte présentant le module.</param>
/// <param name="Tone">Nuance de couleur du module (blue, purple, green, orange).</param>
/// <param name="AllowedRoles">
/// Rôles habilités à voir ce module dans la navigation.
/// Si la liste est vide, le module est affiché pour tous les usagers authentifiés.
/// </param>
public sealed record AppModule(
    string Label,
    string Url,
    string Icon,
    string Description,
    string Tone,
    IReadOnlyList<string>? AllowedRoles = null);

/// <summary>
/// Regroupe plusieurs modules sous un même intitulé de section.
/// </summary>
public sealed record NavSection(string Title, IReadOnlyList<AppModule> Modules);

/// <summary>
/// Source unique des menus de l'application (barre latérale et accès rapides).
/// </summary>
public static class AppModuleCatalog
{
    public static AppModule Dashboard { get; } = new(
        "Tableau de bord",
        "/",
        "dashboard",
        "Vue d'ensemble de l'activité hospitalière",
        "blue",
        RoleNames.ParcoursPatient);

    public static AppModule Patients { get; } = new(
        "Patients",
        "/patients",
        "people",
        "Dossiers, antécédents et statuts des patients",
        "blue",
        RoleNames.ParcoursPatient);

    public static AppModule RendezVous { get; } = new(
        "Rendez-vous",
        "/rendezvous",
        "event_available",
        "Agenda des consultations et suivi des présences",
        "purple",
        RoleNames.ParcoursPatient);

    public static AppModule Chambres { get; } = new(
        "Chambres & lits",
        "/chambres",
        "hotel",
        "Occupation des chambres, lits et tarifs journaliers",
        "green",
        RoleNames.ParcoursPatient);

    public static AppModule Laboratoire { get; } = new(
        "Laboratoire",
        "/laboratoire",
        "science",
        "Catalogue des examens et suivi des analyses",
        "orange",
        RoleNames.Laboratoire);

    public static AppModule Pharmacie { get; } = new(
        "Pharmacie",
        "/pharmacie",
        "medication",
        "Stocks, seuils d'alerte et péremptions des médicaments",
        "green",
        RoleNames.Pharmacie);

    public static AppModule Personnel { get; } = new(
        "Personnel",
        "/personnel",
        "badge",
        "Équipes médicales, administratives et plannings",
        "purple",
        RoleNames.AdministrationOnly);

    public static AppModule Facturation { get; } = new(
        "Facturation",
        "/facturation",
        "receipt_long",
        "Factures, encaissements et restes à recouvrer",
        "orange",
        RoleNames.Facturation);

    public static AppModule Roles { get; } = new(
        "Gestion des rôles",
        "/roles",
        "admin_panel_settings",
        "Gestion des utilisateurs, rôles et permissions",
        "purple",
        RoleNames.AdministrationOnly);

    /// <summary>Tous les modules, dans l'ordre d'affichage de la navigation.</summary>
    public static IReadOnlyList<AppModule> Modules { get; } =
    [
        Dashboard,
        Patients,
        RendezVous,
        Chambres,
        Laboratoire,
        Pharmacie,
        Personnel,
        Facturation,
        Roles
    ];

    /// <summary>Sections de la barre latérale.</summary>
    public static IReadOnlyList<NavSection> Sections { get; } =
    [
        new NavSection("Pilotage", [Dashboard]),
        new NavSection("Parcours patient", [Patients, RendezVous, Chambres]),
        new NavSection("Services médicaux", [Laboratoire, Pharmacie]),
        new NavSection("Administration", [Personnel, Facturation, Roles])
    ];

    /// <summary>Modules proposés en accès rapide sur le tableau de bord (hors accueil).</summary>
    public static IReadOnlyList<AppModule> QuickAccess { get; } =
        Modules.Where(module => module.Url != Dashboard.Url).ToList();

    /// <summary>
    /// Détermine si un module donné peut être affiché pour l'utilisateur connecté.
    /// </summary>
    public static bool MayAccess(AppModule module, ClaimsPrincipal user)
    {
        if (module.AllowedRoles is null or { Count: 0 })
        {
            return true;
        }

        return RoleNames.IsInAnyRole(user, module.AllowedRoles);
    }
}
