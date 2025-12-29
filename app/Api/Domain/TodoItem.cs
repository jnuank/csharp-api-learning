namespace Api.Domain;

public record TodoItem(int Id, string Name, bool IsComplete);
public record TodoItems(List<TodoItem> Items);