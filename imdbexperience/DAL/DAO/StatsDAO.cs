using MongoDB.Driver;
using imdbexperience.DAL.Entities;

namespace imdbexperience.DAL.DAO
{
    public class StatsDAO
    {
        private readonly IMongoCollection<MediaStatus> _collection;

        public StatsDAO(AppDbContext context)
        {
            _collection = context.MediaStatuses;
        }

        public async Task<Dictionary<string, int>> CountByStatusAsync()
        {
            var results = await _collection.Aggregate()
                .Group(
                    ms => ms.Status,
                    g => new { Status = g.Key, Count = g.Count() }
                )
                .ToListAsync();
            return results.ToDictionary(r => r.Status, r => r.Count);
        }
        public async Task<long> GetUserCountAsync()
        {
            var userCollection = _collection.Database.GetCollection<User>("users");
            return await userCollection.CountDocumentsAsync(_ => true);
        }

        public async Task<long> GetMediaItemCountAsync()
        {
            var mediaCollection = _collection.Database.GetCollection<MediaItem>("mediaItems");
            return await mediaCollection.CountDocumentsAsync(_ => true);
        }

        public async Task<long> GetMediaStatusCountAsync()
        {
            return await _collection.CountDocumentsAsync(_ => true);
        }
        
        public async Task<Dictionary<string, int>> GetFavGenresForUserAsync(string userId, MediaItemDAO mediaItemDao)
        {
            //On récupère tous les statuts "Seen" pour l'utilisateur
            var seenStatuses = await _collection.Find(ms => ms.UserId == userId && ms.Status.ToLower() == "seen").ToListAsync();
            var mediaIds = seenStatuses.Select(ms => ms.MediaId).Distinct().ToList();

            //On récupère les mediaitem liés à ces statuts
            var medias = await mediaItemDao.GetByIdsAsync(mediaIds);
            var genreCounts = medias
                .SelectMany(m => m.Genres)
                .GroupBy(g => g)
                .ToDictionary(g => g.Key, g => g.Count());

            return genreCounts;
        }

    }
}
