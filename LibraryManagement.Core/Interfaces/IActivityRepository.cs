using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces;

public interface IActivityRepository : IRepository<Activity>
{
    Task<Activity?> GetActivityWithParticipantsAsync(int id);
    Task<IEnumerable<Activity>> GetUpcomingActivitiesAsync();
}
