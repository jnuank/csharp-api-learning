namespace Api.Gateway;

using System.Threading.Tasks;
using Api.Domain;
using Api.Driver;
using Api.Usecase.Port;

public record TodoItemGateway(SQLiteDriver driver) : ITodoItemPort
{
	public async Task<TodoItems> GetAll()
	{
		var items = await driver.GetAll();


		return new TodoItems
		(
			[.. items.Select(v => new TodoItem(v.Id, v.Name, v.IsComplete))]
		);
	}
}