using Panda.SEOTracker.Core.Interfaces;

namespace Panda.SEOTracker.Entities;

public record SearchTermEntity : IEntity
{
	public required Guid Id { get; set; }
	public required string Term { get; set; }
	public List<SearchTermHistoryEntity> History { get; set; } = new();
	public required Guid TrackedUrlId { get; set; }
}