using imdbexperience.DAL;
using imdbexperience.DAL.DAO;
using imdbexperience.DAL.Entities;
using MongoDB.Driver;

namespace imdbexperience.Utils
{
    public static class StartSeed
    {
        public static async Task Init(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await CreateAdminUser(scope.ServiceProvider);
            await CreateMediaItems(context);
            await CreateGenres(context);
        }
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
        public static async Task CreateMediaItems(AppDbContext context)
        {
            var collection = context.MediaItems;
            var count = await collection.CountDocumentsAsync(_ => true);
            if (count > 0)
            {
                return;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "media_items_1200.json");
            var json = await File.ReadAllTextAsync(filePath);
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var items = System.Text.Json.JsonSerializer.Deserialize<List<MediaItem>>(json, options);

            await collection.InsertManyAsync(items);
            Console.WriteLine("MediaItems ajoutés depuis JSON");
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
