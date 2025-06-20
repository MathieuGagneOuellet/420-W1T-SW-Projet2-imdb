using imdbexperience.DAL.Entities;
using MongoDB.Driver;

namespace imdbexperience.DAL.DAO
{
    public class GenreDAO
    {
        private readonly IMongoCollection<Genre> _collection;

        public GenreDAO(AppDbContext context)
        {
            _collection = context.Genres;
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(string id)
        {
            return await _collection.Find(g => g.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Genre?> GetByNomAsync(string nom)
        {
            return await _collection.Find(g => g.Nom.ToLower() == nom.ToLower()).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Genre genre)
        {
            await _collection.InsertOneAsync(genre);
        }

        public async Task<bool> UpdateAsync(Genre genre)
        {
            var result = await _collection.ReplaceOneAsync(g => g.Id == genre.Id, genre);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(g => g.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<List<Genre>> GetByIdsAsync(List<string> ids)
        {
            return await _collection.Find(g => ids.Contains(g.Id)).ToListAsync();
        }

        public async Task<List<Genre>> GetByNamesAsync(List<string> names)
        {
            var filter = Builders<Genre>.Filter.In("nom", names);
            return await _collection.Find(filter).ToListAsync();
        }
        public async Task CreateManyAsync(List<Genre> genres)
        {
            if (genres.Count > 0)
                await _collection.InsertManyAsync(genres);
        }
        public async Task<List<Genre>> GetOrCreateGenresAsync(List<string> names, IClientSessionHandle session)
        {
            var existing = await _collection.Find(session, g => names.Contains(g.Nom)).ToListAsync();
            var existingNames = existing.Select(g => g.Nom).ToHashSet();
            var toCreate = names.Where(n => !existingNames.Contains(n)).ToList();

            var genresToInsert = toCreate.Select(n => new Genre { Nom = n }).ToList();

            if (genresToInsert.Count > 0)
                await _collection.InsertManyAsync(session, genresToInsert);

            return existing.Concat(genresToInsert).ToList();
        }


    }
}
