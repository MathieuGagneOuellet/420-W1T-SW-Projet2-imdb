using imdbexperience.DAL.DAO;
using imdbexperience.DAL.Entities;

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
    }
}
