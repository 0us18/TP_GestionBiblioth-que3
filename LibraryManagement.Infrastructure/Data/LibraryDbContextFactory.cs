using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryManagement.Infrastructure.Data;

/// <summary>
/// Design-time factory for EF Core tools (dotnet ef migrations / update).
/// </summary>
public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
    public LibraryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();

        // Keep in sync with WinForms Program.cs
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=LibraryManagementDB_Migrated;Trusted_Connection=true;MultipleActiveResultSets=true");

        return new LibraryDbContext(optionsBuilder.Options);
    }
}

