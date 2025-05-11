# projet-gsb2

# 🏥 - Application de gestion de praticiens (Projet GSB)

> Réalisé dans le cadre du BTS SIO SLAM  
> Technologies principales : C#, ASP.NET Core MVC, Entity Framework, MySQL

## 📌 Description

MedManager est une application web permettant la gestion des praticiens (ajout, modification, suppression, visualisation).  
Elle a été développée dans le cadre d’un projet pédagogique pour le laboratoire fictif **Galaxy Swiss Bourdin (GSB)**.

L'application facilite le suivi des informations des professionnels de santé pour une meilleure organisation interne.

## 🚀 Fonctionnalités

- 🔐 Authentification sécurisée
- 👨‍⚕️ Création, consultation, modification et suppression de fiches praticien ( CRUD ) 
- 🔎 Recherche de praticiens
- 🗃️ Base de données MySQL connectée via Entity Framework
- 📄 Documentation technique claire et structurée

## 🛠️ Technologies utilisées

- **Langage** : C# (POO)
- **Framework** : ASP.NET Core MVC (.NET 8)
- **Base de données** : MySQL (via WAMP)
- **ORM** : Entity Framework Core
- **Interface BDD** : phpMyAdmin
- **IDE** : Visual Studio 2022

## ⚙️ Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [WAMP Server](https://www.wampserver.com/)
- Visual Studio 2022
- phpMyAdmin
- Navigateur web (Chrome, Firefox...)

## 📦 Installation

1/ git clone 

2/ Créer la base de données MySQL et configurer la chaîne de connexion dans appsettings.json.

3/ Appliquer les migrations :
dotnet ef database update


4/ Lancer l’application :
dotnet run


5/ Ouvrir dans un navigateur et lancer en local

Projet-GSB/
├── Controllers/
├── Models/
├── Views/
├── Data/
├── wwwroot/
├── appsettings.json
└── Program.cs

Guide utilisateur – Comment utiliser l'application
1. Accéder à l'application
Ouvrir un navigateur

Saisir l’adresse du serveur, par exemple :


2. Écran de connexion
Entrez vos identifiants (admin ou médecin)


Redirection vers le tableau de bord

4. Gestion des praticiens
Accéder au menu « Praticiens »

Possibilités :

* Ajouter un praticien

* Modifier un praticien existant

* Supprimer un praticien

* Rechercher un praticien par nom, spécialité ou ville



4. Déconnexion
Cliquez sur l’icône de profil (en haut à droite)

Sélectionnez Déconnexion

