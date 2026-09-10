using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Medicament : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Dosage { get; set; }
    public string? Forme { get; set; } // comprimé, sirop, injectable...
    public string? Fabricant { get; set; }
    public decimal PrixAchat { get; set; }
    public decimal PrixVente { get; set; }
    public int QuantiteStock { get; set; }
    public int SeuilAlerte { get; set; } = 10;
    public DateTime? DatePeremption { get; set; }
    public bool EstActif { get; set; } = true;

    public int? FournisseurId { get; set; }
    public Fournisseur? Fournisseur { get; set; }
    public ICollection<MouvementStock> MouvementsStock { get; set; } = new List<MouvementStock>();
    public ICollection<OrdonnanceLigne> OrdonnanceLignes { get; set; } = new List<OrdonnanceLigne>();
}

public class Fournisseur : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string? Contact { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public string? Adresse { get; set; }
    public bool EstActif { get; set; } = true;

    public ICollection<Medicament> Medicaments { get; set; } = new List<Medicament>();
    public ICollection<CommandeMedicament> Commandes { get; set; } = new List<CommandeMedicament>();
}

public class CommandeMedicament : BaseEntity
{
    public string NumeroCommande { get; set; } = string.Empty;
    public int FournisseurId { get; set; }
    public Fournisseur? Fournisseur { get; set; }
    public DateTime DateCommande { get; set; } = DateTime.UtcNow;
    public DateTime? DateReception { get; set; }
    public decimal MontantTotal { get; set; }
    public StatutCommande Statut { get; set; } = StatutCommande.Planifiee;
    public string? Notes { get; set; }

    public ICollection<CommandeLigne> Lignes { get; set; } = new List<CommandeLigne>();
}

public class CommandeLigne : BaseEntity
{
    public int CommandeMedicamentId { get; set; }
    public CommandeMedicament? CommandeMedicament { get; set; }
    public int MedicamentId { get; set; }
    public Medicament? Medicament { get; set; }
    public int Quantite { get; set; }
    public decimal PrixUnitaire { get; set; }
    public decimal Montant => Quantite * PrixUnitaire;
}

public class MouvementStock : BaseEntity
{
    public int MedicamentId { get; set; }
    public Medicament? Medicament { get; set; }
    public StatutMouvementStock Type { get; set; }
    public int Quantite { get; set; }
    public DateTime DateMouvement { get; set; } = DateTime.UtcNow;
    public string? Motif { get; set; }
    public string? Reference { get; set; }
}

public class Ordonnance : BaseEntity
{
    public int ConsultationId { get; set; }
    public Consultation? Consultation { get; set; }
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public DateTime DatePrescription { get; set; } = DateTime.UtcNow;
    public string? Instructions { get; set; }

    public ICollection<OrdonnanceLigne> Lignes { get; set; } = new List<OrdonnanceLigne>();
}

public class OrdonnanceLigne : BaseEntity
{
    public int OrdonnanceId { get; set; }
    public Ordonnance? Ordonnance { get; set; }
    public int MedicamentId { get; set; }
    public Medicament? Medicament { get; set; }
    public string? Posologie { get; set; }
    public int DureeEnJours { get; set; }
    public int Quantite { get; set; }
}
