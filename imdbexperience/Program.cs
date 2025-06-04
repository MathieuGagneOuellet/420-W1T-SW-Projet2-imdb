using imdbexperience.DAL;
using imdbexperience.DAL.DAO;

var builder = WebApplication.CreateBuilder(args);

//J'ajoute mes propres services en haut des defaults
builder.Services.AddControllers(); //permet d'utiliser [ApiController]
builder.Services.AddSingleton<AppDbContext>(_ => new AppDbContext());
builder.Services.AddSingleton<MediaItemDAO>(); //Mon data access object
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

app.Run();
