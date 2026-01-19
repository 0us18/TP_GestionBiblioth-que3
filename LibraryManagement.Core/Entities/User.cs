using LibraryManagement.Core.Enums;

namespace LibraryManagement.Core.Entities;

public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public UserStatus Status { get; set; }
    public UserType UserType { get; set; }

    // Navigation properties
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public ICollection<ActivityParticipation> Participations { get; set; } = new List<ActivityParticipation>();
    public ICollection<EquipmentLoan> EquipmentLoans { get; set; } = new List<EquipmentLoan>();
    public ICollection<BookReview> Reviews { get; set; } = new List<BookReview>();
}
