using Api.Domain;
using Api.Usecase.Port;

namespace Api.Usecase;

public record StartTodoItemUsecase(ITodoItemPort TodoItemRepository)
{
	internal async Task Execute(Guid id)
	{
		TodoItem todoItem = await TodoItemRepository.GetById(id);

		TodoItemEvent startedEvent = todoItem.StartEvent();
		await TodoItemRepository.UpdateStated(startedEvent);

	}
}