using System.Data;
using Api.Controller;
using Api.Driver;
using Api.Gateway;
using Api.Middleware;
using Api.Usecase;
using Api.Usecase.Port;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
builder.Services.AddScoped<CreateTodoItemUsecase>();
builder.Services.AddScoped<StartTodoItemUsecase>();
builder.Services.AddScoped<TodoController>();
builder.Services.AddScoped<TodoStartController>();
builder.Services.AddScoped<CompleteTodoItemUsecase>();
builder.Services.AddScoped<TodoCompleteController>();

builder.Services.AddHealthChecks().AddNpgSql(connectionStringBuilder.ConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.MapHealthChecks("/ping", new HealthCheckOptions {
    ResponseWriter = async (context, report) => {
        var message = report.Status == HealthStatus.Healthy ? "OK" : "NG";
        await context.Response.WriteAsync(message);
    }
});
app.UseHttpsRedirection();

app.MapGet("/todoitems/", (TodoController controller, string? status) => {

    Console.WriteLine($"status: {status}");
    return controller.Get();

} );
app.MapPost("/todoitems/", (TodoController controller, CreateTodoItemRequest request) =>{
    return controller.Create(request);
});
app.MapPost("/todoitems/{id}/start", (TodoStartController controller, string id) => {
    return controller.Start(id);
});
app.MapPost("/todoitems/{id}/complete", (TodoCompleteController controller, string id) => {
    return controller.Complete(id);
});


app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("exception", () => {
    throw new Exception("エラーですよ！");
});

app.Run();