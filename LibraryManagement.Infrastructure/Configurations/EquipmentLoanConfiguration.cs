using LibraryManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Configurations;

public class EquipmentLoanConfiguration : IEntityTypeConfiguration<EquipmentLoan>
{
    public void Configure(EntityTypeBuilder<EquipmentLoan> builder)
    {
        builder.HasKey(el => el.EquipmentLoanId);

        builder.HasOne(el => el.Equipment)
            .WithMany(e => e.Loans)
            .HasForeignKey(el => el.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(el => el.User)
            .WithMany(u => u.EquipmentLoans)
            .HasForeignKey(el => el.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
