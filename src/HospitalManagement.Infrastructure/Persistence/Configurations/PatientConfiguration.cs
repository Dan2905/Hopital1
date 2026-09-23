using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");
        builder.Property(p => p.Nom).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Prenom).IsRequired().HasMaxLength(100);
        builder.Property(p => p.NumeroDossier).HasMaxLength(50);
        builder.Property(p => p.Telephone).HasMaxLength(20);
        builder.Property(p => p.Email).HasMaxLength(150);
        builder.Property(p => p.Adresse).HasMaxLength(250);
        builder.Property(p => p.Allergies).HasMaxLength(500);
        builder.Property(p => p.AntecedentsMedicaux).HasMaxLength(1000);
        builder.HasIndex(p => p.NumeroDossier).IsUnique();

        builder.HasOne(p => p.DossierMedical)
            .WithOne(d => d.Patient)
            .HasForeignKey<DossierMedical>(d => d.PatientId);
    }
}

public class DossierMedicalConfiguration : IEntityTypeConfiguration<DossierMedical>
{
    public void Configure(EntityTypeBuilder<DossierMedical> builder)
    {
        builder.ToTable("DossiersMedicaux");
        builder.Property(d => d.NotesGenerales).HasMaxLength(2000);
    }
}

public class AdmissionConfiguration : IEntityTypeConfiguration<Admission>
{
    public void Configure(EntityTypeBuilder<Admission> builder)
    {
        builder.ToTable("Admissions");
        builder.Property(a => a.MotifAdmission).HasMaxLength(500);
        builder.Property(a => a.Diagnostic).HasMaxLength(500);
        builder.Property(a => a.MedecinResponsable).HasMaxLength(150);

        // Recherche des séjours par lit / patient et contrôle des chevauchements.
        builder.HasIndex(a => new { a.LitId, a.Statut });
        builder.HasIndex(a => new { a.PatientId, a.DateEntree });

        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Admissions)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Lit)
            .WithMany(l => l.Admissions)
            .HasForeignKey(a => a.LitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Chambre)
            .WithMany()
            .HasForeignKey(a => a.ChambreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
