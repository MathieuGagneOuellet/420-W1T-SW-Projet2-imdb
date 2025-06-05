using imdbexperience.DAL;
using imdbexperience.DAL.DAO;
using imdbexperience.DAL.Entities;
using MongoDB.Driver;

namespace imdbexperience.Utils
{
    public static class StartSeed
    {
        public static async Task CreateAdminUser(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dao = scope.ServiceProvider.GetRequiredService<UserDAO>();

            var existing = await dao.GetByUsernameAsync("admin");
            if (existing == null)
            {
                var admin = new User
                {
                    Username = "admin",
                    Password = "admin"
                };
                await dao.CreateAsync(admin);
                Console.WriteLine("Utilisateur admin créé automatiquement.");
            }
        }
        public static async Task CreateGenres(AppDbContext context)
        {
            var genresCollection = context.Genres;
            var existingGenres = await genresCollection.Find(_ => true).AnyAsync();
            if (existingGenres) return; // Pas besoin de re-seed

            var mediaItems = await context.MediaItems.Find(_ => true).ToListAsync();
            var genreNames = mediaItems
                .SelectMany(m => m.Genres)
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .Distinct()
                .ToList();

            var genresToInsert = genreNames.Select(name => new Genre
            {
                Nom = name
            });

            await genresCollection.InsertManyAsync(genresToInsert);
            Console.WriteLine("Genres extraits depuis les objets médias et insérés");
        }
    }
}
