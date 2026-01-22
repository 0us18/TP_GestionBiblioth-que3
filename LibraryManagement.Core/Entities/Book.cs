using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Core.Entities;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int AvailableCopies { get; set; }

    [NotMapped]
    public int CurrentlyAvailable => AvailableCopies;

    public ICollection<Author> Authors { get; set; } = new List<Author>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public ICollection<BookReview> Reviews { get; set; } = new List<BookReview>();
}
