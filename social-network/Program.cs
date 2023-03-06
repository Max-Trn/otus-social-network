using System.Data;
using Npgsql;
using social_network.DAL.Repositories;
using social_network.Services;
using social_network.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<UserRepository>();

//TODO: сделать чтение из конфигов
builder.Services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection("Server=postgres_image; Port=5432; User Id=user; Password=password; Database=socialnetwork"));

builder.WebHost.UseKestrel();
builder.WebHost.UseUrls("http://*:7285");

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

