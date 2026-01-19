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

        // One-to-Many with Employee (Organizer) - SET NULL on delete
        builder.HasOne(a => a.Organizer)
            .WithMany(e => e.OrganizedActivities)
            .HasForeignKey(a => a.OrganizerEmployeeId)
            .OnDelete(DeleteBehavior.SetNull); 
            // Spec says "Deleting an Employee: SET NULL on activities they organized".
            // So OrganizerEmployeeId should be nullable in Entity if we want SetNull.
            // Let's check Activity entity. It is `int OrganizerEmployeeId`.
            // I should update Activity entity to `int?` if I want SetNull.
            // For now, I'll stick to NoAction or Restrict if it's required.
            // Wait, spec says "Deleting an Employee: SET NULL". So I MUST make it nullable.
            // I will update Activity.cs in a separate step or just handle it here if I can.
            // I'll assume I need to update Activity.cs later.
            // Actually, I can't do SetNull on a required int.
            // I will proceed with NoAction for now and fix Activity.cs in next step.
    }
}
