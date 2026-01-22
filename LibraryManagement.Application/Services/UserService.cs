using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Application.Services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _unitOfWork.Users.GetAllAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _unitOfWork.Users.GetUserWithLoansAsync(id);
    }

    public async Task AddUserAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("L'email est requis.");

        if (!await _unitOfWork.Users.IsEmailUniqueAsync(user.Email))
            throw new ArgumentException("L'email doit être unique.");

        user.RegistrationDate = DateTime.Now;
        user.Status = UserStatus.Active;

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        var existingUser = await _unitOfWork.Users.GetByIdAsync(user.UserId);
        if (existingUser == null)
            throw new ArgumentException("Usager introuvable.");

        if (!await _unitOfWork.Users.IsEmailUniqueAsync(user.Email, user.UserId))
            throw new ArgumentException("L'email doit être unique.");

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.Phone = user.Phone;
        existingUser.Address = user.Address;
        existingUser.City = user.City;
        existingUser.PostalCode = user.PostalCode;
        existingUser.UserType = user.UserType;
        existingUser.Status = user.Status;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeactivateUserAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new ArgumentException("Usager introuvable.");

        user.Status = UserStatus.Inactive;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _unitOfWork.Users.GetUserWithLoansAsync(id);
        if (user == null)
            throw new ArgumentException("Usager introuvable.");

        if (user.Loans.Any())
            throw new InvalidOperationException("Impossible de supprimer un usager qui a des emprunts associés.");

        if (user.Participations.Any())
            throw new InvalidOperationException("Impossible de supprimer un usager qui a des participations associées.");

        if (user.Reviews.Any())
            throw new InvalidOperationException("Impossible de supprimer un usager qui a des évaluations (reviews) associées.");

        if (user.EquipmentLoans.Any())
            throw new InvalidOperationException("Impossible de supprimer un usager qui a des prêts d'équipement associés.");

        _unitOfWork.Users.Remove(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
