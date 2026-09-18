using System.Security.Claims;
using HospitalManagement.Infrastructure.Identity;
using HospitalManagement.Web.Navigation;

namespace HospitalManagement.Web.Components.Layout;

/// <summary>
/// Extensions utilitaires pour l'évaluation des autorisations dans les composants Blazor.
/// </summary>
public static class AuthorizationHelper
{
    /// <summary>
    /// Retourne le code de rôle brut du principal.
    /// </summary>
    public static string? GetRoleCode(this ClaimsPrincipal principal) =>
        principal.FindFirst(ClaimTypes.Role)?.Value;

    /// <summary>
    /// Retourne le libellé lisible du rôle (ex: "Administrateur", "Médecin"...).
    /// </summary>
    public static string GetRoleLabel(this ClaimsPrincipal principal) =>
        RoleNames.GetLabel(principal.GetRoleCode());

    /// <summary>
    /// Détermine si le principal appartient à au moins un des rôles spécifiés.
    /// </summary>
    public static bool HasAnyRole(this ClaimsPrincipal principal, params string[] roles) =>
        RoleNames.IsInAnyRole(principal, roles.ToList());

    /// <summary>
    /// Indique si le principal peut accéder à l'un des modules listés.
    /// </summary>
    public static bool CanAccessAny(this ClaimsPrincipal principal, params AppModule[] modules) =>
        modules.Any(m => AppModuleCatalog.MayAccess(m, principal));

    /// <summary>
    /// Retourne les badges de rôle pour l'affichage dans l'interface.
    /// </summary>
    public static IReadOnlyList<RolBadgeInfo> GetRoleBadges(this ClaimsPrincipal principal)
    {
        var role = principal.GetRoleCode();
        if (string.IsNullOrEmpty(role))
        {
            return [];
        }

        return new[]
        {
            new RolBadgeInfo(
                role,
                RoleNames.GetLabel(role),
                GetRoleTone(role))
        };
    }

    private static string GetRoleTone(string role) => role switch
    {
        RoleNames.Administrateur => "purple",
        RoleNames.Medecin => "blue",
        RoleNames.Infirmier => "green",
        RoleNames.Receptionniste => "orange",
        RoleNames.Pharmacien => "green",
        RoleNames.Laborantin => "orange",
        _ => "blue"
    };
}

/// <summary>
/// Informations d'un badge de rôle pour l'affichage.
/// </summary>
public sealed record RolBadgeInfo(string Code, string Label, string Tone);
