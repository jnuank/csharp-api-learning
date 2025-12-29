
using Api.Domain;

namespace Api.Usecase.Port;
public interface ITodoItemPort
{
	Task<TodoItems> GetAll();
}