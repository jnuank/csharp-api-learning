using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Api.Domain;
using Api.Usecase;
using Api.Usecase.Port;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
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
		mockPort.Setup(x => x.GetAll()).ReturnsAsync(new TodoItems([new TodoItem(todoGuid, "Todo 1", new TodoItemEvents([new TodoItemEvent(guid, EventType.Created, dateTime)]))]));

		var sut = new FetchTodoItemsUsecase(mockPort.Object);
		var actual = await sut.Execute([]);
		Assert.Equal(new TodoItems([new TodoItem(todoGuid, "Todo 1", new TodoItemEvents([new TodoItemEvent(guid, EventType.Created, dateTime)]))]), actual);
	}
}