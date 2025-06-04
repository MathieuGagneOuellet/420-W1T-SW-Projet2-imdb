# 420-W1T-SW-Projet2-imdb
Code étudiant : 6349821

### step 1 prep work début de projet
- 1.1 : récupération des données externes sur imdb, puis convertir en tableau excel
- 1.2 : convertir le excel en json
- 1.3 : faire le github
- 1.4 : structure de base pour un rest API minimale, juste à faire dotnet new webapi -n leNomQueJeVeux, cleanup de weatherforecast

## step 2 créer le modèle MediaItem complet avec MongoDB.Driver
- 2.1 : Faire une arborescence de fichier qui respecte la façon de faire n-tier. J'ai un dossier DAL qui contient DAO et Entities. MediaItemDAO est séparé de ce qu'il constitue en tant qu'entité, et j'ai ajouté mon DBContext tel que vu en classe.
- 2.2 : Écriture de /DAL/DbContext.cs, /DAL/DAO/MediaItemDAO.cs et /DAL/Entities/MediaItem.cs
- 2.3 : Test avec Postman requis à cet instant, donc j'écris un controlleur de base avec GetAll et GetById
- 2.4 : Création de la DB à partir de media_items_1200.json. Je l'ai fait directement a partir de l'extension VS Code -> New Playground -> Run All -> "Inserted 1200 documents into mediaItems"
- 2.5 : Tests dans Postman, si mon GetAll et GetById marchent, je sais que je suis en bizness pour la suite de mes interactions BD... Mon GetById fonctionne parfaitement mais pas mon GetAll. "Cannot deserialize a 'String' from BsonType 'Int32'". Ma propriété c# attend un string mais reçoit un int. J'ai mon tt0192947 que le titre c'est 12 (au lieu de "12") et tt0212712 qui s'appelle 2046 (plutôt que "2046"). Je les change manuellement et on reteste. Bingo. GetAll et GetById fonctionnent.

## step 3 création d'une route de recherche qui peut prendre multiple critères (nom et/ou annee et/ou genre)
- 3.1: la route de recherche par type uniquement /api/mediaitem/search?type=movie ou type=tvSeries
- 3.2: ajouter un critère optionnel à cette route -> année (peut être null)
- 3.3: ajouter un critère optionnel à cette route -> genre (peut être null)

## step 4 reste du crud pour un mediaitem
- 4.1: Créer, POST /api/mediaitem pour créer un mediaitem
- 4.2: Update, PUT /api/mediaitem/sonID pour modifier
- 4.3: Delete