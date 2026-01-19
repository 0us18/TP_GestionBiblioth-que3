# Guide Utilisateur

## Navigation
Le tableau de bord principal offre un accès rapide à tous les modules via des boutons dédiés.

## Gestion des Livres
1. **Lister les livres**: Cliquez sur "Gestion Livres". La liste affiche tous les livres disponibles.
2. **Rechercher**: Utilisez la barre de recherche en haut pour filtrer par titre, auteur ou ISBN.
3. **Ajouter un livre**: Cliquez sur "Nouveau Livre", remplissez le formulaire et sauvegardez.
4. **Modifier**: Double-cliquez sur un livre dans la liste pour modifier ses détails.

## Gestion des Emprunts
1. **Créer un emprunt**:
   - Allez dans "Gestion Emprunts".
   - Cliquez sur "Nouvel Emprunt".
   - Sélectionnez l'usager et le livre dans les listes déroulantes.
   - Validez. Le système vérifiera automatiquement la disponibilité et les droits de l'usager.

2. **Retourner un livre**:
   - (Fonctionnalité à venir dans l'interface détaillée)
   - Le système calculera automatiquement les frais de retard si la date d'échéance est dépassée (0.50$ / jour).

## Gestion des Usagers
- Consultez la liste des membres inscrits via "Gestion Usagers".
- Les usagers inactifs ne peuvent pas effectuer de nouveaux emprunts.

## Dépannage
- **Erreur de connexion BD**: Vérifiez que SQL Server LocalDB est bien installé et en cours d'exécution.
- **Livre non disponible**: Vérifiez le nombre de copies disponibles dans la fiche du livre.
