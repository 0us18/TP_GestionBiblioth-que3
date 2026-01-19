using LibraryManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Configurations;

public class ActivityParticipationConfiguration : IEntityTypeConfiguration<ActivityParticipation>
{
    public void Configure(EntityTypeBuilder<ActivityParticipation> builder)
    {
        builder.HasKey(ap => ap.ParticipationId);

        builder.HasOne(ap => ap.Activity)
            .WithMany(a => a.Participations)
            .HasForeignKey(ap => ap.ActivityId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting activity deletes participations

        builder.HasOne(ap => ap.User)
            .WithMany(u => u.Participations)
            .HasForeignKey(ap => ap.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Deleting user keeps history (or fails)
    }
}
