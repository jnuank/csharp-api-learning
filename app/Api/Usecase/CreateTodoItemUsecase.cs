using Api.Domain;
using Api.Usecase.Port;

namespace Api.Usecase;

public record CreateTodoItemUsecase(ITodoItemPort TodoItemRepository)
{
	internal async Task Execute(TodoItem request)
	{
		await TodoItemRepository.Create(request);
	}
}