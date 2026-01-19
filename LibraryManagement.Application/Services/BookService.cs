using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Application.Services;

public class BookService
{
    private readonly IUnitOfWork _unitOfWork;

    public BookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _unitOfWork.Books.GetAllAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _unitOfWork.Books.GetBookWithDetailsAsync(id);
    }

    public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
    {
        return await _unitOfWork.Books.SearchBooksAsync(searchTerm);
    }

    public async Task AddBookAsync(Book book)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(book.Title))
            throw new ArgumentException("Le titre est requis.");

        if (!await _unitOfWork.Books.IsISBNUniqueAsync(book.ISBN))
            throw new ArgumentException("L'ISBN doit être unique.");

        if (book.AvailableCopies < 0)
            throw new ArgumentException("Le nombre de copies ne peut pas être négatif.");

        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(Book book)
    {
        var existingBook = await _unitOfWork.Books.GetByIdAsync(book.BookId);
        if (existingBook == null)
            throw new ArgumentException("Livre introuvable.");

        if (!await _unitOfWork.Books.IsISBNUniqueAsync(book.ISBN, book.BookId))
            throw new ArgumentException("L'ISBN doit être unique.");

        // Update properties
        existingBook.Title = book.Title;
        existingBook.ISBN = book.ISBN;
        existingBook.PublicationYear = book.PublicationYear;
        existingBook.AvailableCopies = book.AvailableCopies;
        
        // Note: Authors and Categories update would require more complex logic handling relationships
        // For simplicity in this scope, we assume basic property updates or handled via UI specific logic

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(id);
        if (book == null)
            throw new ArgumentException("Livre introuvable.");

        // Check for active loans
        var loans = await _unitOfWork.Loans.FindAsync(l => l.BookId == id);
        if (loans.Any())
            throw new InvalidOperationException("Impossible de supprimer un livre qui a des emprunts associés.");

        _unitOfWork.Books.Remove(book);
        await _unitOfWork.SaveChangesAsync();
    }
}
