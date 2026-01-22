# Library Management System

Application Windows Forms de gestion de bibliothèque (C# / .NET 8 / EF Core / SQL Server LocalDB).

## Fonctionnalités

- **Livres**: CRUD, recherche, gestion des copies disponibles
- **Usagers**: CRUD, gestion du statut (actif/inactif)
- **Emprunts (livres)**: CRUD, dates, états (en cours / en retard / retourné), pénalités
- **Activités (concours/événements)**: CRUD (entité `Activity` avec un `Type`)
- **Matériel**: CRUD, emprunt, retour, historique des prêts
- **Rapports**: emprunts par usager

## Règles de suppression (important)

La suppression est bloquée si l’élément est lié à de l’historique.

- **Livre**: impossible si emprunts ou évaluations
- **Usager**: impossible si emprunts, participations, évaluations, prêts d’équipement
- **Activité**: impossible si participations
- **Matériel**: impossible si historique de prêts

## Installation / exécution

- Pré-requis: **.NET 8 SDK**, **SQL Server LocalDB**
- Ouvrir `LibraryManagementSystem.sln` dans Visual Studio
- Démarrer `LibraryManagement.WinForms`

Documentation:
- `INSTRUCTIONS_INSTALLATION.md`
- `INSTRUCTIONS_UTILISATION.md`
- `USER_GUIDE.md`
- `README_TECHNICAL.md`
- `sql/LibraryManagementDB.sql`

