using System.Net;

namespace Panda.SEOTracker.API.Middleware
{
	/// <summary>
	///    This middleware is used to catch exception globally.
	/// </summary>
	public class ErrorHandlerMiddleware
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ILogger<ErrorHandlerMiddleware> _logger;
		private readonly RequestDelegate _next;

		/// <summary>
		///    This middleware is used to catch exception globally.
		/// </summary>
		/// <remarks>
		///    In development environment the exception message will be shown, otherwise default
		///    error message will be shown.
		/// </remarks>
		public ErrorHandlerMiddleware(
			RequestDelegate next,
			IWebHostEnvironment webHostEnvironment,
			ILogger<ErrorHandlerMiddleware> logger)
		{
			_next = next;
			_logger = logger;
			_environment = webHostEnvironment;
		}

		/// <inheritdoc/>
		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				var response = context.Response;
				response.ContentType = "text/plain";
				response.StatusCode = (int)HttpStatusCode.InternalServerError;

				_logger.LogCritical(ex, "Unhandled Exception");
				if (_environment.IsDevelopment())
					await response.WriteAsync(ex.Message);
				else
					await response.WriteAsync("Unexpected server error has occurred.");
			}
		}
	}
}