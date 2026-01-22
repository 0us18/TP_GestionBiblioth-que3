using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms.Forms.Equipment;

public partial class EquipmentListForm : Form
{
    private readonly EquipmentService _equipmentService;

    private DataGridView _gridEquipment = null!;
    private Button _btnEdit = null!;
    private Button _btnDelete = null!;

    private sealed class EquipmentGridItem
    {
        public int EquipmentId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string SerialNumber { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime PurchaseDate { get; init; }
    }

    public EquipmentListForm(EquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
        InitializeComponent();
        LoadEquipment();
    }

    private void InitializeComponent()
    {
        this.Text = "Gestion du Matériel";
        this.Size = new Size(980, 520);

        var btnAdd = new Button { Text = "Nouvel Équipement", Location = new Point(20, 20), Width = 150 };
        btnAdd.Click += (s, e) => OpenEquipmentDetail(null);

        _btnEdit = new Button { Text = "Modifier", Location = new Point(180, 20), Width = 100 };
        _btnEdit.Click += (s, e) => EditSelectedEquipment();

        _btnDelete = new Button { Text = "Supprimer", Location = new Point(290, 20), Width = 100 };
        _btnDelete.Click += async (s, e) => await DeleteSelectedEquipment();

        var btnLoan = new Button { Text = "Emprunter", Location = new Point(400, 20), Width = 100 };
        btnLoan.Click += (s, e) => LoanSelectedEquipment();

        var btnHistory = new Button { Text = "Historique / Retours", Location = new Point(510, 20), Width = 160 };
        btnHistory.Click += (s, e) => OpenHistoryForSelectedEquipment();

        this.Controls.Add(btnAdd);
        this.Controls.Add(_btnEdit);
        this.Controls.Add(_btnDelete);
        this.Controls.Add(btnLoan);
        this.Controls.Add(btnHistory);

        _gridEquipment = new DataGridView
        {
            Location = new Point(20, 60),
            Size = new Size(920, 400),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true
        };
        _gridEquipment.DoubleClick += (s, e) =>
        {
            var id = GetSelectedEquipmentId();
            if (id.HasValue)
                OpenEquipmentDetail(id.Value);
        };

        this.Controls.Add(_gridEquipment);
    }

    private async void LoadEquipment()
    {
        var items = (await _equipmentService.GetAllEquipmentAsync())
            .Select(e => new EquipmentGridItem
            {
                EquipmentId = e.EquipmentId,
                Name = e.Name,
                Type = e.Type.ToString(),
                SerialNumber = e.SerialNumber,
                Status = e.Status.ToString(),
                PurchaseDate = e.PurchaseDate
            })
            .ToList();

        _gridEquipment.DataSource = items;
    }

    private int? GetSelectedEquipmentId()
    {
        if (_gridEquipment.SelectedRows.Count <= 0)
            return null;

        if (_gridEquipment.SelectedRows[0].DataBoundItem is not EquipmentGridItem item)
            return null;

        return item.EquipmentId;
    }

    private void OpenEquipmentDetail(int? equipmentId)
    {
        var form = Program.ServiceProvider.GetRequiredService<EquipmentDetailForm>();
        form.EquipmentId = equipmentId;
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadEquipment();
        }
    }

    private void EditSelectedEquipment()
    {
        var id = GetSelectedEquipmentId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un équipement.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        OpenEquipmentDetail(id.Value);
    }

    private async Task DeleteSelectedEquipment()
    {
        var id = GetSelectedEquipmentId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un équipement.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show("Confirmer la suppression de l'équipement sélectionné ?", "Confirmation",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes)
            return;

        try
        {
            await _equipmentService.DeleteEquipmentAsync(id.Value);
            MessageBox.Show("Équipement supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadEquipment();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Suppression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoanSelectedEquipment()
    {
        var id = GetSelectedEquipmentId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un équipement.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var form = Program.ServiceProvider.GetRequiredService<EquipmentLoanForm>();
        form.EquipmentId = id.Value;
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadEquipment();
        }
    }

    private void OpenHistoryForSelectedEquipment()
    {
        var id = GetSelectedEquipmentId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un équipement.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var form = Program.ServiceProvider.GetRequiredService<EquipmentHistoryForm>();
        form.EquipmentId = id.Value;
        form.ShowDialog();
        LoadEquipment();
    }
}

