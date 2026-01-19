using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class ActivityRepository : Repository<Activity>, IActivityRepository
{
    public ActivityRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<Activity?> GetActivityWithParticipantsAsync(int id)
    {
        return await _dbSet
            .Include(a => a.Organizer)
            .Include(a => a.Participations)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(a => a.ActivityId == id);
    }

    public async Task<IEnumerable<Activity>> GetUpcomingActivitiesAsync()
    {
        return await _dbSet
            .Include(a => a.Organizer)
            .Where(a => a.ActivityDate >= DateTime.Today)
            .OrderBy(a => a.ActivityDate)
            .ToListAsync();
    }
}
