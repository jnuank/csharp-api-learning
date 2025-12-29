public record TodoItems(List<TodoItem> Items);
public record TodoItem(int Id, string Name, bool IsComplete);

public class TodoController
{
	public async Task<IResult> Get()
	{
		return await Task.FromResult(Results.Ok(new TodoItems(new List<TodoItem>
		{
			new TodoItem(1, "Todo 1", false),
			new TodoItem(2, "Todo 2", true),
			new TodoItem(3, "Todo 3", false),
		})));
	}
}