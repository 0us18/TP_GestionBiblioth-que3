using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms.Forms.Loans;

public partial class LoanListForm : Form
{
    private readonly LoanService _loanService;
    private DataGridView _gridLoans;

    public LoanListForm(LoanService loanService)
    {
        _loanService = loanService;
        InitializeComponent();
        LoadLoans();
    }

    private void InitializeComponent()
    {
        this.Text = "Gestion des Emprunts";
        this.Size = new Size(900, 500);

        var btnNewLoan = new Button { Text = "Nouvel Emprunt", Location = new Point(20, 20), Width = 150 };
        btnNewLoan.Click += (s, e) => OpenNewLoan();
        this.Controls.Add(btnNewLoan);

        // Grid
        _gridLoans = new DataGridView
        {
            Location = new Point(20, 60),
            Size = new Size(840, 380),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true
        };

        this.Controls.Add(_gridLoans);
    }

    private async void LoadLoans()
    {
        var loans = await _loanService.GetAllLoansAsync();
        _gridLoans.DataSource = loans.Select(l => new 
        {
            l.LoanId,
            User = l.User?.LastName + " " + l.User?.FirstName,
            Book = l.Book?.Title,
            l.LoanDate,
            l.DueDate,
            l.Status
        }).ToList();
    }

    private void OpenNewLoan()
    {
        var form = Program.ServiceProvider.GetRequiredService<NewLoanForm>();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadLoans();
        }
    }
}
