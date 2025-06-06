using System.Text.RegularExpressions;
using imdbexperience.DAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace imdbexperience.DAL.DAO
{
    public class MediaItemDAO
    {
        //crud pour MediaItems. GetAll, GetById, Insert, Delete, Update
        private readonly IMongoCollection<MediaItem> _collection;

        public MediaItemDAO(AppDbContext context)
        {
            _collection = context.MediaItems;
        }

        public async Task<List<MediaItem>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<MediaItem?> GetByIdAsync(string id)
        {
            return await _collection.Find(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(MediaItem item)
        {
            await _collection.InsertOneAsync(item);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(item => item.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> UpdateAsync(MediaItem item)
        {
            var result = await _collection.ReplaceOneAsync(i => i.Id == item.Id, item);
            return result.ModifiedCount > 0;
        }

        //méthode pour rechercher plusieurs MediaItems selon des critères (type, annee, genre, keyword)
        public async Task<List<MediaItem>> GetByCriteriaAsync(string type, int? annee = null, string? genre = null, string? keyword = null)
        {
            var filterBuilder = Builders<MediaItem>.Filter;
            var filters = new List<FilterDefinition<MediaItem>>//liste des filtres à appliquer
            {
                filterBuilder.Regex(m => m.Type, new BsonRegularExpression($"^{Regex.Escape(type)}$", "i")) //filtre sur le type, case insensitive
            };
            //filtres additionnels si précisés
            if (annee.HasValue)
                filters.Add(filterBuilder.Eq(m => m.Annee, annee.Value));

            if (!string.IsNullOrWhiteSpace(genre))
                filters.Add(filterBuilder.AnyEq(m => m.Genres, genre));

            if (!string.IsNullOrWhiteSpace(keyword))
                filters.Add(filterBuilder.Regex(m => m.Titre, new BsonRegularExpression(keyword, "i")));

            //fusion des filtres (si applicable)
            var filter = filters.Count > 1 ? filterBuilder.And(filters) : filters.First();
            return await _collection.Find(filter).ToListAsync();
        }

        //Méthode complémentaire a GetByIdAsync (Ids plutôt que Id), 
        //elle permet de chercher plusieurs MediaItem à la fois, 
        //par exemple chercher tous les MediaItem qui sont dans la Watchlist d'un user
        public async Task<List<MediaItem>> GetByIdsAsync(List<string> ids)
        {
            var filter = Builders<MediaItem>.Filter.In(item => item.Id, ids);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
