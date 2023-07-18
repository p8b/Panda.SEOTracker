using Panda.SEOTracker.Core.Interfaces;

namespace Panda.SEOTracker.Dtos.Dtos;

public record TrackedUrlDto : IEntity
{
	public required Guid Id { get; set; }
	public required string Url { get; set; }
	public required int TotalResultsToCheck { get; set; }
	public IEnumerable<SearchTermDto> SearchTerms { get; set; } = new List<SearchTermDto>();
}