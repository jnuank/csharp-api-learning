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
				NpgsqlException => (StatusCodes.Status503ServiceUnavailable, ex.Message),
				_ => (StatusCodes.Status500InternalServerError, ex.Message)
			};

			context.Response.StatusCode = statusCode;
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsJsonAsync(new { error = message });
		}
	}
}