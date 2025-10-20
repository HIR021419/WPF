# 📸 PhotoManager — Application WPF de gestion de photos

## 🔎 Objectif du projet

Application WPF permettant de **charger, afficher, trier, filtrer et localiser des photos**. Ce projet met en pratique les notions avancées de WPF, Entity Framework Core et la gestion des métadonnées EXIF/GPS.

## 📊 Fonctionnalités principales

1. **Chargement et affichage des photos**

    * Importation depuis un dossier local.
    * Affichage sous forme de vignettes (ItemsControl + WrapPanel).
    * Double-clic pour afficher en plein écran.

2. **Tri et organisation**

    * Tri par nom, date, taille ou type.
    * Ordre croissant/décroissant.
    * Filtrage par catégorie ou extension.

3. **Recherche et tags**

    * Barre de recherche par mot-clé.
    * Système de tags associés aux images.
    * Ajout/suppression de tags.

4. **Mode diaporama**

    * Défilement automatique avec animations WPF.

5. **Edition basique**

    * Rotation d'image.
    * Suppression ou archivage.

6. **(Optionnel)** Localisation sur carte

    * Affichage des photos sur une carte via Bing Maps ou Leaflet.
    * Lecture des coordonnées GPS depuis les métadonnées.

---

## 🛠️ Technologies et bibliothèques

* **.NET 7+ / WPF**
* **Entity Framework Core (SQLite)**
* **MetadataExtractor** pour les EXIF
* **Microsoft.Toolkit.Mvvm** (pattern MVVM)
* **Bing Maps SDK** ou **Leaflet** (optionnel)

---

## 🔧 Installation et exécution

### 1. Cloner le dépôt

```bash
git clone https://github.com/HIR021419/ImageManager.git
cd ImageManager
```

### 2. Restaurer les dépendances

```bash
dotnet restore
```

### 3. Construire et exécuter

```bash
dotnet build
dotnet run --project ImageManager
```

### 4. Créer la base de données SQLite (si EF utilisé)

```bash
dotnet ef database update
```

---

## 🔖 Exemple d'utilisation

1. Cliquer sur **Importer dossier** et sélectionner un répertoire d'images.
2. Les vignettes s'affichent automatiquement.
3. Double-cliquer sur une image pour l'afficher en plein écran.
4. Utiliser les menus pour trier, filtrer, ou lancer le diaporama.

---

## 🌐 Fonctionnalités facultatives

* **Mode sombre / clair** via styles dynamiques.
* **Carte interactive** avec affichage des points GPS.
* **IA de génération de tags** (ML.NET ou API externe).

---

## 🔒 Bonnes pratiques appliquées

* Architecture **MVVM**.
* Accès données via **Entity Framework Core**.
* Respect de la séparation **UI / logique**.
* Code clair, documenté et modulaire.

---

## 🔄 Auteurs

 - Yann Lemétayer
 - Pierre Maunier
