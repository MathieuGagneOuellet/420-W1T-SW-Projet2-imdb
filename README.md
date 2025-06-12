# 420-W1T-SW-Projet2-imdb
Code étudiant : 6349821
Le projet est une API REST complète qui utilise ASP.NET Core et MongoDB. L'objectif est de construire un back-end qui pourrait être utilisé pour un site-web ou une app qui documente des items médias depuis IMDB. Le projet permet de gérer des utilisateurs avec leurs favorites/watchlist/seen, d'afficher à ces users leurs stats pour tel ou tel item média (j'ai vu 56 films comedy, 112 drames, etc.)
Technologies utilisées : ASP.NET Core 8, MongoDB, Postman pour tests
Structure des entités : n-tier, DAL/Entities pour les classes, DAL/DAO pour les data access object, Controllers/ pour les routes API, j'ai inséré le seed de démarrage DB dans /utils/
4 entités, 'User', 'MediaItem', 'MediaStatus', 'Genre', routes CRUD pour les 4
Exemples de requêtes : 
GET => /api/mediaitem/recherche?type=movie&annee=2002&genre=Comedy : Recherche par type + filtres
GET => /api/mediaitem/partialproj : Retourne uniquement titre et année des médias
GET => /api/user/6840f398c0569533b4eaeab3/medias : Tous les MediaItems liés à un user, avec le status (ex: Watchlist)
GET => /api/user/6840f398c0569533b4eaeab3/medias/seen : Filtrer par statut custom pour un user
GET => /api/mediaitem/tt0035423/genres : Liste des genres associés a un MediaItem (N:N relation)
GET => /api/stats/totals : Retourne combien d'users/mediaItems/mediaStatuses sont enregistrés
GET => /api/stats/statuses : Retourne combien d'éléments sont dans les différents status (Watchlist, Seen, Favorite, etc., l'user peut créer des nouveaux status dans cette version)
GET => /api/stats/6840f398c0569533b4eaeab3/genres-frequents : Genres les plus vus par un user donné
GET => /api/stats/6840f398c0569533b4eaeab3/activity-by-year : 
POST => /api/mediastatus : Crée un lien entre un user et un mediaItem avec un statut
PUT => /api/user/6840f398c0569533b4eaeab3 : Modifier un user (ex, mdp mais non sécurisé)
DELETE => /api/mediastatus/6841aafea8333b2baf9c4b8d : Delete un mediaStatus pour un user donné
(ma suite de test postman est disponible dans le fichier IMDB Experience.postman_collection.json, tous les tests m'ont retourné un status 200 sur mon poste avec le port 5198)



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

## step 5 mediastatus (favorite, watchlist, seen), incluant DAO et Controller

## step 6 implémentation de routes qui vont retourner la liste complète des MediaItems associés à un userId, qui va aussi inclure les MediaStatus
- 6.1: Premièrement, une route GET pour lister tous les mediaItems qui sont liés a un user
- 6.2: Ensuite, une route qui va chercher tous les MediaStatus établis par un userId, extraire la liste des mediaItems associés puis faire une requête dans MediaItemDAO pour retourner les objets

## step 7 entité Genre
- 7.1: Création de l'entity
- 7.2: Creation du DAO
- 7.3: Creation du Controller
- 7.4: Le Genre doit être lié aux MediaItem via relation N<->N
- 7.5: Peupler la DB de genres. ChatGPT me génère un script de seed automatique pour le faire
- 7.6: Création de la route GET /api/mediaitem/{id}/genres
- 7.7: Pauffinage du Controller pour que Genre aie un CRUD de base complet. Comme c'est le même principe depuis le début, je laisse GPT me générer ces routes
- 7.8 : GetByCriteriaAsync doit être modifié pour permettre l'utilisation de l'entité Genre, pour que on puisse maintenant rechercher un mediaItem par mot-clé ou genre
- 7.9: Je vais ajouter la projection partielle maintenant vu que c'est vraiment simple. J'ai préféré l'insérer dans l'entité MediaItem question organisation

## step 8 Nouveau DAO/Controller dédié aux stats personalisées d'un user (nombre de films vus par année, moyenne de durees des films vus, genres les plus frequents)
- 8.1: statsDao.cs créé, implémentation de CountByStatusAsync, GetUserCountAsync, GetMediaItemCountAsync, GetMediaStatusCountAsync (AIA). 
- 8.2: Ajout de la méthode/route pour tout combiner les genres des mediaItems que l'utilisateur a vu (status = "seen")
- 8.3: Ajout de "Moyenne de durée des films vus" et "Activité par année", deux routes GET qui vont être facile à incorporer dans StatsDAO/StatsController (AIA)

## step 9 pauffinage, prép pour la remise
- Ajout de la section au début du readme, export de la suite de test Postman, verif finale
- Je vais créer une nouvelle méthode/route qui permet de créer un film et ses genres directement (en une seule route). La méthode va permettre de créer des nouveaux genres et ceux-ci seront ajoutés à la DB directement comme étant des objets Mongo
- La grille de correction indique que ça me prend une "Requête impliquant une transaction". Je vais reprendre la job que je viens de faire et l'intégrer dans une transaction (même principe que pour MySQL). Création de la route api/mediaitem/transaction 
- J'ai aussi ajouté des vérifications simples sur les champs Titre de MediaItem et Nom de Genres
- J'avais complètement oublié Delete User. Je l'ajoute dans UserController
- J'ajoute mes tests post-response à ma collection de tests Postman