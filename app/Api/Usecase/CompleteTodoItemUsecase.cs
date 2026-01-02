using Api.Domain;
using Api.Usecase.Port;

namespace Api.Usecase;

public record CompleteTodoItemUsecase(ITodoItemPort TodoItemRepository)
{
	public async Task Execute(Guid id)
	{
		TodoItem todoItem = await TodoItemRepository.GetById(id);
		if (todoItem == null){
			throw new Exception("Todo item is not found");
		}
		TodoItemEvent completedEvent = todoItem.CompleteEvent();
		await TodoItemRepository.UpdateCompleted(completedEvent);
	}
}