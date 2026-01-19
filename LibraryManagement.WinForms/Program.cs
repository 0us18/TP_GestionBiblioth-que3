using LibraryManagement.Application.Services;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories;
using LibraryManagement.WinForms.Forms;
using LibraryManagement.WinForms.Forms.Books;
using LibraryManagement.WinForms.Forms.Users;
using LibraryManagement.WinForms.Forms.Loans;
using LibraryManagement.WinForms.Forms.Activities;
using LibraryManagement.WinForms.Forms.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms;

public static class Program
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    [STAThread]
    static void Main()
    {
        // Manual initialization instead of ApplicationConfiguration
        System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        // Seed Database
        using (var scope = ServiceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            DbInitializer.Initialize(context);
        }

        var mainForm = ServiceProvider.GetRequiredService<MainForm>();
        System.Windows.Forms.Application.Run(mainForm);
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        // Database
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LibraryManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"));

        // Repositories
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<BookService>();
        services.AddScoped<UserService>();
        services.AddScoped<LoanService>();
        services.AddScoped<ActivityService>();
        services.AddScoped<EquipmentService>();
        services.AddScoped<ReportService>();

        // Forms
        services.AddTransient<MainForm>();
        services.AddTransient<BookListForm>();
        services.AddTransient<BookDetailForm>();
        services.AddTransient<UserListForm>();
        services.AddTransient<LoanListForm>();
        services.AddTransient<NewLoanForm>();
        services.AddTransient<ActivityListForm>();
        services.AddTransient<ActivityDetailForm>();
        services.AddTransient<ReportsForm>();
    }
}
