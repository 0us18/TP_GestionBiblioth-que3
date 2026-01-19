using LibraryManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Configurations;

public class BookReviewConfiguration : IEntityTypeConfiguration<BookReview>
{
    public void Configure(EntityTypeBuilder<BookReview> builder)
    {
        builder.HasKey(br => br.ReviewId);

        builder.Property(br => br.Comment)
            .HasMaxLength(1000);

        builder.HasOne(br => br.Book)
            .WithMany(b => b.Reviews)
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(br => br.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(br => br.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
