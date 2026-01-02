namespace Api.Gateway;

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

		var tasks = items.Select(async v => new TodoItem(v.Id, v.Name, await GetEvents(v.Id)));

		var todoItems = await Task.WhenAll(tasks);

		return new TodoItems([.. todoItems]);
	}

	public async Task<TodoItem> GetById(Guid id)
	{
		TodoItemDto? item = await Driver.GetById(id);
		if (item == null) {
			throw new Exception("Todo item not found");
		}

		List<TodoItemEvent> events = await GetEvents(item.Id);

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

	private async Task<List<TodoItemEvent>> GetEvents(Guid id)
	{
		var createdAtTask = Driver.GetCreated(id);
		var startedAtTask = Driver.GetStarted(id);
		var completedAtTask = Driver.GetCompleted(id);

		await Task.WhenAll(createdAtTask, startedAtTask, completedAtTask);

		List<DateTime> createdAt = createdAtTask.Result;
		List<DateTime> startedAt = startedAtTask.Result;
		List<DateTime> completedAt = completedAtTask.Result;

		List<TodoItemEvent> events = [
			..createdAt.Select(v => new TodoItemEvent(id, EventType.Created, v)),
			..startedAt.Select(v => new TodoItemEvent(id, EventType.Started, v)),
			..completedAt.Select(v => new TodoItemEvent(id, EventType.Completed, v)),
		];

		return events;
	}
}