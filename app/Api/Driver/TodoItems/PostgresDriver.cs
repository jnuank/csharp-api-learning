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
			"SELECT id AS Id, name AS Name, is_complete AS IsComplete FROM todo_items_old"
		);

		return [.. items];
	}
}