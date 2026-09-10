using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Facture : BaseEntity
{
    public string Numero { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public int? ConsultationId { get; set; }
    public Consultation? Consultation { get; set; }
    public DateTime DateEmission { get; set; } = DateTime.UtcNow;
    public DateTime? DateEcheance { get; set; }
    public decimal MontantTotal { get; set; }
    public decimal MontantPaye { get; set; }
    public decimal MontantRestant => MontantTotal - MontantPaye;
    public StatutFacture Statut { get; set; } = StatutFacture.EnAttente;
    public string? Notes { get; set; }

    public ICollection<FactureLigne> Lignes { get; set; } = new List<FactureLigne>();
    public ICollection<Paiement> Paiements { get; set; } = new List<Paiement>();
}

public class FactureLigne : BaseEntity
{
    public int FactureId { get; set; }
    public Facture? Facture { get; set; }
    public string? Designation { get; set; }
    public int Quantite { get; set; } = 1;
    public decimal PrixUnitaire { get; set; }
    public decimal Montant => Quantite * PrixUnitaire;
}

public class Paiement : BaseEntity
{
    public int FactureId { get; set; }
    public Facture? Facture { get; set; }
    public DateTime DatePaiement { get; set; } = DateTime.UtcNow;
    public decimal Montant { get; set; }
    public ModePaiement Mode { get; set; }
    public StatutPaiement Statut { get; set; } = StatutPaiement.Confirme;
    public string? Reference { get; set; }
}
