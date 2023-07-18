using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.Dtos;

public record ValidationError : IValidationError
{
	public required string PropertyName { get; init; }

	public required string Message { get; init; }

	public object? AttemptedValue { get; init; }
}