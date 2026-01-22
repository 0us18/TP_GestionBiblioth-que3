using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;

namespace LibraryManagement.WinForms.Forms.Users;

public partial class UserDetailForm : Form
{
    private readonly UserService _userService;
    public int? UserId { get; set; }

    private TextBox _txtFirstName = null!;
    private TextBox _txtLastName = null!;
    private TextBox _txtEmail = null!;
    private TextBox _txtPhone = null!;
    private TextBox _txtAddress = null!;
    private TextBox _txtCity = null!;
    private TextBox _txtPostalCode = null!;
    private ComboBox _cmbUserType = null!;
    private ComboBox _cmbStatus = null!;

    public UserDetailForm(UserService userService)
    {
        _userService = userService;
        InitializeComponent();
        this.Load += async (s, e) => await LoadData();
    }

    private void InitializeComponent()
    {
        this.Text = "Détail de l'Usager";
        this.Size = new Size(520, 520);
        this.StartPosition = FormStartPosition.CenterParent;

        int xLabel = 20;
        int xInput = 170;
        int y = 20;
        int rowH = 35;

        var lblFirstName = new Label { Text = "Prénom:", Location = new Point(xLabel, y), AutoSize = true };
        _txtFirstName = new TextBox { Location = new Point(xInput, y), Width = 300 };
        y += rowH;

        var lblLastName = new Label { Text = "Nom:", Location = new Point(xLabel, y), AutoSize = true };
        _txtLastName = new TextBox { Location = new Point(xInput, y), Width = 300 };
        y += rowH;

        var lblEmail = new Label { Text = "Email:", Location = new Point(xLabel, y), AutoSize = true };
        _txtEmail = new TextBox { Location = new Point(xInput, y), Width = 300 };
        y += rowH;

        var lblPhone = new Label { Text = "Téléphone:", Location = new Point(xLabel, y), AutoSize = true };
        _txtPhone = new TextBox { Location = new Point(xInput, y), Width = 300 };
        y += rowH;

        var lblAddress = new Label { Text = "Adresse:", Location = new Point(xLabel, y), AutoSize = true };
        _txtAddress = new TextBox { Location = new Point(xInput, y), Width = 300 };
        y += rowH;

        var lblCity = new Label { Text = "Ville:", Location = new Point(xLabel, y), AutoSize = true };
        _txtCity = new TextBox { Location = new Point(xInput, y), Width = 300 };
        y += rowH;

        var lblPostal = new Label { Text = "Code Postal:", Location = new Point(xLabel, y), AutoSize = true };
        _txtPostalCode = new TextBox { Location = new Point(xInput, y), Width = 150 };
        y += rowH;

        var lblType = new Label { Text = "Type:", Location = new Point(xLabel, y), AutoSize = true };
        _cmbUserType = new ComboBox { Location = new Point(xInput, y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        _cmbUserType.DataSource = Enum.GetValues(typeof(UserType));
        y += rowH;

        var lblStatus = new Label { Text = "Statut:", Location = new Point(xLabel, y), AutoSize = true };
        _cmbStatus = new ComboBox { Location = new Point(xInput, y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        _cmbStatus.DataSource = Enum.GetValues(typeof(UserStatus));
        y += rowH + 10;

        var btnSave = new Button { Text = "Enregistrer", Location = new Point(xInput, y), Width = 120, DialogResult = DialogResult.None };
        btnSave.Click += async (s, e) => await SaveUser();

        var btnCancel = new Button { Text = "Annuler", Location = new Point(xInput + 140, y), Width = 120, DialogResult = DialogResult.Cancel };

        this.Controls.AddRange(new Control[]
        {
            lblFirstName, _txtFirstName,
            lblLastName, _txtLastName,
            lblEmail, _txtEmail,
            lblPhone, _txtPhone,
            lblAddress, _txtAddress,
            lblCity, _txtCity,
            lblPostal, _txtPostalCode,
            lblType, _cmbUserType,
            lblStatus, _cmbStatus,
            btnSave, btnCancel
        });
    }

    private async Task LoadData()
    {
        if (!UserId.HasValue)
        {
            _cmbStatus.SelectedItem = UserStatus.Active;
            return;
        }

        var user = await _userService.GetUserByIdAsync(UserId.Value);
        if (user == null)
            return;

        _txtFirstName.Text = user.FirstName;
        _txtLastName.Text = user.LastName;
        _txtEmail.Text = user.Email;
        _txtPhone.Text = user.Phone;
        _txtAddress.Text = user.Address;
        _txtCity.Text = user.City;
        _txtPostalCode.Text = user.PostalCode;
        _cmbUserType.SelectedItem = user.UserType;
        _cmbStatus.SelectedItem = user.Status;
    }

    private async Task SaveUser()
    {
        try
        {
            var user = new User
            {
                FirstName = _txtFirstName.Text,
                LastName = _txtLastName.Text,
                Email = _txtEmail.Text,
                Phone = _txtPhone.Text,
                Address = _txtAddress.Text,
                City = _txtCity.Text,
                PostalCode = _txtPostalCode.Text,
                UserType = (UserType)_cmbUserType.SelectedItem!,
                Status = (UserStatus)_cmbStatus.SelectedItem!
            };

            if (UserId.HasValue)
            {
                user.UserId = UserId.Value;
                await _userService.UpdateUserAsync(user);
            }
            else
            {
                await _userService.AddUserAsync(user);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

