using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.BusinessLogic.SearchTermHistoryLogic;

public interface ISearchTermHistoryRepository
{
	Task AddRange(IEnumerable<SearchTermHistoryEntity> values, CancellationToken cancellationToken = default);
}