using LibraryManagement.Application.Services;
using LibraryManagement.WinForms.Forms.Books;
using LibraryManagement.WinForms.Forms.Users;
using LibraryManagement.WinForms.Forms.Loans;
using LibraryManagement.WinForms.Forms.Activities;
using LibraryManagement.WinForms.Forms.Reports;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

public partial class MainForm : Form
{
    private readonly BookService _bookService;
    private readonly LoanService _loanService;
    private readonly ActivityService _activityService;

    public MainForm(BookService bookService, LoanService loanService, ActivityService activityService)
    {
        _bookService = bookService;
        _loanService = loanService;
        _activityService = activityService;
        
        InitializeComponent();
        LoadDashboardStats();
    }

    private void InitializeComponent()
    {
        this.Text = "Système de Gestion de Bibliothèque";
        this.Size = new System.Drawing.Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Simple Dashboard UI
        var labelWelcome = new Label
        {
            Text = "Bienvenue dans le système de gestion",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            Location = new Point(20, 20),
            AutoSize = true
        };
        this.Controls.Add(labelWelcome);

        // Buttons for Modules
        var btnBooks = CreateMenuButton("Gestion Livres", 20, 80, (s, e) => OpenBookList());
        var btnUsers = CreateMenuButton("Gestion Usagers", 20, 130, (s, e) => OpenUserList());
        var btnLoans = CreateMenuButton("Gestion Emprunts", 20, 180, (s, e) => OpenLoanList());
        var btnActivities = CreateMenuButton("Activités", 20, 230, (s, e) => OpenActivityList());
        var btnReports = CreateMenuButton("Rapports", 20, 280, (s, e) => OpenReports());

        this.Controls.Add(btnBooks);
        this.Controls.Add(btnUsers);
        this.Controls.Add(btnLoans);
        this.Controls.Add(btnActivities);
        this.Controls.Add(btnReports);
    }

    private Button CreateMenuButton(string text, int x, int y, EventHandler onClick)
    {
        var btn = new Button
        {
            Text = text,
            Location = new Point(x, y),
            Size = new Size(200, 40),
            Font = new Font("Segoe UI", 10)
        };
        btn.Click += onClick;
        return btn;
    }

    private async void LoadDashboardStats()
    {
        // Load stats asynchronously
        // var books = await _bookService.GetAllBooksAsync();
        // Update labels...
    }

    private void OpenBookList()
    {
        var form = Program.ServiceProvider.GetRequiredService<BookListForm>();
        form.ShowDialog();
    }

    private void OpenUserList()
    {
        var form = Program.ServiceProvider.GetRequiredService<UserListForm>();
        form.ShowDialog();
    }

    private void OpenLoanList()
    {
        var form = Program.ServiceProvider.GetRequiredService<LoanListForm>();
        form.ShowDialog();
    }

    private void OpenActivityList()
    {
        var form = Program.ServiceProvider.GetRequiredService<ActivityListForm>();
        form.ShowDialog();
    }

    private void OpenReports()
    {
        var form = Program.ServiceProvider.GetRequiredService<ReportsForm>();
        form.ShowDialog();
    }
}
