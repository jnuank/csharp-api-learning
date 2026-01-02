namespace Api.Controller;

using Api.Domain;
using Api.Usecase;
public record TodoItemsResponse(List<TodoItemResponse> Items);
public record TodoItemResponse(int Id, string Name, bool IsComplete);

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
		return Results.Ok(result);
	}

	public async Task<IResult> Create(CreateTodoItemRequest request)
	{
		var todoItem = new TodoItem(request.Id, request.Name);
		await createTodoItemUsecase.Execute(todoItem);
		return Results.Ok();

	}
}

public class CreateTodoItemRequest
{
	public required string Name { get; set; }
	public required int Id { get; set; }
}