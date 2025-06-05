using System.Text.RegularExpressions;
using imdbexperience.DAL.Entities;
using MongoDB.Driver;

namespace imdbexperience.DAL.DAO
{
    public class MediaStatusDAO
    {
        private readonly IMongoCollection<MediaStatus> _collection;

        public MediaStatusDAO(AppDbContext context)
        {
            _collection = context.MediaStatuses;
        }

        public async Task<List<MediaStatus>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<List<MediaStatus>> GetByUserIdAsync(string userId)
        {
            return await _collection.Find(ms => ms.UserId == userId).ToListAsync();
        }

        public async Task<MediaStatus?> GetByUserAndMediaAsync(string userId, string mediaId)
        {
            return await _collection.Find(ms => ms.UserId == userId && ms.MediaId == mediaId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(MediaStatus status)
        {
            await _collection.InsertOneAsync(status);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(ms => ms.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> UpdateAsync(MediaStatus status)
        {
            var result = await _collection.ReplaceOneAsync(item => item.Id == status.Id, status);
            return result.ModifiedCount > 0;
        }
                public async Task<List<MediaStatus>> GetByUserAndStatusAsync(string userId, string status)
                {
                    var filter = Builders<MediaStatus>.Filter.And(
                        Builders<MediaStatus>.Filter.Eq(ms => ms.UserId, userId),
                        Builders<MediaStatus>.Filter.Regex(ms => ms.Status, new MongoDB.Bson.BsonRegularExpression($"^{Regex.Escape(status)}$", "i"))
                    );

                    return await _collection.Find(filter).ToListAsync();
                }

    }
}