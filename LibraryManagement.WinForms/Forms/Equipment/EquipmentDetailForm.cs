using LibraryManagement.Application.Services;
using EquipmentEntity = LibraryManagement.Core.Entities.Equipment;
using LibraryManagement.Core.Enums;

namespace LibraryManagement.WinForms.Forms.Equipment;

public partial class EquipmentDetailForm : Form
{
    private readonly EquipmentService _equipmentService;
    public int? EquipmentId { get; set; }

    private TextBox _txtName = null!;
    private ComboBox _cmbType = null!;
    private TextBox _txtSerial = null!;
    private ComboBox _cmbStatus = null!;
    private DateTimePicker _dtpPurchase = null!;

    public EquipmentDetailForm(EquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
        InitializeComponent();
        this.Load += async (s, e) => await LoadData();
    }

    private void InitializeComponent()
    {
        this.Text = "Détail de l'Équipement";
        this.Size = new Size(520, 360);
        this.StartPosition = FormStartPosition.CenterParent;

        int xLabel = 20;
        int xInput = 170;
        int y = 20;
        int rowH = 35;

        var lblName = new Label { Text = "Nom:", Location = new Point(xLabel, y), AutoSize = true };
        _txtName = new TextBox { Location = new Point(xInput, y), Width = 300 };
        y += rowH;

        var lblType = new Label { Text = "Type:", Location = new Point(xLabel, y), AutoSize = true };
        _cmbType = new ComboBox { Location = new Point(xInput, y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        _cmbType.DataSource = Enum.GetValues(typeof(EquipmentType));
        y += rowH;

        var lblSerial = new Label { Text = "No. Série:", Location = new Point(xLabel, y), AutoSize = true };
        _txtSerial = new TextBox { Location = new Point(xInput, y), Width = 200 };
        y += rowH;

        var lblStatus = new Label { Text = "Statut:", Location = new Point(xLabel, y), AutoSize = true };
        _cmbStatus = new ComboBox { Location = new Point(xInput, y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        _cmbStatus.DataSource = Enum.GetValues(typeof(EquipmentStatus));
        y += rowH;

        var lblPurchase = new Label { Text = "Date d'achat:", Location = new Point(xLabel, y), AutoSize = true };
        _dtpPurchase = new DateTimePicker { Location = new Point(xInput, y), Width = 200, Format = DateTimePickerFormat.Short };
        y += rowH + 10;

        var btnSave = new Button { Text = "Enregistrer", Location = new Point(xInput, y), Width = 120, DialogResult = DialogResult.None };
        btnSave.Click += async (s, e) => await Save();

        var btnCancel = new Button { Text = "Annuler", Location = new Point(xInput + 140, y), Width = 120, DialogResult = DialogResult.Cancel };

        this.Controls.AddRange(new Control[]
        {
            lblName, _txtName,
            lblType, _cmbType,
            lblSerial, _txtSerial,
            lblStatus, _cmbStatus,
            lblPurchase, _dtpPurchase,
            btnSave, btnCancel
        });
    }

    private async Task LoadData()
    {
        if (!EquipmentId.HasValue)
        {
            _cmbStatus.SelectedItem = EquipmentStatus.Available;
            _cmbType.SelectedItem = EquipmentType.Other;
            _dtpPurchase.Value = DateTime.Today;
            return;
        }

        var equipment = await _equipmentService.GetEquipmentByIdAsync(EquipmentId.Value);
        if (equipment == null)
            return;

        _txtName.Text = equipment.Name;
        _cmbType.SelectedItem = equipment.Type;
        _txtSerial.Text = equipment.SerialNumber;
        _cmbStatus.SelectedItem = equipment.Status;
        _dtpPurchase.Value = equipment.PurchaseDate == default ? DateTime.Today : equipment.PurchaseDate;
    }

    private async Task Save()
    {
        try
        {
            var equipment = new EquipmentEntity
            {
                Name = _txtName.Text,
                Type = (EquipmentType)_cmbType.SelectedItem!,
                SerialNumber = _txtSerial.Text,
                Status = (EquipmentStatus)_cmbStatus.SelectedItem!,
                PurchaseDate = _dtpPurchase.Value.Date
            };

            if (EquipmentId.HasValue)
            {
                equipment.EquipmentId = EquipmentId.Value;
                await _equipmentService.UpdateEquipmentAsync(equipment);
            }
            else
            {
                await _equipmentService.AddEquipmentAsync(equipment);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

