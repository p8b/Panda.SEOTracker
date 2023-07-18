using Microsoft.Extensions.Logging;

using Panda.SEOTracker.Dtos;
using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.CreateTrackedUrl
{
	public class CreateTrackedUrlCommand : AbstractCommand<IResult<Guid>, CreateTrackedUrlDto>
	{
		protected string DefaultErrorMessage { get; }
			= AppMessages.UnableToCreate("TrackedUrl");

		private readonly ILogger<CreateTrackedUrlCommand> _logger;
		private readonly ITrackedUrlRepository _repository;

		public CreateTrackedUrlCommand(
			ITrackedUrlRepository repository,
			ILogger<CreateTrackedUrlCommand> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		public override async Task<IResult<Guid>> ExecuteAsync(
			CreateTrackedUrlDto request,
			CancellationToken cancellationToken = default)
		{
			// Validation
			if (!new CreateTrackedUrlValidator().IsValid(request, out var errors))
				return Result<Guid>.ValidationFailed(errors);

			try
			{
				// Action
				var result = await _repository.Create(request, cancellationToken);

				// Result
				return Result<Guid>.Created(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "{message}", AppMessages.Unexpected_error_in_repository);

				return Result<Guid>.InternalError(DefaultErrorMessage);
			}
		}
	}
}