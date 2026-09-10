using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class RendezVous : BaseEntity
{
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public int PersonnelId { get; set; }
    public Personnel? Personnel { get; set; }
    public string? Motif { get; set; }
    public DateTime DateHeure { get; set; }
    public TimeSpan Duree { get; set; } = TimeSpan.FromMinutes(30);
    public StatutRendezVous Statut { get; set; } = StatutRendezVous.Planifie;
    public string? Notes { get; set; }

    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
}

public class Consultation : BaseEntity
{
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public int PersonnelId { get; set; }
    public Personnel? Personnel { get; set; }
    public int? RendezVousId { get; set; }
    public RendezVous? RendezVous { get; set; }
    public int? DossierMedicalId { get; set; }
    public DossierMedical? DossierMedical { get; set; }
    public DateTime DateConsultation { get; set; } = DateTime.UtcNow;
    public string? Motif { get; set; }
    public string? Diagnostic { get; set; }
    public string? NotesCliniques { get; set; }
    public string? Prescription { get; set; }
    public StatutConsultation Statut { get; set; } = StatutConsultation.Terminee;

    public ICollection<Ordonnance> Ordonnances { get; set; } = new List<Ordonnance>();
    public ICollection<Analyse> Analyses { get; set; } = new List<Analyse>();
    public ICollection<Facture> Factures { get; set; } = new List<Facture>();
}
