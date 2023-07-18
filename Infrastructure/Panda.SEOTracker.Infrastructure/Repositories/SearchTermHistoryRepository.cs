using Panda.SEOTracker.BusinessLogic.SearchTermHistoryLogic;
using Panda.SEOTracker.DataAccess;
using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.Infrastructure.Repositories;

public class SearchTermHistoryRepository : ISearchTermHistoryRepository
{
	private readonly SEOTrackerDbContext _db;

	public SearchTermHistoryRepository(SEOTrackerDbContext db)
	{
		_db = db;
	}

	public async Task AddRange(IEnumerable<SearchTermHistoryEntity> values, CancellationToken cancellationToken = default)
	{
		await _db.SearchTermHistories.AddRangeAsync(values, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);
	}
}