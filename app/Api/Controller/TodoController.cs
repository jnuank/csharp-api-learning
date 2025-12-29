namespace Api.Controller;

using Api.Gateway;
using Api.Usecase;
public record TodoItemsResponse(List<TodoItemResponse> Items);
public record TodoItemResponse(int Id, string Name, bool IsComplete);

public class TodoController
{
	public async Task<IResult> Get()
	{
		var usecase = new FetchTodoItemsUsecase(new TodoItemGateway());
		var result = await usecase.Execute();
		return await Task.FromResult(Results.Ok(result));
	}
}