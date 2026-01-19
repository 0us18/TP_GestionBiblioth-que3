using LibraryManagement.Core.Enums;

namespace LibraryManagement.Core.Entities;

public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public EmployeeRole Role { get; set; }

    // Navigation properties
    public ICollection<Activity> OrganizedActivities { get; set; } = new List<Activity>();
}
