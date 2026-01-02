
using Api.Domain;

namespace Api.Usecase.Port;
public interface ITodoItemPort
{
	Task<TodoItems> GetAll();
	Task<TodoItem> GetById(Guid id);
	Task Create(TodoItem request);
	Task UpdateStated(TodoItemEvent todoEvent);
	Task UpdateCompleted(TodoItemEvent todoEvent);
}