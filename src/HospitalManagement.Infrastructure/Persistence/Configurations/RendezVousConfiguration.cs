using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Persistence.Configurations;

public class RendezVousConfiguration : IEntityTypeConfiguration<RendezVous>
{
    public void Configure(EntityTypeBuilder<RendezVous> builder)
    {
        builder.ToTable("RendezVous");
        builder.Property(r => r.Motif).HasMaxLength(500);
        builder.Property(r => r.Notes).HasMaxLength(1000);

        builder.HasOne(r => r.Patient)
            .WithMany(p => p.RendezVous)
            .HasForeignKey(r => r.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Personnel)
            .WithMany(p => p.RendezVous)
            .HasForeignKey(r => r.PersonnelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.DateHeure);
    }
}

public class ConsultationConfiguration : IEntityTypeConfiguration<Consultation>
{
    public void Configure(EntityTypeBuilder<Consultation> builder)
    {
        builder.ToTable("Consultations");
        builder.Property(c => c.Motif).HasMaxLength(500);
        builder.Property(c => c.Diagnostic).HasMaxLength(1000);
        builder.Property(c => c.NotesCliniques).HasMaxLength(2000);
        builder.Property(c => c.Prescription).HasMaxLength(2000);

        builder.HasOne(c => c.Patient)
            .WithMany(p => p.Consultations)
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Personnel)
            .WithMany()
            .HasForeignKey(c => c.PersonnelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.RendezVous)
            .WithMany(r => r.Consultations)
            .HasForeignKey(c => c.RendezVousId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.DossierMedical)
            .WithMany(d => d.Consultations)
            .HasForeignKey(c => c.DossierMedicalId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
