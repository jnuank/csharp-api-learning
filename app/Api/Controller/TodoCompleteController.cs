using Api.Usecase;

namespace Api.Controller;

public class TodoCompleteController(CompleteTodoItemUsecase CompleteTodoItemUsecase)
{
	public async Task<IResult> Complete(string id)
	{
		await CompleteTodoItemUsecase.Execute(Guid.Parse(id));
		return Results.NoContent();
	}
}