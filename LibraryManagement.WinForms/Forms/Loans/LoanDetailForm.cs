using LibraryManagement.Application.Services;
using LibraryManagement.Core.Enums;

namespace LibraryManagement.WinForms.Forms.Loans;

public partial class LoanDetailForm : Form
{
    private readonly LoanService _loanService;
    public int LoanId { get; set; }

    private Label _lblUser = null!;
    private Label _lblBook = null!;
    private DateTimePicker _dtpDueDate = null!;
    private ComboBox _cmbStatus = null!;

    public LoanDetailForm(LoanService loanService)
    {
        _loanService = loanService;
        InitializeComponent();
        this.Load += async (s, e) => await LoadData();
    }

    private void InitializeComponent()
    {
        this.Text = "Modifier l'Emprunt";
        this.Size = new Size(520, 260);
        this.StartPosition = FormStartPosition.CenterParent;

        var lblUserTitle = new Label { Text = "Usager:", Location = new Point(20, 20), AutoSize = true };
        _lblUser = new Label { Location = new Point(120, 20), AutoSize = true };

        var lblBookTitle = new Label { Text = "Livre:", Location = new Point(20, 55), AutoSize = true };
        _lblBook = new Label { Location = new Point(120, 55), AutoSize = true };

        var lblDue = new Label { Text = "Date retour prévue:", Location = new Point(20, 90), AutoSize = true };
        _dtpDueDate = new DateTimePicker { Location = new Point(170, 85), Width = 200, Format = DateTimePickerFormat.Short };

        var lblStatus = new Label { Text = "Statut:", Location = new Point(20, 125), AutoSize = true };
        _cmbStatus = new ComboBox { Location = new Point(170, 120), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        _cmbStatus.DataSource = Enum.GetValues(typeof(LoanStatus));

        var btnSave = new Button { Text = "Enregistrer", Location = new Point(170, 170), Width = 120, DialogResult = DialogResult.None };
        btnSave.Click += async (s, e) => await Save();

        var btnCancel = new Button { Text = "Annuler", Location = new Point(310, 170), Width = 120, DialogResult = DialogResult.Cancel };

        this.Controls.AddRange(new Control[]
        {
            lblUserTitle, _lblUser,
            lblBookTitle, _lblBook,
            lblDue, _dtpDueDate,
            lblStatus, _cmbStatus,
            btnSave, btnCancel
        });
    }

    private async Task LoadData()
    {
        var loan = (await _loanService.GetAllLoansAsync()).FirstOrDefault(l => l.LoanId == LoanId);
        if (loan == null)
        {
            MessageBox.Show("Emprunt introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
            return;
        }

        _lblUser.Text = $"{loan.User?.LastName} {loan.User?.FirstName}".Trim();
        _lblBook.Text = loan.Book?.Title ?? string.Empty;
        _dtpDueDate.Value = loan.DueDate;
        _cmbStatus.SelectedItem = loan.Status;
    }

    private async Task Save()
    {
        try
        {
            var status = (LoanStatus)_cmbStatus.SelectedItem!;
            await _loanService.UpdateLoanAsync(LoanId, _dtpDueDate.Value, status);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

