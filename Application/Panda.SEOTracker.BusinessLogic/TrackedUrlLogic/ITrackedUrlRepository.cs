using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;
using Panda.SEOTracker.Dtos.Interfaces;
using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;

public interface ITrackedUrlRepository
{
	Task<Guid> Create(CreateTrackedUrlDto request, CancellationToken cancellationToken = default);

	Task Delete(Guid request, CancellationToken cancellationToken);

	Task<bool> IsValidId(Guid request, CancellationToken cancellationToken = default);

	Task<TrackedUrlEntity?> Get(Guid request, CancellationToken cancellationToken = default);

	Task<IResultPaginated<TrackedUrlEntity>> Search(SearchTrackedUrlDto request, CancellationToken cancellationToken = default);
}