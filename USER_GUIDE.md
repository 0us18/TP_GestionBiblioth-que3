# Guide Utilisateur

## Navigation
Le tableau de bord principal offre un accès rapide à tous les modules via des boutons dédiés.

## Gestion des Livres
1. **Lister les livres**: Cliquez sur "Gestion Livres". La liste affiche tous les livres disponibles.
2. **Rechercher**: Utilisez la barre de recherche en haut pour filtrer par titre, auteur ou ISBN.
3. **Ajouter un livre**: Cliquez sur "Nouveau Livre", remplissez le formulaire et sauvegardez.
4. **Modifier**: Sélectionnez un livre puis cliquez sur "Modifier" (ou double-cliquez sur la ligne).
5. **Supprimer**: Sélectionnez un livre puis cliquez sur "Supprimer".

## Gestion des Emprunts
1. **Créer un emprunt**:
   - Allez dans "Gestion Emprunts".
   - Cliquez sur "Nouvel Emprunt".
   - Sélectionnez l'usager et le livre dans les listes déroulantes.
   - Validez. Le système vérifiera automatiquement la disponibilité et les droits de l'usager.

2. **Modifier un emprunt**:
   - Sélectionnez un emprunt puis cliquez sur "Modifier".
   - Vous pouvez ajuster la date d’échéance et le statut.

3. **Supprimer un emprunt**:
   - Sélectionnez un emprunt puis cliquez sur "Supprimer".

## Gestion des Usagers
1. **Ajouter un usager**: cliquez sur "Nouvel Usager".
2. **Modifier**: sélectionnez un usager puis "Modifier" (ou double-clic).
3. **Supprimer**: sélectionnez un usager puis "Supprimer".

Les usagers inactifs ne peuvent pas effectuer de nouveaux emprunts.

## Gestion des Activités
1. **Ajouter**: cliquez sur "Nouvelle Activité".
2. **Modifier**: sélectionnez une activité puis "Modifier" (ou double-clic).
3. **Supprimer**: sélectionnez une activité puis "Supprimer".

## Suppression impossible (message)
Si un élément est lié à de l’historique, la suppression est refusée et un message explique pourquoi.

## Dépannage
- **Erreur de connexion BD**: Vérifiez que SQL Server LocalDB est bien installé et en cours d'exécution.
- **Livre non disponible**: Vérifiez le nombre de copies disponibles dans la fiche du livre.
