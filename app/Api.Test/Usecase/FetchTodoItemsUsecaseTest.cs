using System.Collections.Immutable;
using Api.Domain;
using Api.Domain.Base;
using Api.Usecase;
using Api.Usecase.Port;
using Moq;

namespace Api.Test.Usecase;

public class FetchTodoItemsUsecaseTest
{
	[Fact]
	public async Task TestExecute()
	{
		var dateTime = DateTime.UtcNow;
		var guid = Guid.NewGuid();
		var todoGuid = Guid.NewGuid();
		var mockPort = new Mock<ITodoItemPort>();
		var events = new SequenceEquatableList<TodoItemEvent>([new TodoItemEvent(todoGuid, EventType.Created, dateTime)]);
		mockPort.Setup(x => x.GetAll()).ReturnsAsync(new TodoItems([new TodoItem(todoGuid, "Todo 1", new TodoItemEvents(events))]));

		var sut = new FetchTodoItemsUsecase(mockPort.Object);
		var actual = await sut.Execute([]);
		Assert.Equal(new TodoItems([new TodoItem(todoGuid, "Todo 2", new TodoItemEvents(events))]), actual);
	}
}