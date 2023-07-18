using Panda.SEOTracker.Core.Interfaces;

namespace Panda.SEOTracker.Dtos.Dtos;

public record SearchTermDto : IEntity
{
	public required Guid Id { get; set; }
	public required string Term { get; set; }
	public IEnumerable<SearchTermHistoryDto> History { get; set; } = new List<SearchTermHistoryDto>();
}