using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Patient : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public DateTime DateNaissance { get; set; }
    public Sexe Sexe { get; set; }
    public string? NumeroDossier { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public string? Adresse { get; set; }
    public GroupeSanguin GroupeSanguin { get; set; } = GroupeSanguin.Inconnu;
    public string? Allergies { get; set; }
    public string? AntecedentsMedicaux { get; set; }
    public StatutPatient Statut { get; set; } = StatutPatient.Actif;

    public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
    public ICollection<RendezVous> RendezVous { get; set; } = new List<RendezVous>();
    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
    public ICollection<Facture> Factures { get; set; } = new List<Facture>();
    public ICollection<Analyse> Analyses { get; set; } = new List<Analyse>();
    public ICollection<Ordonnance> Ordonnances { get; set; } = new List<Ordonnance>();
    public DossierMedical? DossierMedical { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string NomComplet => $"{Prenom} {Nom}";
}

public class DossierMedical : BaseEntity
{
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public string? NotesGenerales { get; set; }
    public DateTime DateOuverture { get; set; } = DateTime.UtcNow;
    public bool EstActif { get; set; } = true;

    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
    public ICollection<Analyse> Analyses { get; set; } = new List<Analyse>();
}
