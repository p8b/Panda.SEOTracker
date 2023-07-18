using Microsoft.AspNetCore.Mvc;

using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.API.Extensions;

/// <summary>
///    Extensions for <see cref="IResult"/> instances.
/// </summary>
public static class IResultExtensions
{
	/// <summary>
	///    Converts an <see cref="IResult"/> into an <see cref="IActionResult"/>.
	/// </summary>
	public static async Task<IActionResult> ToActionResult(this Task<Dtos.Interfaces.IResult> result)
		=> (await result).ToActionResult();

	/// <summary>
	///    Converts an <see cref="IResult"/> into an <see cref="IActionResult"/>.
	/// </summary>
	public static IActionResult ToActionResult(this Dtos.Interfaces.IResult result)
		=> result.Status switch
		{
			ResultStatus.Created => new CreatedResult(string.Empty, null),
			ResultStatus.Succeeded => new NoContentResult(),
			ResultStatus.Unauthorised => new UnauthorizedObjectResult(result.Errors),
			ResultStatus.ValidationFailed => new BadRequestObjectResult(result.Errors),
			_ => new StatusCodeResult(StatusCodes.Status500InternalServerError),
		};

	/// <summary>
	///    Converts an <see cref="IResult{T}"/> into an <see cref="IActionResult"/>.
	/// </summary>
	public static async Task<IActionResult> ToActionResult<T>(this Task<IResult<T>> result, string createdLocation = "")
		=> (await result).ToActionResult(createdLocation);

	/// <summary>
	///    Converts an <see cref="IResult{T}"/> into an <see cref="IActionResult"/>.
	/// </summary>
	public static IActionResult ToActionResult<T>(this IResult<T> result, string createdLocation = "")
		=> result.Status switch
		{
			ResultStatus.Created => new CreatedResult(createdLocation, result.Data),
			ResultStatus.Succeeded => new OkObjectResult(result.Data),
			_ => ((Dtos.Interfaces.IResult)result).ToActionResult(),
		};

	/// <summary>
	///    Converts a <see cref="PaginatedResult{T}"/> into an <see cref="IActionResult"/>.
	/// </summary>
	public static async Task<IActionResult> ToActionResult<T>(this Task<IResultPaginated<T>> result)
		=> (await result).ToActionResult();

	/// <summary>
	///    Converts a <see cref="PaginatedResult{T}"/> into an <see cref="IActionResult"/>.
	/// </summary>
	public static IActionResult ToActionResult<T>(this IResultPaginated<T> result)
		=> result.Status switch
		{
			ResultStatus.Created => new StatusCodeResult(StatusCodes.Status500InternalServerError),
			ResultStatus.Succeeded => new OkObjectResult(result),
			_ => ((Dtos.Interfaces.IResult)result).ToActionResult(),
		};
}