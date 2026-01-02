namespace Api.Domain;

public record TodoItemOld(int Id, string Name, bool IsComplete);
public record TodoItemsOld(List<TodoItemOld> Items);


public record TodoItem
{
	public TodoItem(Guid? id, string name, List<TodoItemEvent> events)
	{
		Id = id;
		Name = name;
		Events = events;
	}
	public Guid? Id {
		get => field ?? throw new Exception("Id is not set");
		init;
	 }
	public string Name { get; set; }

	public Status Status => 
		Events.OrderByDescending(e => e.OccurredAt).First().EventType switch {
			EventType.Created => Status.NotStarted,
			EventType.Started => Status.InProgress,
			EventType.Completed => Status.Done,
			_ => throw new Exception("oko"),
		};

	public List<TodoItemEvent> Events { get; set;}

	internal TodoItemEvent CompleteEvent()
	{
		if (Status != Status.InProgress) {
			throw new Exception("Todo item is not in in progress status");
		}
		return new TodoItemEvent(Id!.Value, EventType.Completed, DateTime.UtcNow);
	}

	internal TodoItemEvent StartEvent()
	{
		if (Status != Status.NotStarted) {
			throw new Exception("Todo item is not in not started status");
		}
		return new TodoItemEvent(Id!.Value, EventType.Started, DateTime.UtcNow);
	}
}
public record TodoItems(List<TodoItem> Items)
{
	internal TodoItems NotDoneItems(List<Status> filters)
	{
		return new TodoItems([.. Items.Where(v => filters.Contains(v.Status))]);
	}
}


public enum Status
{
	NotStarted,
	InProgress,
	Done
}

public enum EventType
{
	Created,
	Started,
	Completed
}

public record TodoItemEvent(Guid Id, EventType EventType, DateTime OccurredAt);