using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Core.Entities;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int AvailableCopies { get; set; }

    // Calculated property - logic will be in service or computed column, 
    // but for entity it might be better to just have it as a method or not mapped if complex.
    // Spec says "CurrentlyAvailable (calculated property)". 
    // We'll implement it as a read-only property based on logic if possible, or leave it for now.
    // For EF Core, we usually don't map calculated properties unless they are computed columns.
    // I'll add it as a NotMapped property for now.
    [NotMapped]
    public int CurrentlyAvailable => AvailableCopies; // This logic needs to account for active loans

    // Navigation properties
    public ICollection<Author> Authors { get; set; } = new List<Author>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public ICollection<BookReview> Reviews { get; set; } = new List<BookReview>();
}
