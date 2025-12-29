namespace Api.Controller;

using Api.Gateway;
using Api.Usecase;
public record TodoItemsResponse(List<TodoItemResponse> Items);
public record TodoItemResponse(int Id, string Name, bool IsComplete);

public class TodoController
{
	private readonly FetchTodoItemsUsecase usecase;

	public TodoController(FetchTodoItemsUsecase usecase)
	{
		this.usecase = usecase;
	}

	public async Task<IResult> Get()
	{
		var result = await usecase.Execute();
		return Results.Ok(result);
	}
}