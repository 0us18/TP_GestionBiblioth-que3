using LibraryManagement.Application.Services;
using LibraryManagement.Core.Enums;

namespace LibraryManagement.WinForms.Forms.Equipment;

public partial class EquipmentHistoryForm : Form
{
    private readonly EquipmentService _equipmentService;

    public int EquipmentId { get; set; }

    private Label _lblTitle = null!;
    private DataGridView _grid = null!;
    private Button _btnReturn = null!;

    private sealed class LoanGridItem
    {
        public int EquipmentLoanId { get; init; }
        public string User { get; init; } = string.Empty;
        public DateTime LoanDate { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime? ReturnDate { get; init; }
        public string Status { get; init; } = string.Empty;
    }

    public EquipmentHistoryForm(EquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
        InitializeComponent();
        this.Load += async (s, e) => await LoadHistory();
    }

    private void InitializeComponent()
    {
        this.Text = "Historique des prêts (Matériel)";
        this.Size = new Size(900, 520);
        this.StartPosition = FormStartPosition.CenterParent;

        _lblTitle = new Label { Location = new Point(20, 15), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

        _btnReturn = new Button { Text = "Retourner le prêt sélectionné", Location = new Point(20, 45), Width = 220 };
        _btnReturn.Click += async (s, e) => await ReturnSelected();

        _grid = new DataGridView
        {
            Location = new Point(20, 85),
            Size = new Size(840, 370),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true
        };

        this.Controls.AddRange(new Control[] { _lblTitle, _btnReturn, _grid });
    }

    private async Task LoadHistory()
    {
        var equipment = await _equipmentService.GetEquipmentWithLoansAsync(EquipmentId);
        _lblTitle.Text = equipment == null
            ? $"Équipement #{EquipmentId}"
            : $"{equipment.Name} ({equipment.SerialNumber}) - {equipment.Status}";

        if (equipment == null)
        {
            _grid.DataSource = new List<LoanGridItem>();
            return;
        }

        var items = equipment.Loans
            .OrderByDescending(l => l.LoanDate)
            .Select(l => new LoanGridItem
            {
                EquipmentLoanId = l.EquipmentLoanId,
                User = $"{l.User?.LastName} {l.User?.FirstName}".Trim(),
                LoanDate = l.LoanDate,
                DueDate = l.DueDate,
                ReturnDate = l.ReturnDate,
                Status = l.Status.ToString()
            })
            .ToList();

        _grid.DataSource = items;
    }

    private int? GetSelectedLoanId()
    {
        if (_grid.SelectedRows.Count <= 0)
            return null;

        if (_grid.SelectedRows[0].DataBoundItem is not LoanGridItem item)
            return null;

        return item.EquipmentLoanId;
    }

    private async Task ReturnSelected()
    {
        var loanId = GetSelectedLoanId();
        if (!loanId.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un prêt.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            await _equipmentService.ReturnEquipmentAsync(loanId.Value);
            MessageBox.Show("Retour effectué.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadHistory();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Retour impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

