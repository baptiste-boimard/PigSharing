using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;
using PigSharing.Server.Database;
using PigSharing.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// // Ajout de BlazoredLocalStorage
// builder.Services.AddBlazoredLocalStorage();

// Ajout des repositories
builder.Services.AddScoped<AuthRepository>();

// Service de la Base de données
builder.Services.AddDbContext<PostgresDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration des CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsPolicyBuilder => corsPolicyBuilder
            .WithOrigins("http://localhost:5167")
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
