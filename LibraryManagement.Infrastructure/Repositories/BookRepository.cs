using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        return await _dbSet
            .Include(b => b.Authors)
            .Include(b => b.Categories)
            .Where(b => b.AvailableCopies > 0)
            .ToListAsync();
    }

    public async Task<Book?> GetBookWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(b => b.Authors)
            .Include(b => b.Categories)
            .Include(b => b.Reviews)
            .FirstOrDefaultAsync(b => b.BookId == id);
    }

    public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAvailableBooksAsync();

        searchTerm = searchTerm.ToLower();
        return await _dbSet
            .Include(b => b.Authors)
            .Include(b => b.Categories)
            .Where(b => b.Title.ToLower().Contains(searchTerm) || 
                        b.ISBN.Contains(searchTerm) ||
                        b.Authors.Any(a => a.LastName.ToLower().Contains(searchTerm) || a.FirstName.ToLower().Contains(searchTerm)))
            .ToListAsync();
    }

    public async Task<bool> IsISBNUniqueAsync(string isbn, int? excludeBookId = null)
    {
        var query = _dbSet.AsQueryable();
        
        if (excludeBookId.HasValue)
            query = query.Where(b => b.BookId != excludeBookId.Value);
            
        return !await query.AnyAsync(b => b.ISBN == isbn);
    }
}
