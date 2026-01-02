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
				DateTime createdAt = await Driver.GetCreated(v.Id);
				DateTime startedAt = await Driver.GetStarted(v.Id);
				DateTime completedAt = await Driver.GetCompleted(v.Id);

				Status status = Status.NotStarted;
				DateTime latest = 
					new[] { createdAt, startedAt, completedAt }.Max();
				if (latest == createdAt) {
					status = Status.NotStarted;
				} else if (latest == startedAt) {
					status = Status.InProgress;
				} else if (latest == completedAt) {
					status = Status.Done;
				}

				return new TodoItem(v.Id.ToString(), v.Name, status);
			}))]
		);
	}

	public async Task<TodoItem> GetById(Guid id)
	{
		TodoItemDto? item = await Driver.GetById(id);
		if (item == null) {
			throw new Exception("Todo item not found");
		}
		DateTime createdAt = await Driver.GetCreated(item.Id);
		DateTime startedAt = await Driver.GetStarted(item.Id);
		DateTime completedAt = await Driver.GetCompleted(item.Id);

		Status status = Status.NotStarted;
		DateTime latest = 
			new[] { createdAt, startedAt, completedAt }.Max();
		if (latest == createdAt) {
			status = Status.NotStarted;
		} else if (latest == startedAt) {
			status = Status.InProgress;
		} else if (latest == completedAt) {
			status = Status.Done;
		}
		return new TodoItem(item.Id.ToString(), item.Name, status);
	}

	public async Task UpdateStated(TodoItem todoItem)
	{
		await Driver.CreateStated(Guid.Parse(todoItem.Id!));
	}
}