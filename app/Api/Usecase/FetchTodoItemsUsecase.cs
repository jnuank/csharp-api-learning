namespace Api.Usecase;

using Api.Domain;
using Api.Usecase.Port;

public record FetchTodoItemsUsecase(ITodoItemPort todoItemRepository)
{
	public async Task<TodoItems> Execute()
	{
		var items = await todoItemRepository.GetAll();
		return items.NotDoneItems();
	}
}