using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Chambre : BaseEntity
{
    public string Numero { get; set; } = string.Empty;
    public int Etage { get; set; }
    public TypeChambre Type { get; set; }
    public StatutChambre Statut { get; set; } = StatutChambre.Libre;
    public decimal TarifJournalier { get; set; }
    public string? Description { get; set; }

    public int? DepartementId { get; set; }
    public Departement? Departement { get; set; }
    public ICollection<Lit> Lits { get; set; } = new List<Lit>();
}

public class Lit : BaseEntity
{
    public string Numero { get; set; } = string.Empty;
    public StatutLit Statut { get; set; } = StatutLit.Libre;
    public bool EstAdapteEnfant { get; set; }

    public int ChambreId { get; set; }
    public Chambre? Chambre { get; set; }
    public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
}

public class Admission : BaseEntity
{
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public int? LitId { get; set; }
    public Lit? Lit { get; set; }
    public int? ChambreId { get; set; }
    public Chambre? Chambre { get; set; }
    public DateTime DateEntree { get; set; }
    public DateTime? DateSortie { get; set; }
    public string? MotifAdmission { get; set; }
    public string? Diagnostic { get; set; }
    public StatutAdmission Statut { get; set; } = StatutAdmission.EnCours;
    public string? MedecinResponsable { get; set; }
}
