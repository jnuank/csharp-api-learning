using Api.Domain;
using Api.Usecase.Port;

namespace Api.Usecase;

public record StartTodoItemUsecase(ITodoItemPort TodoItemRepository)
{
	internal async Task Execute(Guid id)
	{
		TodoItem todoItem = await TodoItemRepository.GetById(id);

		TodoItem startedTodoItem = todoItem.Start();
		if (startedTodoItem == todoItem) {
			throw new NotImplementedException("Not implemented");
		}
		await TodoItemRepository.UpdateStated(startedTodoItem);

	}
}