using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    public bool EstActif { get; set; } = true;

    public int? PersonnelId { get; set; }
}
