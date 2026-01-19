using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserWithLoansAsync(int id);
    Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);
    Task<IEnumerable<User>> GetActiveUsersAsync();
}
