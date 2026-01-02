using Api.Usecase;

namespace Api.Controller;

public class TodoStartController(StartTodoItemUsecase StartTodoItemUsecase)
{
	public async Task<IResult> Start(string id)
	{
		await StartTodoItemUsecase.Execute(Guid.Parse(id));
		return Results.NoContent();
	}


}