namespace Api.Usecase;

using Api.Domain;
using Api.Usecase.Port;

public record FetchTodoItemsUsecase(ITodoItemPort todoItemRepository)
{
	public async Task<TodoItems> Execute(List<Status> filters)
	{
		var items = await todoItemRepository.GetAll();
		return items.NotDoneItems(filters);
	}
}