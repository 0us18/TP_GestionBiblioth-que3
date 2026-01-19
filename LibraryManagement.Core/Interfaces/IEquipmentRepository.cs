using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces;

public interface IEquipmentRepository : IRepository<Equipment>
{
    Task<IEnumerable<Equipment>> GetAvailableEquipmentAsync();
    Task<Equipment?> GetEquipmentWithLoansAsync(int id);
}
