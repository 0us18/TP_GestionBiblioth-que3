using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms.Forms.Loans;

public partial class LoanListForm : Form
{
    private readonly LoanService _loanService;
    private DataGridView _gridLoans = null!;
    private Button _btnEdit = null!;
    private Button _btnDelete = null!;

    private sealed class LoanGridItem
    {
        public int LoanId { get; init; }
        public string User { get; init; } = string.Empty;
        public string Book { get; init; } = string.Empty;
        public DateTime LoanDate { get; init; }
        public DateTime DueDate { get; init; }
        public string Status { get; init; } = string.Empty;
    }

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

        _btnEdit = new Button { Text = "Modifier", Location = new Point(180, 20), Width = 100 };
        _btnEdit.Click += (s, e) => EditSelectedLoan();

        _btnDelete = new Button { Text = "Supprimer", Location = new Point(290, 20), Width = 100 };
        _btnDelete.Click += async (s, e) => await DeleteSelectedLoan();

        this.Controls.Add(_btnEdit);
        this.Controls.Add(_btnDelete);

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
        _gridLoans.DataSource = loans.Select(l => new LoanGridItem
        {
            LoanId = l.LoanId,
            User = (l.User?.LastName + " " + l.User?.FirstName).Trim(),
            Book = l.Book?.Title ?? string.Empty,
            LoanDate = l.LoanDate,
            DueDate = l.DueDate,
            Status = l.Status.ToString()
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

    private int? GetSelectedLoanId()
    {
        if (_gridLoans.SelectedRows.Count <= 0)
            return null;

        if (_gridLoans.SelectedRows[0].DataBoundItem is not LoanGridItem item)
            return null;

        return item.LoanId;
    }

    private void EditSelectedLoan()
    {
        var id = GetSelectedLoanId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un emprunt.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var form = Program.ServiceProvider.GetRequiredService<LoanDetailForm>();
        form.LoanId = id.Value;
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadLoans();
        }
    }

    private async Task DeleteSelectedLoan()
    {
        var id = GetSelectedLoanId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un emprunt.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show("Confirmer la suppression de l'emprunt sélectionné ?", "Confirmation",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes)
            return;

        try
        {
            await _loanService.DeleteLoanAsync(id.Value);
            MessageBox.Show("Emprunt supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadLoans();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Suppression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
