using System.Data;
using Api.Controller;
using Api.Driver;
using Api.Gateway;
using Api.Usecase;
using Api.Usecase.Port;
using Microsoft.Data.Sqlite;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// DIコンテナ
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDbConnection>(_ => new SqliteConnection(connectionString));
builder.Services.AddScoped<SQLiteDriver>();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";
var dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "postgres";
var dbPooling = Environment.GetEnvironmentVariable("DB_POOLING") ?? "true";
var dbMinPoolSize = Environment.GetEnvironmentVariable("DB_MIN_POOL_SIZE") ?? "0";
var dbMaxPoolSize = Environment.GetEnvironmentVariable("DB_MAX_POOL_SIZE") ?? "10";
var maxTimeout = Environment.GetEnvironmentVariable("DB_MAX_TIMEOUT") ?? "5";

var connectionStringBuilder = new NpgsqlConnectionStringBuilder()
{
    Host = dbHost,
    Port = int.Parse(dbPort),
    Pooling = bool.Parse(dbPooling),
    MinPoolSize = int.Parse(dbMinPoolSize),
    MaxPoolSize = int.Parse(dbMaxPoolSize),

    Timeout = int.Parse(maxTimeout),


    Database = dbDatabase,
    Username = dbUser,
    Password = dbPassword,
};
builder.Services.AddScoped(_ => new PostgresDriver(connectionStringBuilder.ConnectionString));

builder.Services.AddScoped<ITodoItemPort, TodoItemGateway>();
builder.Services.AddScoped<FetchTodoItemsUsecase>();
builder.Services.AddScoped<TodoController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast/{number}", (int number) =>
{
    Console.WriteLine(number);
    var forecast = Enumerable.Range(1, number).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/todoitems/", (TodoController controller) => controller.Get());

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public static string TestName => "Test";
}