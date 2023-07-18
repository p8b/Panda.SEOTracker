using Microsoft.Extensions.Logging;

using Panda.SEOTracker.Dtos;
using Panda.SEOTracker.Dtos.Dtos;
using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.SearchTrackedUrl
{
	public class SearchTrackedUrlCommand : AbstractCommand<IResultPaginated<TrackedUrlDto>, SearchTrackedUrlDto>
	{
		protected string DefaultErrorMessage { get; }
			= AppMessages.UnableToPerformSearchOn("Tracked Urls");
		private readonly ILogger<SearchTrackedUrlCommand> _logger;
		private readonly ITrackedUrlRepository _repository;

		public SearchTrackedUrlCommand(
			ITrackedUrlRepository repository,
			ILogger<SearchTrackedUrlCommand> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		public override async Task<IResultPaginated<TrackedUrlDto>> ExecuteAsync(
			SearchTrackedUrlDto request,
			CancellationToken cancellationToken = default)
		{
			// Validation
			if (!new SearchTrackedUrlValidator().IsValid(request, out var errors))
				return ResultPaginated<TrackedUrlDto>.ValidationFailed(errors);

			try
			{
				// Action
				var result = await _repository.Search(request, cancellationToken);

				// Result
				return ResultPaginated<TrackedUrlDto>.Succeeded(result, x => x.MapToDto()!);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "{message}", AppMessages.Unexpected_error_in_repository);

				return ResultPaginated<TrackedUrlDto>.InternalError(DefaultErrorMessage);
			}
		}
	}
}