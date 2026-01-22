namespace LibraryManagement.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBookRepository Books { get; }
    IUserRepository Users { get; }
    ILoanRepository Loans { get; }
    IActivityRepository Activities { get; }
    IEquipmentRepository Equipment { get; }
    IEquipmentLoanRepository EquipmentLoans { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
