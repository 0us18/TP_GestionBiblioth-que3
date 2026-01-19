using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Application.Services;

public class ReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Loan>> GetUserLoansReportAsync(int userId)
    {
        return await _unitOfWork.Loans.GetLoansByUserIdAsync(userId);
    }

    public async Task<Activity?> GetActivityParticipationReportAsync(int activityId)
    {
        return await _unitOfWork.Activities.GetActivityWithParticipantsAsync(activityId);
    }

    public async Task<Book?> GetBookReviewsReportAsync(int bookId)
    {
        return await _unitOfWork.Books.GetBookWithDetailsAsync(bookId);
    }
}
