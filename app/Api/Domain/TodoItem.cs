using Api.Domain.Base;

namespace Api.Domain;

public record TodoItemOld(int Id, string Name, bool IsComplete);
public record TodoItemsOld(List<TodoItemOld> Items);


public record TodoItem
{
	public TodoItem(Guid? id, string name, TodoItemEvents events)
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
		Events.LastEvent.EventType switch {
			EventType.Created => Status.NotStarted,
			EventType.Started => Status.InProgress,
			EventType.Completed => Status.Done,
			_ => throw new Exception("oko"),
		};

	public string StatusString => Status.ToString();

	public TodoItemEvents Events { get; set;}

	internal TodoItemEvent CompleteEvent()
	{
		if (Status != Status.InProgress) {
			throw new Exception("Todo item is not in in progress status");
		}
		return new TodoItemEvent(Id!.Value, EventType.Completed, DateTime.UtcNow);
	}

	public string IdString => Id!.Value.ToString();

	internal TodoItemEvent StartEvent()
	{
		if (Status != Status.NotStarted) {
			throw new Exception("Todo item is not in not started status");
		}
		return new TodoItemEvent(Id!.Value, EventType.Started, DateTime.UtcNow);
	}
}
public record TodoItems(SequenceEquatableList<TodoItem> Items)
{
	internal TodoItems FilterByStatuses(List<Status> statuses)
	{
		return statuses switch {
			[] => new TodoItems([.. Items.Where(v => v.Status != Status.Done)]),
			[..] => new TodoItems([.. Items.Where(v => statuses.Contains(v.Status))]),
		};
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

public record TodoItemEvents(SequenceEquatableList<TodoItemEvent> Values)
{
	internal TodoItemEvent LastEvent => Values.MaxBy(e => e.OccurredAt) ?? throw new Exception("No events");
}

