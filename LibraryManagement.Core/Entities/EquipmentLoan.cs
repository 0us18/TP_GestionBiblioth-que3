using LibraryManagement.Core.Enums;

namespace LibraryManagement.Core.Entities;

public class EquipmentLoan
{
    public int EquipmentLoanId { get; set; }
    public int EquipmentId { get; set; }
    public int UserId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; }

    public Equipment Equipment { get; set; } = null!;
    public User User { get; set; } = null!;
}
