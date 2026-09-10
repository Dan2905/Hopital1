using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Departement : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Localisation { get; set; }
    public string? Telephone { get; set; }

    public ICollection<Personnel> Personnels { get; set; } = new List<Personnel>();
    public ICollection<Chambre> Chambres { get; set; } = new List<Chambre>();
}

public class Personnel : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public TypePersonnel Type { get; set; }
    public string? Matricule { get; set; }
    public string? Specialite { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public DateTime? DateEmbauche { get; set; }
    public string? Adresse { get; set; }
    public bool EstActif { get; set; } = true;

    public int? DepartementId { get; set; }
    public Departement? Departement { get; set; }
    public ICollection<Planning> Plannings { get; set; } = new List<Planning>();
    public ICollection<RendezVous> RendezVous { get; set; } = new List<RendezVous>();

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string NomComplet => $"{Prenom} {Nom}";
}

public class Planning : BaseEntity
{
    public int PersonnelId { get; set; }
    public Personnel? Personnel { get; set; }
    public DayOfWeek JourSemaine { get; set; }
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; }
    public string? Note { get; set; }
}
