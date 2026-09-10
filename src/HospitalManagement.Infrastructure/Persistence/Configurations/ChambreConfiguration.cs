using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Persistence.Configurations;

public class ChambreConfiguration : IEntityTypeConfiguration<Chambre>
{
    public void Configure(EntityTypeBuilder<Chambre> builder)
    {
        builder.ToTable("Chambres");
        builder.Property(c => c.Numero).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.TarifJournalier).HasColumnType("decimal(18,2)");
        builder.HasIndex(c => c.Numero).IsUnique();

        builder.HasOne(c => c.Departement)
            .WithMany(d => d.Chambres)
            .HasForeignKey(c => c.DepartementId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class LitConfiguration : IEntityTypeConfiguration<Lit>
{
    public void Configure(EntityTypeBuilder<Lit> builder)
    {
        builder.ToTable("Lits");
        builder.Property(l => l.Numero).IsRequired().HasMaxLength(20);
        builder.HasIndex(l => new { l.ChambreId, l.Numero }).IsUnique();

        builder.HasOne(l => l.Chambre)
            .WithMany(c => c.Lits)
            .HasForeignKey(l => l.ChambreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
