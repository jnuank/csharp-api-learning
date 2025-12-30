using System.Data;
using Api.Controller;
using Api.Driver;
using Api.Gateway;
using Api.Usecase;
using Api.Usecase.Port;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// DIコンテナ
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDbConnection>(_ => new SqliteConnection(connectionString));

builder.Services.AddScoped<SQLiteDriver>();

builder.Services.AddScoped<InMemoryDriver>(_ =>
{
    Console.WriteLine("InMemoryDriver");
    return new InMemoryDriver(new List<TodoItemDto>
{
    new TodoItemDto(1, "Todo 1", false),
    new TodoItemDto(2, "Todo 2", true),
    new TodoItemDto(3, "Todo 3", false),
    new TodoItemDto(4, "Todo 4", true),
    new TodoItemDto(5, "Todo 5", false),
    new TodoItemDto(6, "Todo 6", true),
    new TodoItemDto(7, "Todo 7", false),
    new TodoItemDto(8, "Todo 8", true),
    new TodoItemDto(9, "Todo 9", false),
    new TodoItemDto(10, "Todo 10", true),
});
});
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