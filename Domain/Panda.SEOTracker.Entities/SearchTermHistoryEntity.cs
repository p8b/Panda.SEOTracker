using Panda.SEOTracker.Core.Enums;
using Panda.SEOTracker.Core.Interfaces;

namespace Panda.SEOTracker.Entities;

public record SearchTermHistoryEntity : IEntity
{
	public required Guid Id { get; set; }
	public required DateTime Date { get; set; }
	public required SearchEngines SearchEngineUsed { get; set; }
	public ICollection<int> Positions { get; set; } = new List<int>();
	public required Guid SearchTermId { get; set; }
}