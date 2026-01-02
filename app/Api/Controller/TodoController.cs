namespace Api.Controller;

using Api.Domain;
using Api.Usecase;
public record TodoItemsResponse(List<TodoItemResponse> Items);
public record TodoItemResponse(string Id, string Name, string Status);

public class TodoController
{
	private readonly FetchTodoItemsUsecase usecase;
	private readonly CreateTodoItemUsecase createTodoItemUsecase;

	public TodoController(FetchTodoItemsUsecase usecase, CreateTodoItemUsecase createTodoItemUsecase)
	{
		this.usecase = usecase;
		this.createTodoItemUsecase = createTodoItemUsecase;
	}

	public async Task<IResult> Get()
	{
		var result = await usecase.Execute();
		return Results.Ok(new TodoItemsResponse(result.Items.Select(v => new TodoItemResponse(v.Id!, v.Name, v.Status.ToString())).ToList()));
	}

	public async Task<IResult> Create(CreateTodoItemRequest request)
	{
		var todoItem = new TodoItem(null, request.Name, new List<TodoItemEvent>());
		await createTodoItemUsecase.Execute(todoItem);
		return Results.Ok();

	}
}

public class CreateTodoItemRequest
{
	public required string Name { get; set; }
}
