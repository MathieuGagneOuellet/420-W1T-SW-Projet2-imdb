using imdbexperience.DAL;
using imdbexperience.DAL.DAO;
using imdbexperience.Utils;

var builder = WebApplication.CreateBuilder(args);

//J'ajoute mes propres services en haut des defaults
builder.Services.AddControllers(); //permet d'utiliser [ApiController]
builder.Services.AddSingleton<AppDbContext>(_ => new AppDbContext());
builder.Services.AddSingleton<MediaItemDAO>();
builder.Services.AddSingleton<UserDAO>();

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

await StartSeed.CreateAdminUser(app.Services); //s'assure que admin existe comme user (password : admin)

app.Run();
