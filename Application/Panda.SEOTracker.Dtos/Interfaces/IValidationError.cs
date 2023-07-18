namespace Panda.SEOTracker.Dtos.Interfaces;

public interface IValidationError
{
	public string PropertyName { get; }
	public string Message { get; }
	public object? AttemptedValue { get; }
}