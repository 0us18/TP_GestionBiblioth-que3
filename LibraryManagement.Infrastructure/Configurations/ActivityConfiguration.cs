using LibraryManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(a => a.ActivityId);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(a => a.Organizer)
            .WithMany(e => e.OrganizedActivities)
            .HasForeignKey(a => a.OrganizerEmployeeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
