namespace Api.Usecase;

using Api.Domain;
using Api.Usecase.Port;

public record FetchTodoItemsUsecase(ITodoItemPort todoItemRepository)
{
	public async Task<TodoItemsOld> Execute()
	{
		var items = await todoItemRepository.GetAll();
		return items;
	}
}