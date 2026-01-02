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

	public async Task<TodoItemsOld> GetAll()
	{
		var items = await Driver.GetAll();


		return new TodoItemsOld
		(
			[.. items.Select(v => new TodoItemOld(v.Id, v.Name, v.IsComplete))]
		);
	}
}