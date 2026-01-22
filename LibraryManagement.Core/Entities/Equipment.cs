using LibraryManagement.Core.Enums;

namespace LibraryManagement.Core.Entities;

public class Equipment
{
    public int EquipmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public EquipmentType Type { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public EquipmentStatus Status { get; set; }
    public DateTime PurchaseDate { get; set; }

    public ICollection<EquipmentLoan> Loans { get; set; } = new List<EquipmentLoan>();
}
