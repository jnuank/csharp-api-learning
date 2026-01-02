using System.Data;
using Dapper;
using Npgsql;

namespace Api.Driver;

public class PostgresDriver
{
	private readonly string connectionString;
	public PostgresDriver(string connectionString)
	{
		this.connectionString = connectionString;
	}

	internal async Task<List<TodoItemDto>> GetAll()
	{
		using var connection = new NpgsqlConnection(this.connectionString);
		var items = await connection.QueryAsync<TodoItemDto>(
			"SELECT id AS Id, name AS Name FROM todo.items"
		);

		return [.. items];
	}

	internal async Task Create(TodoItemDto request)
	{
		using var connection = new NpgsqlConnection(this.connectionString);
		await connection.ExecuteAsync(
			"INSERT INTO todo.items (id, name) VALUES (@Id, @Name)",
			request
		);
	}

	internal async Task CreateCreated(Guid id)
	{
		using var connection = new NpgsqlConnection(this.connectionString);
		await connection.ExecuteAsync(
			"INSERT INTO todo.item_created (todo_item_id, created_at) VALUES (@Id, @CreatedAt)",
			new { Id = id, CreatedAt = DateTime.UtcNow }
		);
	}
}