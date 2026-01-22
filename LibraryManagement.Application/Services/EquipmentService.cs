using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Application.Services;

public class EquipmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public EquipmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
    {
        return await _unitOfWork.Equipment.GetAllAsync();
    }

    public async Task<IEnumerable<Equipment>> GetAvailableEquipmentAsync()
    {
        return await _unitOfWork.Equipment.GetAvailableEquipmentAsync();
    }

    public async Task<Equipment?> GetEquipmentByIdAsync(int id)
    {
        return await _unitOfWork.Equipment.GetByIdAsync(id);
    }

    public async Task<Equipment?> GetEquipmentWithLoansAsync(int id)
    {
        return await _unitOfWork.Equipment.GetEquipmentWithLoansAsync(id);
    }

    public async Task AddEquipmentAsync(Equipment equipment)
    {
        if (string.IsNullOrWhiteSpace(equipment.Name))
            throw new ArgumentException("Le nom est requis.");

        if (string.IsNullOrWhiteSpace(equipment.SerialNumber))
            throw new ArgumentException("Le numéro de série est requis.");

        var existing = await _unitOfWork.Equipment.FindAsync(e => e.SerialNumber == equipment.SerialNumber);
        if (existing.Any())
            throw new ArgumentException("Le numéro de série doit être unique.");

        await _unitOfWork.Equipment.AddAsync(equipment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateEquipmentAsync(Equipment equipment)
    {
        var existing = await _unitOfWork.Equipment.GetByIdAsync(equipment.EquipmentId);
        if (existing == null)
            throw new ArgumentException("Équipement introuvable.");

        if (string.IsNullOrWhiteSpace(equipment.Name))
            throw new ArgumentException("Le nom est requis.");

        if (string.IsNullOrWhiteSpace(equipment.SerialNumber))
            throw new ArgumentException("Le numéro de série est requis.");

        var serialConflicts = await _unitOfWork.Equipment.FindAsync(e =>
            e.SerialNumber == equipment.SerialNumber && e.EquipmentId != equipment.EquipmentId);
        if (serialConflicts.Any())
            throw new ArgumentException("Le numéro de série doit être unique.");

        existing.Name = equipment.Name;
        existing.Type = equipment.Type;
        existing.SerialNumber = equipment.SerialNumber;
        existing.Status = equipment.Status;
        existing.PurchaseDate = equipment.PurchaseDate;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteEquipmentAsync(int equipmentId)
    {
        var equipment = await _unitOfWork.Equipment.GetEquipmentWithLoansAsync(equipmentId);
        if (equipment == null)
            throw new ArgumentException("Équipement introuvable.");

        if (equipment.Loans.Any())
            throw new InvalidOperationException("Impossible de supprimer un équipement qui a un historique de prêts.");

        _unitOfWork.Equipment.Remove(equipment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task LoanEquipmentAsync(int equipmentId, int userId, DateTime? dueDate = null)
    {
        var equipment = await _unitOfWork.Equipment.GetByIdAsync(equipmentId);
        if (equipment == null)
            throw new ArgumentException("Équipement introuvable.");

        if (equipment.Status != EquipmentStatus.Available)
            throw new InvalidOperationException("L'équipement n'est pas disponible.");

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null || user.Status != UserStatus.Active)
            throw new InvalidOperationException("L'usager n'est pas actif ou n'existe pas.");

        var loan = new EquipmentLoan
        {
            EquipmentId = equipmentId,
            UserId = userId,
            LoanDate = DateTime.Now,
            DueDate = dueDate ?? DateTime.Now.AddDays(7),
            Status = LoanStatus.InProgress
        };

        equipment.Status = EquipmentStatus.OnLoan;
        
        await _unitOfWork.EquipmentLoans.AddAsync(loan);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ReturnEquipmentAsync(int equipmentLoanId)
    {
        var loan = await _unitOfWork.EquipmentLoans.GetByIdWithDetailsAsync(equipmentLoanId);
        if (loan == null)
            throw new ArgumentException("Prêt d'équipement introuvable.");

        if (loan.Status == LoanStatus.Returned)
            throw new InvalidOperationException("Ce prêt est déjà retourné.");

        loan.ReturnDate = DateTime.Now;
        loan.Status = LoanStatus.Returned;

        var equipment = loan.Equipment ?? await _unitOfWork.Equipment.GetByIdAsync(loan.EquipmentId);
        if (equipment != null && equipment.Status == EquipmentStatus.OnLoan)
        {
            equipment.Status = EquipmentStatus.Available;
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
