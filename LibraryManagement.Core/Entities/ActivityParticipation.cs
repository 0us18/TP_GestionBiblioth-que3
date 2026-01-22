using LibraryManagement.Core.Enums;

namespace LibraryManagement.Core.Entities;

public class ActivityParticipation
{
    public int ParticipationId { get; set; }
    public int ActivityId { get; set; }
    public int UserId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }

    public Activity Activity { get; set; } = null!;
    public User User { get; set; } = null!;
}
