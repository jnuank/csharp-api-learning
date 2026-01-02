
namespace Api.Driver;

public record InMemoryDriver(List<TodoItemDtoOld> memory)
{
	public Task<List<TodoItemDtoOld>> GetAll()
	{
		return Task.FromResult(memory);
	}
}
