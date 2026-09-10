using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Persistence.Configurations;

public class ExamenLaboratoireConfiguration : IEntityTypeConfiguration<ExamenLaboratoire>
{
    public void Configure(EntityTypeBuilder<ExamenLaboratoire> builder)
    {
        builder.ToTable("ExamensLaboratoire");
        builder.Property(e => e.Nom).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Code).HasMaxLength(30);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.Prix).HasColumnType("decimal(18,2)");
        builder.Property(e => e.Unite).HasMaxLength(20);
        builder.HasIndex(e => e.Code).IsUnique();
    }
}

public class AnalyseConfiguration : IEntityTypeConfiguration<Analyse>
{
    public void Configure(EntityTypeBuilder<Analyse> builder)
    {
        builder.ToTable("Analyses");
        builder.Property(a => a.Notes).HasMaxLength(1000);

        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Analyses)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Consultation)
            .WithMany(c => c.Analyses)
            .HasForeignKey(a => a.ConsultationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.Personnel)
            .WithMany()
            .HasForeignKey(a => a.PersonnelId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.ExamenLaboratoire)
            .WithMany()
            .HasForeignKey(a => a.ExamenLaboratoireId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ResultatAnalyseConfiguration : IEntityTypeConfiguration<ResultatAnalyse>
{
    public void Configure(EntityTypeBuilder<ResultatAnalyse> builder)
    {
        builder.ToTable("ResultatsAnalyses");
        builder.Property(r => r.Parametre).HasMaxLength(150);
        builder.Property(r => r.Valeur).HasMaxLength(100);
        builder.Property(r => r.Unite).HasMaxLength(20);
        builder.Property(r => r.Commentaire).HasMaxLength(500);

        builder.HasOne(r => r.Analyse)
            .WithMany(a => a.Resultats)
            .HasForeignKey(r => r.AnalyseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
