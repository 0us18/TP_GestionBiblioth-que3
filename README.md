# Library Management System

Application Windows Forms de gestion de bibliothèque (C# / .NET / EF Core).

## Fonctionnalités

- **Livres**
  - Ajouter / Modifier / Supprimer
  - Recherche
- **Usagers**
  - Ajouter / Modifier / Supprimer
- **Emprunts**
  - Ajouter / Modifier / Supprimer
- **Activités**
  - Ajouter / Modifier / Supprimer

## Règles de suppression (important)

La suppression est bloquée si l’élément est lié à de l’historique.

- **Livre**: impossible si emprunts ou évaluations (reviews)
- **Usager**: impossible si emprunts, participations, évaluations, prêts d’équipement
- **Activité**: impossible si participations

Un message explique la raison quand la suppression est refusée.

## Installation / exécution

- **Pré-requis**: .NET 8 SDK, SQL Server LocalDB
- Ouvrir `LibraryManagementSystem.sln` dans Visual Studio
- Démarrer `LibraryManagement.WinForms`
- Au premier lancement, la base est créée et initialisée automatiquement

## Documentation

- `INSTRUCTIONS_INSTALLATION.md`
- `INSTRUCTIONS_UTILISATION.md`
- `sql/LibraryManagementDB.sql`
- `README_TECHNICAL.md`
