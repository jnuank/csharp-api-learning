using Npgsql;

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
			var (statusCode, message) = ex switch
			{
				NpgsqlException => (StatusCodes.Status503ServiceUnavailable, "データベースエラーが発生しました"),
				_ => (StatusCodes.Status500InternalServerError, "予期せぬエラーが発生しました")
			};

			context.Response.StatusCode = statusCode;
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsJsonAsync(new { error = message });
		}
	}
}