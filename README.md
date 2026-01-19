# Library Management System

Système de gestion de bibliothèque complet développé en C# avec Entity Framework Core, SQL Server et Windows Forms.

## Fonctionnalités Principales

- **Gestion des Livres**: Ajout, modification, recherche et suppression de livres.
- **Gestion des Usagers**: Inscription, mise à jour et désactivation des membres.
- **Gestion des Emprunts**: Création et retour d'emprunts avec calcul automatique des frais de retard.
- **Activités**: Organisation d'événements et concours.
- **Rapports**: Suivi des emprunts et participations.

## Prérequis

- .NET 8.0 SDK ou supérieur
- SQL Server LocalDB (installé avec Visual Studio)

## Installation et Démarrage

1. Cloner le dépôt :
   ```bash
   git clone https://github.com/votre-repo/LibraryManagementSystem.git
   ```

2. Ouvrir la solution `LibraryManagementSystem.sln` dans Visual Studio.

3. Configurer la base de données :
   L'application utilise Code First. Au premier lancement, la base de données sera automatiquement créée et peuplée avec des données de test (Seed Data).

4. Exécuter l'application :
   Définir le projet `LibraryManagement.WinForms` comme projet de démarrage et lancer (F5).

## Architecture

Le projet suit une architecture en couches stricte :
- **Core**: Entités et Interfaces (Domain Layer)
- **Infrastructure**: Accès aux données (EF Core, Repositories)
- **Application**: Logique métier (Services)
- **WinForms**: Interface utilisateur (Presentation Layer)

Pour plus de détails techniques, voir [README_TECHNICAL.md](README_TECHNICAL.md).
