
namespace Api.Driver;

public record TodoItemDtoOld(int Id, string Name, bool IsComplete)
{
	public TodoItemDtoOld(long id, string name, long isComplete)
		: this((int)id, name, isComplete == 1)
	{ }
}

public record TodoItemDto(Guid Id, string Name);