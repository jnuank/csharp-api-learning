
public class TodoController
{
	public async Task<TodoItems> Get()
	{
		return await Task.FromResult(new TodoItems(new List<TodoItem>
		{
			new TodoItem(1, "Todo 1", false),
			new TodoItem(2, "Todo 2", true),
			new TodoItem(3, "Todo 3", false),
		}));

	}
}

public record TodoItems(List<TodoItem> Items);

public record TodoItem(int Id, string Name, bool IsComplete);