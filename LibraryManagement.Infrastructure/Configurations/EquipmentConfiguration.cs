using LibraryManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.HasKey(e => e.EquipmentId);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.SerialNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(e => e.SerialNumber)
            .IsUnique();

        builder.HasMany(e => e.Loans)
            .WithOne(el => el.Equipment)
            .HasForeignKey(el => el.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
