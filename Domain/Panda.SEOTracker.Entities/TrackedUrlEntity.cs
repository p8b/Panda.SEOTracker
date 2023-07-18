using Panda.SEOTracker.Core.Interfaces;

namespace Panda.SEOTracker.Entities;

public record TrackedUrlEntity : IEntity
{
	public required Guid Id { get; set; }
	public required string Url { get; set; }
	public required int TotalResultsToCheck { get; set; }
	public List<SearchTermEntity> SearchTerms { get; set; } = new();
}