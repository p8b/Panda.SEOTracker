using Microsoft.Extensions.Logging;

using Panda.SEOTracker.BusinessLogic.SearchTermHistoryLogic;
using Panda.SEOTracker.BusinessLogic.Services;
using Panda.SEOTracker.Dtos;
using Panda.SEOTracker.Dtos.Dtos;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.GetLatestTrackingInformation;

public class GetLatestTrackingInformationCommand : AbstractCommand<IResult<TrackedUrlDto>, Guid>
{
	protected string DefaultErrorMessage { get; }
		= AppMessages.UnableToGet("TrackedUrl");

	private readonly ILogger<GetLatestTrackingInformationCommand> _logger;
	private readonly ITrackedUrlRepository _repository;
	private readonly ISearchTermHistoryRepository _searchTermsHistoryRepository;
	private readonly ISearchEngineService _searchEngineService;

	public GetLatestTrackingInformationCommand(
		ITrackedUrlRepository repository,
		ISearchTermHistoryRepository searchTermsHistoryRepository,
		ISearchEngineService searchEngineService,
		ILogger<GetLatestTrackingInformationCommand> logger)
	{
		_repository = repository;
		_searchTermsHistoryRepository = searchTermsHistoryRepository;
		_searchEngineService = searchEngineService;
		_logger = logger;
	}

	public override async Task<IResult<TrackedUrlDto>> ExecuteAsync(Guid request, CancellationToken cancellationToken = default)
	{
		// Validation
		var errors = await new TrackedUrlIdValidator(_repository).IsValidAsync(request);
		if (errors.Any()) return Result<TrackedUrlDto>.ValidationFailed(errors);

		try
		{
			// Action
			var trackUrl = await _repository.Get(request, cancellationToken);

			// The search was performed today
			if (!trackUrl!.SearchTerms.Any(x => x.History.Any(h => h.Date.Date == DateTime.UtcNow.Date)))
			{
				var latestHistory = await _searchEngineService.GetSearchTermHistoriesAsync(trackUrl, cancellationToken);

				if (latestHistory.Any())
					await _searchTermsHistoryRepository.AddRange(latestHistory);

				trackUrl = await _repository.Get(request, cancellationToken)!;
			}

			// Result
			return Result<TrackedUrlDto>.Succeeded(trackUrl.MapToDto()!);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "{message}", AppMessages.Unexpected_error_in_repository);

			return Result<TrackedUrlDto>.InternalError(DefaultErrorMessage);
		}
	}
}