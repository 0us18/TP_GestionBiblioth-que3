using LibraryManagement.Core.Enums;

namespace LibraryManagement.Core.Entities;

public class Loan
{
    public int LoanId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; }
    public decimal? LateFee { get; set; }

    public User User { get; set; } = null!;
    public Book Book { get; set; } = null!;
}
