using System.Collections.Immutable;
using Api.Domain;
using Api.Domain.Base;

namespace Api.Test.Domain;

public class TodoItemsTest
{
	[Fact]
	public void TodoItemの等価テスト()
	{

		var guid = Guid.NewGuid();
		var eventGuid = Guid.NewGuid();
		var dateTime = DateTime.UtcNow;
		ImmutableList<TodoItemEvent> events = [new TodoItemEvent(eventGuid, EventType.Created, dateTime)];
		ImmutableList<TodoItemEvent> event2 = [
			new TodoItemEvent(eventGuid, EventType.Created, dateTime),
		];

		var expected = new TodoItem(guid, "Todo 1", new TodoItemEvents(events));
		var actual = new TodoItem(guid, "Todo 1", new TodoItemEvents(event2));
		Assert.Equal(expected, actual);
	}


	[Fact]
	public void TodoItemsの等価テスト()
	{
		var guid = Guid.NewGuid();
		var eventGuid = Guid.NewGuid();
		var dateTime = DateTime.UtcNow;
		ImmutableList<TodoItemEvent> events = [new TodoItemEvent(eventGuid, EventType.Created, dateTime)];

		var actual = new TodoItems([new TodoItem(guid, "Todo 1", new TodoItemEvents(events))]);
		var expected = new TodoItems([new TodoItem(guid, "Todo 1", new TodoItemEvents(events))]);

		Assert.Equal(expected, actual);
	}
}