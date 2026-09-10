namespace HospitalManagement.Domain.Common;

/// <summary>
/// Entité de base de toutes les entités du domaine.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
