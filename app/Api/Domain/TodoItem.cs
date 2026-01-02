using System.Reflection.Metadata.Ecma335;

namespace Api.Domain;

public record TodoItemOld(int Id, string Name, bool IsComplete);
public record TodoItemsOld(List<TodoItemOld> Items);


public record TodoItem
{
	public TodoItem(string? id, string name, List<TodoItemEvent> events, Status status = Status.NotStarted)
	{
		Id = id;
		Name = name;
		if (events.Count == 0) {
			Events = new List<TodoItemEvent>{
				new TodoItemEvent(Guid.NewGuid(), EventType.Completed, DateTime.UtcNow + TimeSpan.FromDays(3)),
				new TodoItemEvent(Guid.NewGuid(), EventType.Started, DateTime.UtcNow + TimeSpan.FromDays(2)),
				new TodoItemEvent(Guid.NewGuid(), EventType.Created, DateTime.UtcNow + TimeSpan.FromDays(1)),
			};
		} else {
			Events = events;
		}
	}

	public string? Id {
		get => field ?? throw new Exception("Id is not set");
		set;
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
		return new TodoItemEvent(Guid.Parse(Id!), EventType.Completed, DateTime.UtcNow);
	}

	internal TodoItemEvent StartEvent()
	{
		if (Status != Status.NotStarted) {
			throw new Exception("Todo item is not in not started status");
		}
		return new TodoItemEvent(Guid.Parse(Id!), EventType.Started, DateTime.UtcNow);
	}
}
public record TodoItems(List<TodoItem> Items);

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