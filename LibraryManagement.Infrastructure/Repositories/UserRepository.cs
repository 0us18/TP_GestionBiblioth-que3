using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserWithLoansAsync(int id)
    {
        return await _dbSet
            .Include(u => u.Loans)
            .ThenInclude(l => l.Book)
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null)
    {
        var query = _dbSet.AsQueryable();
        
        if (excludeUserId.HasValue)
            query = query.Where(u => u.UserId != excludeUserId.Value);
            
        return !await query.AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _dbSet
            .Where(u => u.Status == UserStatus.Active)
            .ToListAsync();
    }
}
