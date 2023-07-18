using Panda.SEOTracker.Core.Enums;
using Panda.SEOTracker.Core.Interfaces;

namespace Panda.SEOTracker.Dtos.Dtos;

public record SearchTermHistoryDto : IEntity
{
	public required Guid Id { get; set; }
	public required DateTime Date { get; set; }
	public required SearchEngines SearchEngineUsed { get; set; }
	public IEnumerable<int> Positions { get; set; } = new List<int>();
}