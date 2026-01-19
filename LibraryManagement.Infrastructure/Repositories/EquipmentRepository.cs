using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
{
    public EquipmentRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Equipment>> GetAvailableEquipmentAsync()
    {
        return await _dbSet
            .Where(e => e.Status == EquipmentStatus.Available)
            .ToListAsync();
    }

    public async Task<Equipment?> GetEquipmentWithLoansAsync(int id)
    {
        return await _dbSet
            .Include(e => e.Loans)
                .ThenInclude(l => l.User)
            .FirstOrDefaultAsync(e => e.EquipmentId == id);
    }
}
