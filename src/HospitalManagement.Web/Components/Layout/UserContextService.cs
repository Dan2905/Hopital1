using System.Security.Claims;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Web.Components.Layout;

/// <summary>
/// Sert à afficher un message de bienvenue contextuel selon le rôle de l'utilisateur
/// connecté et à résoudre les informations affichées dans la barre de navigation.
/// </summary>
public sealed class UserContextService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly UserManager<ApplicationUser> userManager;

    public UserContextService(
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.userManager = userManager;
    }

    /// <summary>
    /// Informations de profil destinées à être affichées dans le menu ou le layout.
    /// </summary>
    public UserProfileProfile? GetProfile()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var applicationUser = userManager.GetUserAsync(user).GetAwaiter().GetResult();
        var role = user.FindFirst(ClaimTypes.Role)?.Value;

        return new UserProfileProfile(
            applicationUser?.UserName ?? user.Identity.Name ?? "?",
            RoleNames.GetLabel(role),
            applicationUser?.Prenom ?? applicationUser?.Nom ?? "",
            GetInitials(user),
            role);
    }

    /// <summary>
    /// Message d'accueil personnalisé selon le rôle et le moment de la journée.
    /// </summary>
    public static string BuildWelcomeMessage(string? firstName, string roleLabel)
    {
        var timeOfDay = DateTime.Now.Hour switch
        {
            >= 5 and < 12 => "matin",
            >= 12 and < 18 => "après-midi",
            _ => "soir"
        };

        var greeting = firstName is not null && firstName.Length > 0
            ? $"Bonjour {firstName}"
            : "Bonjour";

        var roleSpecificGreeting = roleLabel switch
        {
            "Administrateur" => ", vous avez accès à l'ensemble des modules de pilotage.",
            "Médecin" => ", votre agenda et les dossiers patients vous attendent.",
            "Infirmier" => ", vos missions de soins et suivi sont affichées ci-dessous.",
            "Accueil" => ", l'accueil des patients et la gestion des rendez-vous sont à votre disposition.",
            "Pharmacien" => ", la gestion des stocks et des prescriptions est ouverte.",
            "Laborantin" => ", le suivi des analyses et résultats est prêt.",
            _ => ", connecté à la plateforme hospitalière."
        };

        return $"{greeting}, {roleLabel}.{roleSpecificGreeting}";
    }

    private static string GetInitials(ClaimsPrincipal user)
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

/// <summary>
/// Profil simplifié destiné à la couche de présentation.
/// </summary>
public sealed record UserProfileProfile(
    string Email,
    string RoleLabel,
    string FirstName,
    string Initials,
    string? RoleCode);
