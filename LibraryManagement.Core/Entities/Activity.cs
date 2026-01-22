using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Core.Enums;

namespace LibraryManagement.Core.Entities;

public class Activity
{
    public int ActivityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ActivityType Type { get; set; }
    public DateTime ActivityDate { get; set; }
    public int MaxCapacity { get; set; }
    public int? OrganizerEmployeeId { get; set; }

    [NotMapped]
    public int CurrentParticipants => Participations?.Count ?? 0;

    public Employee Organizer { get; set; } = null!;
    public ICollection<ActivityParticipation> Participations { get; set; } = new List<ActivityParticipation>();
}
