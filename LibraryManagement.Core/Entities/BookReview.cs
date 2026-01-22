namespace LibraryManagement.Core.Entities;

public class BookReview
{
    public int ReviewId { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; }

    public Book Book { get; set; } = null!;
    public User User { get; set; } = null!;
}
