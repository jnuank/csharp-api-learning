namespace Api.Gateway;

using System.ComponentModel.Design;
using System.Threading.Tasks;
using Api.Domain;
using Api.Driver;
using Api.Usecase.Port;

public record TodoItemGateway(PostgresDriver Driver) : ITodoItemPort
{
	public async Task Create(TodoItem request)
	{
		Guid id = Guid.NewGuid();
		var todoItemDto = new TodoItemDto(id, request.Name);
		await Driver.Create(todoItemDto);
		await Driver.CreateCreated(id);
	}

	public async Task<TodoItems> GetAll()
	{
		var items = await Driver.GetAll();

		return new TodoItems
		(
			[.. await Task.WhenAll(items.Select(async v => {
				List<DateTime> createdAt = await Driver.GetCreated(v.Id);
				List<DateTime> startedAt = await Driver.GetStarted(v.Id);
				List<DateTime> completedAt = await Driver.GetCompleted(v.Id);
				List<TodoItemEvent> events = [
					..createdAt.Select(e => new TodoItemEvent(v.Id, EventType.Created, e)),
					..startedAt.Select(e => new TodoItemEvent(v.Id, EventType.Started, e)),
					..completedAt.Select(e => new TodoItemEvent(v.Id, EventType.Completed, e)),
				];

				return new TodoItem(v.Id, v.Name, events);
			}))]
		);
	}

	public async Task<TodoItem> GetById(Guid id)
	{
		TodoItemDto? item = await Driver.GetById(id);
		if (item == null) {
			throw new Exception("Todo item not found");
		}
		List<DateTime> createdAt = await Driver.GetCreated(item.Id);
		List<DateTime> startedAt = await Driver.GetStarted(item.Id);
		List<DateTime> completedAt = await Driver.GetCompleted(item.Id);

		List<TodoItemEvent> events = [
			..createdAt.Select(v => new TodoItemEvent(item.Id, EventType.Created, v)),
			..startedAt.Select(v => new TodoItemEvent(item.Id, EventType.Started, v)),
			..completedAt.Select(v => new TodoItemEvent(item.Id, EventType.Completed, v)),
		];
		return new TodoItem(item.Id, item.Name, events);
	}

	public async Task UpdateCompleted(TodoItemEvent todoEvent)
	{
		await Driver.CreateCompleted(todoEvent.Id);
	}

	public async Task UpdateStated(TodoItemEvent todoEvent)
	{
		await Driver.CreateStated(todoEvent.Id);
	}
}