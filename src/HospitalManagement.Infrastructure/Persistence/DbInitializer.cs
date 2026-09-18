using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagement.Infrastructure.Identity;

namespace HospitalManagement.Infrastructure.Persistence;

public static class DbInitializer
{
    /// <summary>
    /// Applique les migrations, garantit l'existence des rôles et sécurise le compte administrateur.
    /// </summary>
    /// <param name="serviceProvider">Conteneur d'injection de l'application.</param>
    /// <param name="adminEmail">Adresse du compte administrateur (configurable).</param>
    /// <param name="adminPassword">Mot de passe imposé au compte administrateur (configurable).</param>
    public static async Task InitializeAsync(
        IServiceProvider serviceProvider,
        string? adminEmail = null,
        string? adminPassword = null)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await dbContext.Database.MigrateAsync();

        foreach (var role in RoleNames.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var email = string.IsNullOrWhiteSpace(adminEmail) ? RoleNames.DefaultAdminEmail : adminEmail.Trim();
        var password = string.IsNullOrWhiteSpace(adminPassword) ? null : adminPassword.Trim();

        await EnsureAdministratorAsync(userManager, email, password);
        await EnsureRoleDemoUsersAsync(userManager);
    }

    private static async Task EnsureRoleDemoUsersAsync(UserManager<ApplicationUser> userManager)
    {
        var seedUsers = new[]
        {
            new { Email = "medecin@hopital.local", Password = "Medecin@123", Role = RoleNames.Medecin, FirstName = "Claire", LastName = "Martin" },
            new { Email = "infirmier@hopital.local", Password = "Infirmier@123", Role = RoleNames.Infirmier, FirstName = "Luc", LastName = "Dubois" },
            new { Email = "receptionniste@hopital.local", Password = "Reception@123", Role = RoleNames.Receptionniste, FirstName = "Sophie", LastName = "Leroy" },
            new { Email = "pharmacien@hopital.local", Password = "Pharmacien@123", Role = RoleNames.Pharmacien, FirstName = "Nadia", LastName = "Morel" },
            new { Email = "laborantin@hopital.local", Password = "Laborantin@123", Role = RoleNames.Laborantin, FirstName = "Thomas", LastName = "Girard" }
        };

        foreach (var seedUser in seedUsers)
        {
            var user = await userManager.FindByEmailAsync(seedUser.Email);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = seedUser.Email,
                    Email = seedUser.Email,
                    EmailConfirmed = true,
                    Nom = seedUser.LastName,
                    Prenom = seedUser.FirstName,
                    EstActif = true
                };

                var result = await userManager.CreateAsync(user, seedUser.Password);
                if (!result.Succeeded)
                {
                    continue;
                }
            }

            if (!await userManager.IsInRoleAsync(user, seedUser.Role))
            {
                await userManager.AddToRoleAsync(user, seedUser.Role);
            }

            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
            }

            if (!user.EstActif)
            {
                user.EstActif = true;
                await userManager.UpdateAsync(user);
            }
        }
    }

    /// <summary>
    /// Crée le compte administrateur s'il n'existe pas, sinon restaure ses droits et
    /// remplace un mot de passe hérité (trop faible) par celui fourni par la configuration.
    /// </summary>
    private static async Task EnsureAdministratorAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string? password)
    {
        var adminUser = await userManager.FindByEmailAsync(email);
        var isNewAccount = adminUser is null;

        if (adminUser is null)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "Le compte administrateur n'existe pas. Configurez Admin:Password ou HOSPITAL_ADMIN_PASSWORD avant le premier démarrage.");
            }

            adminUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Nom = "Administrateur",
                Prenom = "Système",
                EstActif = true
            };

            var creation = await userManager.CreateAsync(adminUser, password);
            if (!creation.Succeeded)
            {
                return;
            }
        }

        // Le compte administrateur ne doit jamais rester désactivé, verrouillé ou non confirmé.
        var needsUpdate = false;

        if (!adminUser.EmailConfirmed)
        {
            adminUser.EmailConfirmed = true;
            needsUpdate = true;
        }

        if (!adminUser.EstActif)
        {
            adminUser.EstActif = true;
            needsUpdate = true;
        }

        if (adminUser.LockoutEnabled && adminUser.LockoutEnd is not null)
        {
            adminUser.LockoutEnd = null;
            adminUser.AccessFailedCount = 0;
            needsUpdate = true;
        }

        if (needsUpdate)
        {
            await userManager.UpdateAsync(adminUser);
        }

        if (!await userManager.IsInRoleAsync(adminUser, RoleNames.Administrateur))
        {
            await userManager.AddToRoleAsync(adminUser, RoleNames.Administrateur);
        }

        // Un mot de passe hérité (documenté publiquement) est systématiquement remplacé.
        if (!isNewAccount
            && !string.IsNullOrWhiteSpace(password)
            && !string.Equals(password, RoleNames.LegacyDefaultPassword, StringComparison.Ordinal)
            && await userManager.CheckPasswordAsync(adminUser, RoleNames.LegacyDefaultPassword))
        {
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(adminUser);
            await userManager.ResetPasswordAsync(adminUser, resetToken, password);
        }
    }
}
