using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class LoanRepository : Repository<Loan>, ILoanRepository
{
    public LoanRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Loan>> GetLoansByUserIdAsync(int userId)
    {
        return await _dbSet
            .Include(l => l.Book)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
    {
        var today = DateTime.Today;
        return await _dbSet
            .Include(l => l.Book)
            .Include(l => l.User)
            .Where(l => l.Status == LoanStatus.InProgress && l.DueDate < today)
            .ToListAsync();
    }

    public async Task<int> GetActiveLoanCountForUserAsync(int userId)
    {
        return await _dbSet
            .CountAsync(l => l.UserId == userId && l.Status == LoanStatus.InProgress);
    }
}
