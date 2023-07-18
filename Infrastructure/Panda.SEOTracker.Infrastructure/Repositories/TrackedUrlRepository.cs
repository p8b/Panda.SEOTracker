using Microsoft.EntityFrameworkCore;

using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.DataAccess;
using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;
using Panda.SEOTracker.Dtos.Interfaces;
using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.Infrastructure.Repositories;

public class TrackedUrlRepository : ITrackedUrlRepository
{
	private readonly SEOTrackerDbContext _db;

	public TrackedUrlRepository(SEOTrackerDbContext db)
	{
		_db = db;
	}

	public async Task<Guid> Create(CreateTrackedUrlDto request, CancellationToken cancellationToken = default)
	{
		var newEntity = new TrackedUrlEntity()
		{
			Id = Guid.Empty,
			Url = request.Url,
			TotalResultsToCheck = request.TotalResultsToCheck,
		};

		foreach (var term in request.SearchTerms)
		{
			newEntity.SearchTerms.Add(new SearchTermEntity
			{
				Id = Guid.Empty,
				TrackedUrlId = newEntity.Id,
				Term = term
			});
		}

		await _db.AddAsync(newEntity, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);

		return newEntity.Id;
	}

	public async Task Delete(Guid request, CancellationToken cancellationToken = default)
	{
		var currentEntity = await _db.TrackedUrls
			.AsNoTrackingWithIdentityResolution()
			.SingleOrDefaultAsync(x => x.Id == request, cancellationToken: cancellationToken);

		if (currentEntity is null) return;

		_db.TrackedUrls.Remove(currentEntity);
		await _db.SaveChangesAsync(cancellationToken);
	}

	public Task<TrackedUrlEntity?> Get(
		Guid request,
		CancellationToken cancellationToken = default)
		=> _db.TrackedUrls
			.Include(x => x.SearchTerms)
			.ThenInclude(x => x.History)
			.AsNoTrackingWithIdentityResolution()
			.SingleOrDefaultAsync(x => x.Id == request, cancellationToken);

	public Task<bool> IsValidId(Guid request, CancellationToken cancellationToken = default)
		=> _db.TrackedUrls
			.AsNoTrackingWithIdentityResolution()
			.AnyAsync(x => x.Id == request, cancellationToken);

	public Task<IResultPaginated<TrackedUrlEntity>> Search(SearchTrackedUrlDto request, CancellationToken cancellationToken)
		=> _db.TrackedUrls
			.Include(x => x.SearchTerms)
			.ThenInclude(x => x.History)
			.Where(x => string.IsNullOrWhiteSpace(request.SearchValue)
					 || x.Url.Contains(request.SearchValue))
			.AsNoTrackingWithIdentityResolution()
			.ToPaginatedListAsync(request, cancellationToken);
}