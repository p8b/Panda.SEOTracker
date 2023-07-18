using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.BusinessLogic.Services
{
	public interface ISearchEngineService
	{
		Task<IEnumerable<SearchTermHistoryEntity>> GetSearchTermHistoriesAsync(
			TrackedUrlEntity trackUrl,
			CancellationToken cancellationToken = default);
	}
}