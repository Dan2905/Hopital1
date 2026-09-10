using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Persistence.Configurations;

public class FactureConfiguration : IEntityTypeConfiguration<Facture>
{
    public void Configure(EntityTypeBuilder<Facture> builder)
    {
        builder.ToTable("Factures");
        builder.Property(f => f.Numero).IsRequired().HasMaxLength(50);
        builder.Property(f => f.MontantTotal).HasColumnType("decimal(18,2)");
        builder.Property(f => f.MontantPaye).HasColumnType("decimal(18,2)");
        builder.Property(f => f.Notes).HasMaxLength(1000);
        builder.HasIndex(f => f.Numero).IsUnique();

        builder.HasOne(f => f.Patient)
            .WithMany(p => p.Factures)
            .HasForeignKey(f => f.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Consultation)
            .WithMany(c => c.Factures)
            .HasForeignKey(f => f.ConsultationId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class FactureLigneConfiguration : IEntityTypeConfiguration<FactureLigne>
{
    public void Configure(EntityTypeBuilder<FactureLigne> builder)
    {
        builder.ToTable("FactureLignes");
        builder.Property(l => l.Designation).HasMaxLength(500);
        builder.Property(l => l.PrixUnitaire).HasColumnType("decimal(18,2)");

        builder.HasOne(l => l.Facture)
            .WithMany(f => f.Lignes)
            .HasForeignKey(l => l.FactureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PaiementConfiguration : IEntityTypeConfiguration<Paiement>
{
    public void Configure(EntityTypeBuilder<Paiement> builder)
    {
        builder.ToTable("Paiements");
        builder.Property(p => p.Montant).HasColumnType("decimal(18,2)");
        builder.Property(p => p.Reference).HasMaxLength(100);

        builder.HasOne(p => p.Facture)
            .WithMany(f => f.Paiements)
            .HasForeignKey(p => p.FactureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
