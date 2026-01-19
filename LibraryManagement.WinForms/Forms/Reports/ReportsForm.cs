using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms.Forms.Reports;

public partial class ReportsForm : Form
{
    private readonly ReportService _reportService;
    private readonly UserService _userService;
    
    private ComboBox _cmbUsers;
    private DataGridView _gridResults;

    public ReportsForm(ReportService reportService, UserService userService)
    {
        _reportService = reportService;
        _userService = userService;
        InitializeComponent();
        LoadUsers();
    }

    private void InitializeComponent()
    {
        this.Text = "Rapports";
        this.Size = new Size(800, 600);

        var lblUser = new Label { Text = "Rapport Emprunts par Usager:", Location = new Point(20, 20), AutoSize = true };
        _cmbUsers = new ComboBox { Location = new Point(200, 18), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        var btnGenerate = new Button { Text = "Générer", Location = new Point(410, 18) };
        btnGenerate.Click += async (s, e) => await GenerateUserReport();

        this.Controls.Add(lblUser);
        this.Controls.Add(_cmbUsers);
        this.Controls.Add(btnGenerate);

        _gridResults = new DataGridView
        {
            Location = new Point(20, 60),
            Size = new Size(740, 480),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true
        };
        this.Controls.Add(_gridResults);
    }

    private async void LoadUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        _cmbUsers.DataSource = users.ToList();
        _cmbUsers.DisplayMember = "LastName";
        _cmbUsers.ValueMember = "UserId";
    }

    private async Task GenerateUserReport()
    {
        if (_cmbUsers.SelectedValue == null) return;

        int userId = (int)_cmbUsers.SelectedValue;
        var loans = await _reportService.GetUserLoansReportAsync(userId);
        
        _gridResults.DataSource = loans.Select(l => new
        {
            l.LoanId,
            Book = l.Book?.Title,
            l.LoanDate,
            l.DueDate,
            l.ReturnDate,
            l.Status,
            l.LateFee
        }).ToList();
    }
}
