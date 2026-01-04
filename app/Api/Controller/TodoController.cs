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

	public async Task<IResult> Get(string? status)
	{
		List<Status> filterStatuses = status.ToEnumList<Status>();

		TodoItems result = await usecase.Execute(filterStatuses);

		return Results.Ok(result.ToResponse());
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


public static class StringExtensions
{
	public static List<TEnum> ToEnumList<TEnum>(this string? source) where TEnum : struct, Enum
	{
		if (string.IsNullOrWhiteSpace(source)) return [];

		var values = source.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

		return [.. values.Select(v => Enum.Parse<TEnum>(v, ignoreCase: true))];
	}

}

public static class TodoItemsExtensions
{

	extension(TodoItems items)
	{
		public TodoItemsResponse ToResponse() => new([.. items.Items.Select(v => v.ToResponse())]);

	}

	extension(TodoItem item)
	{
		public TodoItemResponse ToResponse() => new(item.IdString, item.Name, item.StatusString);
	}

}