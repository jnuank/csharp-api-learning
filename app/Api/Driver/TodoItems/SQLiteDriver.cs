using System.Data;
using Dapper;

namespace Api.Driver;


public class SQLiteDriver
{
	private readonly IDbConnection connection;
	public SQLiteDriver(IDbConnection connection)
	{
		this.connection = connection;

		Console.WriteLine("テーブル作成");

		this.connection.Execute("DROP TABLE IF EXISTS todo_items");

		this.connection.Execute("""
		
		CREATE TABLE IF NOT EXISTS todo_items (
			id INTEGER PRIMARY KEY AUTOINCREMENT,
			name TEXT NOT NULL,
			is_complete BOOLEAN NOT NULL
		)
		""");

		Console.WriteLine("データ投入");
		this.connection.Execute("""
		INSERT INTO todo_items (name, is_complete) VALUES ('Todo 1', false);
		INSERT INTO todo_items (name, is_complete) VALUES ('Todo 2', true);
		INSERT INTO todo_items (name, is_complete) VALUES ('Todo 3', false);
		INSERT INTO todo_items (name, is_complete) VALUES ('Todo 4', true);
		INSERT INTO todo_items (name, is_complete) VALUES ('Todo 5', false);
		INSERT INTO todo_items (name, is_complete) VALUES ('Todo 6', true);
		INSERT INTO todo_items (name, is_complete) VALUES ('Todo 7', false);
		INSERT INTO todo_items (name, is_complete) VALUES ('Todo 8', true);
		INSERT INTO todo_items (name, is_complete) VALUES ('Todo 9', false);
		INSERT INTO todo_items (name, is_complete) VALUES ('Todo 10', true);
		""");
	}

	internal async Task<List<TodoItemDtoOld>> GetAll()
	{
		var items = await this.connection.QueryAsync<TodoItemDtoOld>(
			"SELECT id AS Id, name AS Name, is_complete AS IsComplete FROM todo_items"
		);
		return [.. items];
	}
}