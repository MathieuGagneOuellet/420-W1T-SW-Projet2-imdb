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
        public async Task<List<MediaItem>> GetByCriteriaAsync(string type, int? annee = null, string? genre = null)
        {
            var filterBuilder = Builders<MediaItem>.Filter;
            var filter = filterBuilder.Regex(item => item.Type, new BsonRegularExpression($"^{Regex.Escape(type)}$", "i"));

            if (annee.HasValue)
                filter &= filterBuilder.Eq(item => item.Annee, annee.Value);

            if (!string.IsNullOrWhiteSpace(genre))
                filter &= filterBuilder.AnyEq(item => item.Genres, genre);

            return await _collection.Find(filter).ToListAsync();    
        }

    }
}
