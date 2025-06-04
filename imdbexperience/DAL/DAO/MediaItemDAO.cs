using imdbexperience.DAL.Entities;
using MongoDB.Driver;

namespace imdbexperience.DAL.DAO
{
    public class MediaItemDAO
    {
        //opérations d'accès a la db de base. GetAll, GetById, Insert, Delete, Update
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
    }
}
