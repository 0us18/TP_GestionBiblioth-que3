using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces;

public interface ILoanRepository : IRepository<Loan>
{
    Task<IEnumerable<Loan>> GetLoansByUserIdAsync(int userId);
    Task<IEnumerable<Loan>> GetOverdueLoansAsync();
    Task<int> GetActiveLoanCountForUserAsync(int userId);
}
