using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class ExamenLaboratoire : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string? Code { get; set; }
    public TypeExamen Type { get; set; }
    public string? Description { get; set; }
    public decimal Prix { get; set; }
    public string? Unite { get; set; }
    public decimal? ValeurMinNormale { get; set; }
    public decimal? ValeurMaxNormale { get; set; }
    public bool EstActif { get; set; } = true;
}

public class Analyse : BaseEntity
{
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public int? ConsultationId { get; set; }
    public Consultation? Consultation { get; set; }
    public int? PersonnelId { get; set; }
    public Personnel? Personnel { get; set; }
    public int ExamenLaboratoireId { get; set; }
    public ExamenLaboratoire? ExamenLaboratoire { get; set; }
    public DateTime DateDemande { get; set; } = DateTime.UtcNow;
    public DateTime? DateResultat { get; set; }
    public StatutAnalyse Statut { get; set; } = StatutAnalyse.EnAttente;
    public string? Notes { get; set; }

    public ICollection<ResultatAnalyse> Resultats { get; set; } = new List<ResultatAnalyse>();
}

public class ResultatAnalyse : BaseEntity
{
    public int AnalyseId { get; set; }
    public Analyse? Analyse { get; set; }
    public string? Parametre { get; set; }
    public string? Valeur { get; set; }
    public string? Unite { get; set; }
    public decimal? ValeurMinNormale { get; set; }
    public decimal? ValeurMaxNormale { get; set; }
    public bool EstAnormal { get; set; }
    public string? Commentaire { get; set; }
}
