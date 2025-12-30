
namespace Api.Driver;

public record TodoItemDto(int Id, string Name, bool IsComplete)
{
	public TodoItemDto(long id, string name, long isComplete)
		: this((int)id, name, isComplete == 1)
	{ }
}