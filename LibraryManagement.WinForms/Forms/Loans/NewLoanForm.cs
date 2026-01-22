using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;

namespace LibraryManagement.WinForms.Forms.Loans;

public partial class NewLoanForm : Form
{
    private readonly LoanService _loanService;
    private readonly UserService _userService;
    private readonly BookService _bookService;

    private ComboBox _cmbUsers;
    private ComboBox _cmbBooks;

    public NewLoanForm(LoanService loanService, UserService userService, BookService bookService)
    {
        _loanService = loanService;
        _userService = userService;
        _bookService = bookService;
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        this.Text = "Nouvel Emprunt";
        this.Size = new Size(400, 300);
        this.StartPosition = FormStartPosition.CenterParent;

        var lblUser = new Label { Text = "Usager:", Location = new Point(20, 20) };
        _cmbUsers = new ComboBox { Location = new Point(120, 20), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

        var lblBook = new Label { Text = "Livre:", Location = new Point(20, 60) };
        _cmbBooks = new ComboBox { Location = new Point(120, 60), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

        var btnSave = new Button { Text = "Créer Emprunt", Location = new Point(120, 120), Width = 120, DialogResult = DialogResult.None };
        btnSave.Click += async (s, e) => await CreateLoan();

        this.Controls.AddRange(new Control[] { lblUser, _cmbUsers, lblBook, _cmbBooks, btnSave });
    }

    private async void LoadData()
    {
        var users = await _userService.GetAllUsersAsync();
        _cmbUsers.DataSource = users.ToList();
        _cmbUsers.DisplayMember = "LastName";
        _cmbUsers.ValueMember = "UserId";

        var books = await _bookService.GetAllBooksAsync();
        _cmbBooks.DataSource = books.Where(b => b.AvailableCopies > 0).ToList();
        _cmbBooks.DisplayMember = "Title";
        _cmbBooks.ValueMember = "BookId";
    }

    private async Task CreateLoan()
    {
        try
        {
            if (_cmbUsers.SelectedValue == null || _cmbBooks.SelectedValue == null)
            {
                MessageBox.Show("Veuillez sélectionner un usager et un livre.");
                return;
            }

            int userId = (int)_cmbUsers.SelectedValue;
            int bookId = (int)_cmbBooks.SelectedValue;

            await _loanService.CreateLoanAsync(userId, bookId);
            
            MessageBox.Show("Emprunt créé avec succès!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
