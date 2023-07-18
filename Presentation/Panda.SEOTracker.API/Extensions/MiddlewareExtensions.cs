using Panda.SEOTracker.API.Middleware;

namespace Panda.SEOTracker.API.Extensions;

/// <summary>
///    Middleware registration extensions.
/// </summary>
public static class MiddlewareExtensions
{
	/// <summary>
	///    Registers <see cref="ErrorHandlerMiddleware"/>;
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder app)
		=> app.UseMiddleware<ErrorHandlerMiddleware>();
}