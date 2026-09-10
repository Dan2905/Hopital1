using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Persistence.Configurations;

public class DepartementConfiguration : IEntityTypeConfiguration<Departement>
{
    public void Configure(EntityTypeBuilder<Departement> builder)
    {
        builder.ToTable("Departements");
        builder.Property(d => d.Nom).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Code).HasMaxLength(20);
        builder.Property(d => d.Description).HasMaxLength(500);
        builder.Property(d => d.Localisation).HasMaxLength(150);
        builder.Property(d => d.Telephone).HasMaxLength(20);
        builder.HasIndex(d => d.Code).IsUnique();
    }
}

public class PersonnelConfiguration : IEntityTypeConfiguration<Personnel>
{
    public void Configure(EntityTypeBuilder<Personnel> builder)
    {
        builder.ToTable("Personnels");
        builder.Property(p => p.Nom).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Prenom).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Matricule).HasMaxLength(20);
        builder.Property(p => p.Specialite).HasMaxLength(150);
        builder.Property(p => p.Telephone).HasMaxLength(20);
        builder.Property(p => p.Email).HasMaxLength(150);
        builder.Property(p => p.Adresse).HasMaxLength(250);
        builder.HasIndex(p => p.Matricule).IsUnique();

        builder.HasOne(p => p.Departement)
            .WithMany(d => d.Personnels)
            .HasForeignKey(p => p.DepartementId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class PlanningConfiguration : IEntityTypeConfiguration<Planning>
{
    public void Configure(EntityTypeBuilder<Planning> builder)
    {
        builder.ToTable("Plannings");
        builder.Property(p => p.Note).HasMaxLength(500);

        builder.HasOne(p => p.Personnel)
            .WithMany(p => p.Plannings)
            .HasForeignKey(p => p.PersonnelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
