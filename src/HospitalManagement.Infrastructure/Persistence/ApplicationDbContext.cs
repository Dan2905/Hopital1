using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<DossierMedical> DossiersMedicaux => Set<DossierMedical>();
    public DbSet<Admission> Admissions => Set<Admission>();
    public DbSet<Chambre> Chambres => Set<Chambre>();
    public DbSet<Lit> Lits => Set<Lit>();
    public DbSet<Departement> Departements => Set<Departement>();
    public DbSet<Personnel> Personnels => Set<Personnel>();
    public DbSet<Planning> Plannings => Set<Planning>();
    public DbSet<RendezVous> RendezVous => Set<RendezVous>();
    public DbSet<Consultation> Consultations => Set<Consultation>();
    public DbSet<Facture> Factures => Set<Facture>();
    public DbSet<FactureLigne> FactureLignes => Set<FactureLigne>();
    public DbSet<Paiement> Paiements => Set<Paiement>();
    public DbSet<Medicament> Medicaments => Set<Medicament>();
    public DbSet<Fournisseur> Fournisseurs => Set<Fournisseur>();
    public DbSet<CommandeMedicament> CommandesMedicaments => Set<CommandeMedicament>();
    public DbSet<CommandeLigne> CommandeLignes => Set<CommandeLigne>();
    public DbSet<MouvementStock> MouvementsStock => Set<MouvementStock>();
    public DbSet<Ordonnance> Ordonnances => Set<Ordonnance>();
    public DbSet<OrdonnanceLigne> OrdonnanceLignes => Set<OrdonnanceLigne>();
    public DbSet<ExamenLaboratoire> ExamensLaboratoire => Set<ExamenLaboratoire>();
    public DbSet<Analyse> Analyses => Set<Analyse>();
    public DbSet<ResultatAnalyse> ResultatsAnalyses => Set<ResultatAnalyse>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
