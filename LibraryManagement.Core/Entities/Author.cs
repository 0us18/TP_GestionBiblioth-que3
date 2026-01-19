using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Core.Entities;

public class Author
{
    public int AuthorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }

    // Navigation properties
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
