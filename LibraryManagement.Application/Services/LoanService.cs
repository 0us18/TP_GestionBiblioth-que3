using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Application.Services;

public class LoanService
{
    private readonly IUnitOfWork _unitOfWork;
    private const int MaxLoansPerUser = 5;
    private const int LoanPeriodDays = 14;
    private const decimal DailyLateFee = 0.50m;

    public LoanService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Loan>> GetAllLoansAsync()
    {
        return await _unitOfWork.Loans.GetAllAsync();
    }

    public async Task<IEnumerable<Loan>> GetLoansByUserAsync(int userId)
    {
        return await _unitOfWork.Loans.GetLoansByUserIdAsync(userId);
    }

    public async Task CreateLoanAsync(int userId, int bookId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null || user.Status != UserStatus.Active)
            throw new InvalidOperationException("L'usager n'est pas actif ou n'existe pas.");

        var activeLoansCount = await _unitOfWork.Loans.GetActiveLoanCountForUserAsync(userId);
        if (activeLoansCount >= MaxLoansPerUser)
            throw new InvalidOperationException($"L'usager a atteint la limite de {MaxLoansPerUser} emprunts actifs.");

        var book = await _unitOfWork.Books.GetByIdAsync(bookId);
        if (book == null)
            throw new ArgumentException("Livre introuvable.");

        if (book.AvailableCopies <= 0)
            throw new InvalidOperationException("Aucune copie disponible pour ce livre.");

        var loan = new Loan
        {
            UserId = userId,
            BookId = bookId,
            LoanDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(LoanPeriodDays),
            Status = LoanStatus.InProgress
        };

        book.AvailableCopies--;

        await _unitOfWork.Loans.AddAsync(loan);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ReturnLoanAsync(int loanId)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(loanId);
        if (loan == null)
            throw new ArgumentException("Emprunt introuvable.");

        if (loan.Status == LoanStatus.Returned)
            throw new InvalidOperationException("Cet emprunt est déjà retourné.");

        loan.ReturnDate = DateTime.Now;
        loan.Status = LoanStatus.Returned;

        if (DateTime.Now > loan.DueDate)
        {
            var overdueDays = (DateTime.Now - loan.DueDate).Days;
            loan.LateFee = overdueDays * DailyLateFee;
            loan.Status = LoanStatus.Returned;
        }

        var book = await _unitOfWork.Books.GetByIdAsync(loan.BookId);
        if (book != null)
        {
            book.AvailableCopies++;
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateLoanAsync(int loanId, DateTime dueDate, LoanStatus status)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(loanId);
        if (loan == null)
            throw new ArgumentException("Emprunt introuvable.");

        if (status == LoanStatus.Returned && loan.Status != LoanStatus.Returned)
        {
            await ReturnLoanAsync(loanId);
            return;
        }

        loan.DueDate = dueDate;
        loan.Status = status;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteLoanAsync(int loanId)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(loanId);
        if (loan == null)
            throw new ArgumentException("Emprunt introuvable.");

        if (loan.Status == LoanStatus.InProgress)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(loan.BookId);
            if (book != null)
                book.AvailableCopies++;
        }

        _unitOfWork.Loans.Remove(loan);
        await _unitOfWork.SaveChangesAsync();
    }
}
