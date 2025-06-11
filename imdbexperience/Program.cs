using imdbexperience.DAL;
using imdbexperience.DAL.DAO;
using imdbexperience.Utils;

var builder = WebApplication.CreateBuilder(args);

//J'ajoute mes propres services en haut des defaults
builder.Services.AddControllers(); //permet d'utiliser [ApiController]
builder.Services.AddSingleton<AppDbContext>(_ => new AppDbContext());
builder.Services.AddSingleton<MediaItemDAO>();
builder.Services.AddSingleton<UserDAO>();
builder.Services.AddSingleton<MediaStatusDAO>();
builder.Services.AddSingleton<GenreDAO>();
builder.Services.AddSingleton<StatsDAO>();



// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); //save mes controlleurs automatiquement

using (var scope = app.Services.CreateScope())
{
    await StartSeed.Init(scope.ServiceProvider);
}
    // await StartSeed.CreateAdminUser(app.Services); //s'assure que admin existe comme user (password : admin)
    // await StartSeed.CreateGenres(app.Services.GetRequiredService<AppDbContext>()); //s'assure que ma DB contient les Genres

    app.Run();
