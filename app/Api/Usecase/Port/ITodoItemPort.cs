
using Api.Domain;

namespace Api.Usecase.Port;
public interface ITodoItemPort
{
	Task<TodoItemsOld> GetAll();
	Task Create(TodoItem request);
}