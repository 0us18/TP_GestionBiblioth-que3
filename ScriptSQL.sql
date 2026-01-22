IF DB_ID(N'LibraryManagementDB_Migrated') IS NULL
BEGIN
    CREATE DATABASE [LibraryManagementDB_Migrated];
END
GO

USE [LibraryManagementDB_Migrated];
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [Authors] (
        [AuthorId] int NOT NULL IDENTITY,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Biography] nvarchar(max) NOT NULL,
        [BirthDate] datetime2 NULL,
        CONSTRAINT [PK_Authors] PRIMARY KEY ([AuthorId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [Books] (
        [BookId] int NOT NULL IDENTITY,
        [Title] nvarchar(200) NOT NULL,
        [ISBN] nvarchar(20) NOT NULL,
        [PublicationYear] int NOT NULL,
        [AvailableCopies] int NOT NULL,
        CONSTRAINT [PK_Books] PRIMARY KEY ([BookId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [Categories] (
        [CategoryId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [Employees] (
        [EmployeeId] int NOT NULL IDENTITY,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [HireDate] datetime2 NOT NULL,
        [Role] int NOT NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([EmployeeId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [Equipment] (
        [EquipmentId] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Type] int NOT NULL,
        [SerialNumber] nvarchar(100) NOT NULL,
        [Status] int NOT NULL,
        [PurchaseDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Equipment] PRIMARY KEY ([EquipmentId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [UserId] int NOT NULL IDENTITY,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [Phone] nvarchar(20) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [City] nvarchar(max) NOT NULL,
        [PostalCode] nvarchar(max) NOT NULL,
        [RegistrationDate] datetime2 NOT NULL,
        [Status] int NOT NULL,
        [UserType] int NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [BookAuthors] (
        [AuthorsAuthorId] int NOT NULL,
        [BooksBookId] int NOT NULL,
        CONSTRAINT [PK_BookAuthors] PRIMARY KEY ([AuthorsAuthorId], [BooksBookId]),
        CONSTRAINT [FK_BookAuthors_Authors_AuthorsAuthorId] FOREIGN KEY ([AuthorsAuthorId]) REFERENCES [Authors] ([AuthorId]) ON DELETE CASCADE,
        CONSTRAINT [FK_BookAuthors_Books_BooksBookId] FOREIGN KEY ([BooksBookId]) REFERENCES [Books] ([BookId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [BookCategories] (
        [BooksBookId] int NOT NULL,
        [CategoriesCategoryId] int NOT NULL,
        CONSTRAINT [PK_BookCategories] PRIMARY KEY ([BooksBookId], [CategoriesCategoryId]),
        CONSTRAINT [FK_BookCategories_Books_BooksBookId] FOREIGN KEY ([BooksBookId]) REFERENCES [Books] ([BookId]) ON DELETE CASCADE,
        CONSTRAINT [FK_BookCategories_Categories_CategoriesCategoryId] FOREIGN KEY ([CategoriesCategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [Activities] (
        [ActivityId] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Type] int NOT NULL,
        [ActivityDate] datetime2 NOT NULL,
        [MaxCapacity] int NOT NULL,
        [OrganizerEmployeeId] int NULL,
        CONSTRAINT [PK_Activities] PRIMARY KEY ([ActivityId]),
        CONSTRAINT [FK_Activities_Employees_OrganizerEmployeeId] FOREIGN KEY ([OrganizerEmployeeId]) REFERENCES [Employees] ([EmployeeId]) ON DELETE SET NULL
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [BookReviews] (
        [ReviewId] int NOT NULL IDENTITY,
        [BookId] int NOT NULL,
        [UserId] int NOT NULL,
        [Rating] int NOT NULL,
        [Comment] nvarchar(1000) NOT NULL,
        [ReviewDate] datetime2 NOT NULL,
        CONSTRAINT [PK_BookReviews] PRIMARY KEY ([ReviewId]),
        CONSTRAINT [FK_BookReviews_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([BookId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_BookReviews_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [EquipmentLoans] (
        [EquipmentLoanId] int NOT NULL IDENTITY,
        [EquipmentId] int NOT NULL,
        [UserId] int NOT NULL,
        [LoanDate] datetime2 NOT NULL,
        [DueDate] datetime2 NOT NULL,
        [ReturnDate] datetime2 NULL,
        [Status] int NOT NULL,
        CONSTRAINT [PK_EquipmentLoans] PRIMARY KEY ([EquipmentLoanId]),
        CONSTRAINT [FK_EquipmentLoans_Equipment_EquipmentId] FOREIGN KEY ([EquipmentId]) REFERENCES [Equipment] ([EquipmentId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_EquipmentLoans_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [Loans] (
        [LoanId] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [BookId] int NOT NULL,
        [LoanDate] datetime2 NOT NULL,
        [DueDate] datetime2 NOT NULL,
        [ReturnDate] datetime2 NULL,
        [Status] int NOT NULL,
        [LateFee] decimal(18,2) NULL,
        CONSTRAINT [PK_Loans] PRIMARY KEY ([LoanId]),
        CONSTRAINT [FK_Loans_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([BookId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Loans_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE TABLE [Participations] (
        [ParticipationId] int NOT NULL IDENTITY,
        [ActivityId] int NOT NULL,
        [UserId] int NOT NULL,
        [RegistrationDate] datetime2 NOT NULL,
        [AttendanceStatus] int NOT NULL,
        CONSTRAINT [PK_Participations] PRIMARY KEY ([ParticipationId]),
        CONSTRAINT [FK_Participations_Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [Activities] ([ActivityId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Participations_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Activities_OrganizerEmployeeId] ON [Activities] ([OrganizerEmployeeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_BookAuthors_BooksBookId] ON [BookAuthors] ([BooksBookId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_BookCategories_CategoriesCategoryId] ON [BookCategories] ([CategoriesCategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_BookReviews_BookId] ON [BookReviews] ([BookId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_BookReviews_UserId] ON [BookReviews] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Books_ISBN] ON [Books] ([ISBN]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Equipment_SerialNumber] ON [Equipment] ([SerialNumber]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_EquipmentLoans_EquipmentId] ON [EquipmentLoans] ([EquipmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_EquipmentLoans_UserId] ON [EquipmentLoans] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Loans_BookId] ON [Loans] ([BookId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Loans_UserId] ON [Loans] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Participations_ActivityId] ON [Participations] ([ActivityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Participations_UserId] ON [Participations] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122045315_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260122045315_InitialCreate', N'8.0.0');
END;
GO

COMMIT;
GO

