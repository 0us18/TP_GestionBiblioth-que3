using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;

namespace LibraryManagement.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(LibraryDbContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Check if DB has been seeded
        if (context.Books.Any())
        {
            return;   // DB has been seeded
        }

        // 1. Authors
        var authors = new Author[]
        {
            new Author { FirstName = "Victor", LastName = "Hugo", Biography = "Écrivain français", BirthDate = new DateTime(1802, 2, 26) },
            new Author { FirstName = "J.K.", LastName = "Rowling", Biography = "Autrice britannique", BirthDate = new DateTime(1965, 7, 31) },
            new Author { FirstName = "George", LastName = "Orwell", Biography = "Écrivain anglais", BirthDate = new DateTime(1903, 6, 25) },
            new Author { FirstName = "Agatha", LastName = "Christie", Biography = "Romancière", BirthDate = new DateTime(1890, 9, 15) },
            new Author { FirstName = "Stephen", LastName = "King", Biography = "Maître de l'horreur", BirthDate = new DateTime(1947, 9, 21) }
        };
        context.Authors.AddRange(authors);
        context.SaveChanges();

        // 2. Categories
        var categories = new Category[]
        {
            new Category { Name = "Roman", Description = "Fiction narrative" },
            new Category { Name = "Science-Fiction", Description = "Futuriste et technologique" },
            new Category { Name = "Policier", Description = "Enquêtes et mystères" },
            new Category { Name = "Fantastique", Description = "Magie et surnaturel" },
            new Category { Name = "Biographie", Description = "Récit de vie" }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        // 3. Books
        var books = new Book[]
        {
            new Book { Title = "Les Misérables", ISBN = "9780140444308", PublicationYear = 1862, AvailableCopies = 5 },
            new Book { Title = "Harry Potter à l'école des sorciers", ISBN = "9780747532743", PublicationYear = 1997, AvailableCopies = 10 },
            new Book { Title = "1984", ISBN = "9780451524935", PublicationYear = 1949, AvailableCopies = 8 },
            new Book { Title = "Le Crime de l'Orient-Express", ISBN = "9780007119318", PublicationYear = 1934, AvailableCopies = 4 },
            new Book { Title = "Ça", ISBN = "9781501142970", PublicationYear = 1986, AvailableCopies = 3 }
        };

        // Link Books to Authors and Categories
        books[0].Authors.Add(authors[0]); // Hugo -> Les Misérables
        books[0].Categories.Add(categories[0]); // Roman

        books[1].Authors.Add(authors[1]); // Rowling -> HP
        books[1].Categories.Add(categories[3]); // Fantastique

        books[2].Authors.Add(authors[2]); // Orwell -> 1984
        books[2].Categories.Add(categories[1]); // SF

        books[3].Authors.Add(authors[3]); // Christie -> Orient Express
        books[3].Categories.Add(categories[2]); // Policier

        books[4].Authors.Add(authors[4]); // King -> Ça
        books[4].Categories.Add(categories[3]); // Fantastique

        context.Books.AddRange(books);
        context.SaveChanges();

        // 4. Users
        var users = new User[]
        {
            new User { FirstName = "Jean", LastName = "Dupont", Email = "jean.dupont@email.com", Phone = "514-555-0101", Address = "123 Rue Principale", City = "Montréal", PostalCode = "H1A 1A1", RegistrationDate = DateTime.Now.AddMonths(-6), Status = UserStatus.Active, UserType = UserType.Student },
            new User { FirstName = "Marie", LastName = "Curie", Email = "marie.curie@email.com", Phone = "514-555-0102", Address = "456 Avenue des Sciences", City = "Québec", PostalCode = "G1A 1A1", RegistrationDate = DateTime.Now.AddMonths(-12), Status = UserStatus.Active, UserType = UserType.Employee },
            new User { FirstName = "Paul", LastName = "Martin", Email = "paul.martin@email.com", Phone = "514-555-0103", Address = "789 Boulevard", City = "Laval", PostalCode = "H7A 1A1", RegistrationDate = DateTime.Now.AddMonths(-1), Status = UserStatus.Inactive, UserType = UserType.External }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        // 5. Employees
        var employees = new Employee[]
        {
            new Employee { FirstName = "Sophie", LastName = "Bibliothécaire", Email = "sophie@library.com", HireDate = DateTime.Now.AddYears(-5), Role = EmployeeRole.Librarian },
            new Employee { FirstName = "Marc", LastName = "Admin", Email = "marc@library.com", HireDate = DateTime.Now.AddYears(-2), Role = EmployeeRole.Administrator }
        };
        context.Employees.AddRange(employees);
        context.SaveChanges();

        // 6. Activities
        var activities = new Activity[]
        {
            new Activity { Name = "Club de lecture", Description = "Discussion mensuelle", Type = ActivityType.Event, ActivityDate = DateTime.Now.AddDays(7), MaxCapacity = 20, Organizer = employees[0] },
            new Activity { Name = "Concours d'écriture", Description = "Thème: L'hiver", Type = ActivityType.Contest, ActivityDate = DateTime.Now.AddDays(30), MaxCapacity = 50, Organizer = employees[0] }
        };
        context.Activities.AddRange(activities);
        context.SaveChanges();

        // 7. Equipment
        var equipment = new Equipment[]
        {
            new Equipment { Name = "Laptop Dell", Type = EquipmentType.Laptop, SerialNumber = "SN123456", Status = EquipmentStatus.Available, PurchaseDate = DateTime.Now.AddYears(-1) },
            new Equipment { Name = "Projecteur Epson", Type = EquipmentType.Projector, SerialNumber = "SN789012", Status = EquipmentStatus.Available, PurchaseDate = DateTime.Now.AddYears(-2) }
        };
        context.Equipment.AddRange(equipment);
        context.SaveChanges();

        // 8. Loans
        var loans = new Loan[]
        {
            new Loan { UserId = users[0].UserId, BookId = books[0].BookId, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(4), Status = LoanStatus.InProgress },
            new Loan { UserId = users[1].UserId, BookId = books[1].BookId, LoanDate = DateTime.Now.AddDays(-20), DueDate = DateTime.Now.AddDays(-6), ReturnDate = null, Status = LoanStatus.Late, LateFee = 3.00m } // Late
        };
        context.Loans.AddRange(loans);
        context.SaveChanges();
    }
}
