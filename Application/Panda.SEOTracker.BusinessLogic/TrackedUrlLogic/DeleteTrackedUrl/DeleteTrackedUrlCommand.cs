using Microsoft.Extensions.Logging;

using Panda.SEOTracker.Dtos;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.DeleteTrackedUrl
{
	public class DeleteTrackedUrlCommand : AbstractCommand<IResult, Guid>
	{
		protected string DefaultErrorMessage { get; }
			= AppMessages.UnableToDelete("TrackedUrl");

		private readonly ILogger<DeleteTrackedUrlCommand> _logger;
		private readonly ITrackedUrlRepository _repository;

		public DeleteTrackedUrlCommand(
			ITrackedUrlRepository repository,
			ILogger<DeleteTrackedUrlCommand> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		public override async Task<IResult> ExecuteAsync(Guid request, CancellationToken cancellationToken = default)
		{
			// Validation
			var errors = await new TrackedUrlIdValidator(_repository).IsValidAsync(request);
			if (errors.Any()) return Result.ValidationFailed(errors);

			try
			{
				// Action
				await _repository.Delete(request, cancellationToken);

				// Result
				return Result.Succeeded();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "{message}", AppMessages.Unexpected_error_in_repository);

				return Result<Guid>.InternalError(DefaultErrorMessage);
			}
		}
	}
}