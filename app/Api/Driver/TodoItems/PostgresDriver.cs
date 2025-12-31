using System.Data;
using Dapper;

namespace Api.Driver;

public class PostgresDriver
{
	private readonly IDbConnection connection;
	public PostgresDriver(IDbConnection connection)
	{
		this.connection = connection;

		// Console.WriteLine("postgresテーブル作成");

		// this.connection.Execute("DROP TABLE IF EXISTS todo_items");

		// this.connection.Execute("""
		// CREATE TABLE IF NOT EXISTS todo_items (
		// 	id SERIAL PRIMARY KEY,
		// 	name TEXT NOT NULL,
		// 	is_complete BOOLEAN NOT NULL
		// )
		// """);

		// Console.WriteLine("postgresデータ投入");
		// this.connection.Execute("""
		// INSERT INTO todo_items (name, is_complete) VALUES ('Todo 1', false);
		// INSERT INTO todo_items (name, is_complete) VALUES ('Todo 2', true);
		// INSERT INTO todo_items (name, is_complete) VALUES ('Todo 3', false);
		// INSERT INTO todo_items (name, is_complete) VALUES ('Todo 4', true);
		// INSERT INTO todo_items (name, is_complete) VALUES ('Todo 5', false);
		// INSERT INTO todo_items (name, is_complete) VALUES ('Todo 6', true);
		// INSERT INTO todo_items (name, is_complete) VALUES ('Todo 7', false);
		// INSERT INTO todo_items (name, is_complete) VALUES ('Todo 8', true);
		// INSERT INTO todo_items (name, is_complete) VALUES ('Todo 9', false);
		// INSERT INTO todo_items (name, is_complete) VALUES ('Todo 10', true);
		// """);
	}

	internal async Task<List<TodoItemDto>> GetAll()
	{
		var items = await this.connection.QueryAsync<TodoItemDto>(
			"SELECT id AS Id, name AS Name, is_complete AS IsComplete FROM todo_items"
		);
		Thread.Sleep(10000);

		return [.. items];
	}
}