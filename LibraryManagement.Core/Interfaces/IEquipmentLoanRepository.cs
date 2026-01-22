using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces;

public interface IEquipmentLoanRepository : IRepository<EquipmentLoan>
{
    Task<EquipmentLoan?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<EquipmentLoan>> GetLoansByEquipmentIdAsync(int equipmentId);
}

