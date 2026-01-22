using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class EquipmentLoanRepository : Repository<EquipmentLoan>, IEquipmentLoanRepository
{
    public EquipmentLoanRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<EquipmentLoan?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(el => el.Equipment)
            .Include(el => el.User)
            .FirstOrDefaultAsync(el => el.EquipmentLoanId == id);
    }

    public async Task<IEnumerable<EquipmentLoan>> GetLoansByEquipmentIdAsync(int equipmentId)
    {
        return await _dbSet
            .Include(el => el.User)
            .Where(el => el.EquipmentId == equipmentId)
            .OrderByDescending(el => el.LoanDate)
            .ToListAsync();
    }
}

