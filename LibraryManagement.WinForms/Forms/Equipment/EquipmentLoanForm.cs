using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.WinForms.Forms.Equipment;

public partial class EquipmentLoanForm : Form
{
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;

    public int EquipmentId { get; set; }

    private Label _lblEquipment = null!;
    private ComboBox _cmbUsers = null!;
    private DateTimePicker _dtpDueDate = null!;

    public EquipmentLoanForm(EquipmentService equipmentService, UserService userService)
    {
        _equipmentService = equipmentService;
        _userService = userService;
        InitializeComponent();
        this.Load += async (s, e) => await LoadData();
    }

    private void InitializeComponent()
    {
        this.Text = "Emprunter un Équipement";
        this.Size = new Size(520, 260);
        this.StartPosition = FormStartPosition.CenterParent;

        var lblEqTitle = new Label { Text = "Équipement:", Location = new Point(20, 20), AutoSize = true };
        _lblEquipment = new Label { Location = new Point(140, 20), AutoSize = true };

        var lblUser = new Label { Text = "Usager:", Location = new Point(20, 60), AutoSize = true };
        _cmbUsers = new ComboBox { Location = new Point(140, 55), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

        var lblDue = new Label { Text = "Date retour prévue:", Location = new Point(20, 100), AutoSize = true };
        _dtpDueDate = new DateTimePicker { Location = new Point(140, 95), Width = 200, Format = DateTimePickerFormat.Short };

        var btnSave = new Button { Text = "Créer prêt", Location = new Point(140, 150), Width = 120, DialogResult = DialogResult.None };
        btnSave.Click += async (s, e) => await CreateLoan();

        var btnCancel = new Button { Text = "Annuler", Location = new Point(280, 150), Width = 120, DialogResult = DialogResult.Cancel };

        this.Controls.AddRange(new Control[]
        {
            lblEqTitle, _lblEquipment,
            lblUser, _cmbUsers,
            lblDue, _dtpDueDate,
            btnSave, btnCancel
        });
    }

    private async Task LoadData()
    {
        var equipment = await _equipmentService.GetEquipmentByIdAsync(EquipmentId);
        _lblEquipment.Text = equipment == null ? $"#{EquipmentId}" : $"{equipment.Name} ({equipment.SerialNumber})";

        var users = await _userService.GetAllUsersAsync();
        _cmbUsers.DataSource = users.ToList();
        _cmbUsers.DisplayMember = "LastName";
        _cmbUsers.ValueMember = "UserId";

        _dtpDueDate.Value = DateTime.Today.AddDays(7);
    }

    private async Task CreateLoan()
    {
        try
        {
            if (_cmbUsers.SelectedValue == null)
            {
                MessageBox.Show("Veuillez sélectionner un usager.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var userId = (int)_cmbUsers.SelectedValue;
            await _equipmentService.LoanEquipmentAsync(EquipmentId, userId, _dtpDueDate.Value.Date);

            MessageBox.Show("Prêt créé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

