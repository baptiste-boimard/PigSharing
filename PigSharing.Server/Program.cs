using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;
using PigSharing.Server.Database;
using PigSharing.Server.Repositories;
using PigSharing.Server.Service;

var builder = WebApplication.CreateBuilder(args);

// Permet de charger mon appsettings.json sous un autre nom
// builder.Configuration.AddJsonFile("server.appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Ajout des repositories
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<PictureRepository>();

// Ajout du service de photo
builder.Services.AddScoped<PictureService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Service de la Base de données
builder.Services.AddDbContext<PostgresDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration des CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsPolicyBuilder => corsPolicyBuilder
            .WithOrigins("http://localhost:5167", "http://51.75.133.155:5167")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Application des CORS policy
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
