using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Persistence.Configurations;

public class MedicamentConfiguration : IEntityTypeConfiguration<Medicament>
{
    public void Configure(EntityTypeBuilder<Medicament> builder)
    {
        builder.ToTable("Medicaments");
        builder.Property(m => m.Nom).IsRequired().HasMaxLength(150);
        builder.Property(m => m.Code).HasMaxLength(30);
        builder.Property(m => m.Dosage).HasMaxLength(50);
        builder.Property(m => m.Forme).HasMaxLength(50);
        builder.Property(m => m.Fabricant).HasMaxLength(150);
        builder.Property(m => m.PrixAchat).HasColumnType("decimal(18,2)");
        builder.Property(m => m.PrixVente).HasColumnType("decimal(18,2)");
        builder.HasIndex(m => m.Code).IsUnique();

        builder.HasOne(m => m.Fournisseur)
            .WithMany(f => f.Medicaments)
            .HasForeignKey(m => m.FournisseurId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class FournisseurConfiguration : IEntityTypeConfiguration<Fournisseur>
{
    public void Configure(EntityTypeBuilder<Fournisseur> builder)
    {
        builder.ToTable("Fournisseurs");
        builder.Property(f => f.Nom).IsRequired().HasMaxLength(150);
        builder.Property(f => f.Contact).HasMaxLength(100);
        builder.Property(f => f.Telephone).HasMaxLength(20);
        builder.Property(f => f.Email).HasMaxLength(150);
        builder.Property(f => f.Adresse).HasMaxLength(250);
    }
}

public class CommandeMedicamentConfiguration : IEntityTypeConfiguration<CommandeMedicament>
{
    public void Configure(EntityTypeBuilder<CommandeMedicament> builder)
    {
        builder.ToTable("CommandesMedicaments");
        builder.Property(c => c.NumeroCommande).IsRequired().HasMaxLength(50);
        builder.Property(c => c.MontantTotal).HasColumnType("decimal(18,2)");
        builder.Property(c => c.Notes).HasMaxLength(1000);
        builder.HasIndex(c => c.NumeroCommande).IsUnique();

        builder.HasOne(c => c.Fournisseur)
            .WithMany(f => f.Commandes)
            .HasForeignKey(c => c.FournisseurId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CommandeLigneConfiguration : IEntityTypeConfiguration<CommandeLigne>
{
    public void Configure(EntityTypeBuilder<CommandeLigne> builder)
    {
        builder.ToTable("CommandeLignes");
        builder.Property(l => l.PrixUnitaire).HasColumnType("decimal(18,2)");

        builder.HasOne(l => l.CommandeMedicament)
            .WithMany(c => c.Lignes)
            .HasForeignKey(l => l.CommandeMedicamentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Medicament)
            .WithMany()
            .HasForeignKey(l => l.MedicamentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class MouvementStockConfiguration : IEntityTypeConfiguration<MouvementStock>
{
    public void Configure(EntityTypeBuilder<MouvementStock> builder)
    {
        builder.ToTable("MouvementsStock");
        builder.Property(m => m.Motif).HasMaxLength(500);
        builder.Property(m => m.Reference).HasMaxLength(100);

        builder.HasOne(m => m.Medicament)
            .WithMany(med => med.MouvementsStock)
            .HasForeignKey(m => m.MedicamentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class OrdonnanceConfiguration : IEntityTypeConfiguration<Ordonnance>
{
    public void Configure(EntityTypeBuilder<Ordonnance> builder)
    {
        builder.ToTable("Ordonnances");
        builder.Property(o => o.Instructions).HasMaxLength(1000);

        builder.HasOne(o => o.Consultation)
            .WithMany(c => c.Ordonnances)
            .HasForeignKey(o => o.ConsultationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.Patient)
            .WithMany(p => p.Ordonnances)
            .HasForeignKey(o => o.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class OrdonnanceLigneConfiguration : IEntityTypeConfiguration<OrdonnanceLigne>
{
    public void Configure(EntityTypeBuilder<OrdonnanceLigne> builder)
    {
        builder.ToTable("OrdonnanceLignes");
        builder.Property(l => l.Posologie).HasMaxLength(500);

        builder.HasOne(l => l.Ordonnance)
            .WithMany(o => o.Lignes)
            .HasForeignKey(l => l.OrdonnanceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Medicament)
            .WithMany(m => m.OrdonnanceLignes)
            .HasForeignKey(l => l.MedicamentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
