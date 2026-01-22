using LibraryManagement.Core.Interfaces;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext _context;
    private IDbContextTransaction? _transaction;

    public IBookRepository Books { get; }
    public IUserRepository Users { get; }
    public ILoanRepository Loans { get; }
    public IActivityRepository Activities { get; }
    public IEquipmentRepository Equipment { get; }
    public IEquipmentLoanRepository EquipmentLoans { get; }

    public UnitOfWork(
        LibraryDbContext context,
        IBookRepository books,
        IUserRepository users,
        ILoanRepository loans,
        IActivityRepository activities,
        IEquipmentRepository equipment,
        IEquipmentLoanRepository equipmentLoans)
    {
        _context = context;
        Books = books;
        Users = users;
        Loans = loans;
        Activities = activities;
        Equipment = equipment;
        EquipmentLoans = equipmentLoans;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
