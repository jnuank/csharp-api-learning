
using Api.Domain;

namespace Api.Usecase.Port;
public interface ITodoItemPort
{
	Task<TodoItems> GetAll();
	Task<TodoItem> GetById(Guid id);
	Task Create(TodoItem request);
	Task UpdateStated(TodoItem todoItem);
	Task UpdateCompleted(TodoItem todoItem);
}