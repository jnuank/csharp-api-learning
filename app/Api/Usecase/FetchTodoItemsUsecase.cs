namespace Api.Usecase;

using Api.Domain;
using Api.Usecase.Port;

public record FetchTodoItemsUsecase(ITodoItemPort todoItemRepository)
{
	public async Task<TodoItems> Execute()
	{
		return await Task.FromResult(new TodoItems(new List<TodoItem>
		{
			new TodoItem(1, "Todo 1", false),
			new TodoItem(2, "Todo 2", true),
			new TodoItem(3, "Todo 3", false),
		}));
	}
}