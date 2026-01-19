# Documentation Technique

## Architecture du Système

Le système est construit selon une architecture en couches (N-Tier) favorisant la séparation des responsabilités et la maintenabilité.

### 1. LibraryManagement.Core (Domain Layer)
Contient les éléments centraux du domaine qui ne dépendent d'aucune technologie d'accès aux données.
- **Entités**: Classes POCO représentant les tables (Book, User, Loan, etc.)
- **Enums**: Types énumérés pour les statuts (UserStatus, LoanStatus, etc.)
- **Interfaces**: Contrats pour les repositories (IRepository<T>, IUnitOfWork)

### 2. LibraryManagement.Infrastructure (Data Access Layer)
Implémente l'accès aux données avec Entity Framework Core.
- **LibraryDbContext**: Contexte de base de données.
- **Configurations**: Classes `IEntityTypeConfiguration` pour la Fluent API.
- **Repositories**: Implémentation générique et spécifique des repositories.
- **UnitOfWork**: Gestion des transactions et coordination des repositories.

### 3. LibraryManagement.Application (Business Logic Layer)
Contient la logique métier et les règles de validation.
- **Services**: Classes orchestrant les opérations (BookService, LoanService, etc.).
- **Validation**: Vérification des règles métier (unicité ISBN, limites d'emprunts, etc.).

### 4. LibraryManagement.WinForms (Presentation Layer)
Interface utilisateur Windows Forms.
- **Forms**: Fenêtres de l'application.
- **Program.cs**: Point d'entrée et configuration de l'injection de dépendances (DI).

## Modèle de Données

Le schéma de base de données comprend les entités principales suivantes :

- **Book**: Livres avec relation N:N vers Authors et Categories.
- **User**: Usagers avec relation 1:N vers Loans.
- **Loan**: Emprunts liant User et Book.
- **Activity**: Événements organisés par des employés.
- **Equipment**: Matériel disponible au prêt.

### Règles de Suppression (Cascade Delete)
- **User**: RESTRICT (Impossible de supprimer si historique d'emprunts existe).
- **Book**: RESTRICT (Impossible de supprimer si emprunts actifs).
- **Activity**: CASCADE (Suppression d'une activité supprime les participations).

## Design Patterns Utilisés

- **Repository Pattern**: Abstraction de l'accès aux données.
- **Unit of Work**: Gestion atomique des transactions.
- **Dependency Injection**: Couplage faible entre les couches.
- **Factory (Simplifié)**: Création des formulaires via DI.

## Technologies

- C# 10 / .NET 6+
- Entity Framework Core 8
- SQL Server LocalDB
- Windows Forms
