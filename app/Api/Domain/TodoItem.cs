using System.Diagnostics.CodeAnalysis;

namespace Api.Domain;

public record TodoItemOld(int Id, string Name, bool IsComplete);
public record TodoItemsOld(List<TodoItemOld> Items);


public record TodoItem
{
	public TodoItem(string? id, string name)
	{
		Id = id;
		Name = name;
	}

	public string? Id {
		get => field ?? throw new Exception("Id is not set");
		set;
	}
	public string Name { get; set; }

}
public record TodoItems(List<TodoItem> Items);
