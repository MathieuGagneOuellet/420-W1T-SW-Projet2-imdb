using imdbexperience.DAL.Entities;
using MongoDB.Driver;

namespace imdbexperience.DAL.DAO
{
    public class UserDAO
    {
        private readonly IMongoCollection<User> _collection;

        public UserDAO(AppDbContext context)
        {
            _collection = context.Users;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }
        
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _collection.Find(u => u.Username == username).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(User user)
        {
            await _collection.InsertOneAsync(user);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var result = await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);
            return result.ModifiedCount > 0;
        }         


    }
}