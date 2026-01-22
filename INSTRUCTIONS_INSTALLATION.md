# Instructions d'installation

## Prérequis

- .NET 8 SDK
- SQL Server LocalDB (souvent installé avec Visual Studio)

## Lancer le projet (Visual Studio)

1. Ouvrir la solution `LibraryManagementSystem.sln`
2. Définir `LibraryManagement.WinForms` comme projet de démarrage
3. Lancer (F5)

Au premier lancement, la base de données est créée et initialisée automatiquement.

## Lancer le projet (ligne de commande)

Depuis le dossier du projet :

```bash
dotnet build .\LibraryManagementSystem\LibraryManagementSystem.sln -c Debug
dotnet run --project .\LibraryManagementSystem\LibraryManagement.WinForms\LibraryManagement.WinForms.csproj -c Debug
```

