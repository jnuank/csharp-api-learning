
namespace Api.Driver;

public record InMemoryDriver(List<TodoItemDto> memory)
{
	public Task<List<TodoItemDto>> GetAll()
	{
		return Task.FromResult(memory);
	}
}

public record TodoItemDto(int Id, string Name, bool IsComplete);