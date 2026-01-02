namespace Api.Domain;

public record TodoItemOld(int Id, string Name, bool IsComplete);
public record TodoItemsOld(List<TodoItemOld> Items);


public record TodoItem(int Id, string Name);
