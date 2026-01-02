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
				new TodoItemEvent(EventType.Completed, DateTime.UtcNow + TimeSpan.FromDays(3)),
				new TodoItemEvent(EventType.Started, DateTime.UtcNow + TimeSpan.FromDays(2)),
				new TodoItemEvent(EventType.Created, DateTime.UtcNow + TimeSpan.FromDays(1)),
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

	public TodoItem Start()
	{
		if(Status != Status.NotStarted) {
			return this with { };
		}
		return this with { 
			Events = [.. Events, new TodoItemEvent(EventType.Started, DateTime.UtcNow)]
		};
	}

	internal TodoItem Complete()
	{
		if(Status != Status.InProgress) {
			return this with { };
		}
		return this with { 
			Events = [.. Events, new TodoItemEvent(EventType.Completed, DateTime.UtcNow)] 
		};
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

public record TodoItemEvent(EventType EventType, DateTime OccurredAt);