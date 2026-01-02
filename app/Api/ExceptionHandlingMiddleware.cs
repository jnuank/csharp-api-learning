using Microsoft.OpenApi;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			Console.WriteLine("ここを通った");
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsJsonAsync(new { error = ex.Message });
		}
	}
}