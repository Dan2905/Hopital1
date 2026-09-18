using System.Security.Claims;
using HospitalManagement.Infrastructure.Identity;

namespace HospitalManagement.Web.Components.Layout;

/// <summary>
/// Permet aux composants Blazor d'évaluer les autorisations de façon déclarative.
/// </summary>
public static class SecurityHelper
{
    /// <summary>
    /// Retourne le nom lisible d'un rôle.
    /// </summary>
    public static string GetRoleLabel(ClaimsPrincipal user) =>
        RoleNames.GetLabel(user.FindFirst(ClaimTypes.Role)?.Value);

    /// <summary>
    /// Indique si l'utilisateur authentifié possède au moins un des rôles listés.
    /// </summary>
    public static bool HasAnyRole(ClaimsPrincipal user, IReadOnlyList<string> roleNames) =>
        user.Identity?.IsAuthenticated == true
            && roleNames.Any(role => user.IsInRole(role));

    /// <summary>
    /// Indique si l'utilisateur peut accéder à un module dont les rôles autorisés sont
    /// définis par <param name="allowedRoles" />.
    /// </summary>
    public static bool MayAccess(ClaimsPrincipal user, IReadOnlyList<string> allowedRoles) =>
        HasAnyRole(user, allowedRoles);

    /// <summary>
    /// Extrait les initiales du compte connecté pour l'affichage dans le menu.
    /// </summary>
    public static string GetInitials(ClaimsPrincipal user)
    {
        var name = user.Identity?.Name;
        if (string.IsNullOrWhiteSpace(name))
        {
            return "?";
        }

        var localPart = name.Split('@')[0];
        var segments = localPart.Split(['.', '_', '-'], StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length >= 2)
        {
            return $"{segments[0][0]}{segments[1][0]}".ToUpperInvariant();
        }

        return localPart.Length >= 2
            ? localPart[..2].ToUpperInvariant()
            : localPart.ToUpperInvariant();
    }
}
