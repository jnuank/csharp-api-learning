using System.Collections.Immutable;

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
		Events.Values.OrderByDescending(e => e.OccurredAt).First().EventType switch {
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
public record TodoItems(ImmutableList<TodoItem> Items)
{
	public virtual bool Equals(TodoItems? other)
	{
		if (other == null) return false;
		if (ReferenceEquals(this, other)) return true;
		return Items.SequenceEqual(other.Items);
	}

	public override int GetHashCode() {
		var hash = new HashCode();
		foreach (var item in Items) {
			hash.Add(item.GetHashCode());
		}
		return hash.ToHashCode();
	}

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

public record TodoItemEvents(ImmutableList<TodoItemEvent> Values)
{
	public virtual bool Equals(TodoItemEvents? other)
	{
		if (other == null) return false;
		if (ReferenceEquals(this, other)) return true;
		return Values.SequenceEqual(other.Values);
	}

	public override int GetHashCode() {
		var hash = new HashCode();
		foreach (var e in Values) {
			hash.Add(e.GetHashCode());
		}
		return hash.ToHashCode();
	}
}
