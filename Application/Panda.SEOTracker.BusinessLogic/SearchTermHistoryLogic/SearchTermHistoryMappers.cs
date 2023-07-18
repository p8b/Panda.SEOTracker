using Panda.SEOTracker.Dtos.Dtos;
using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.BusinessLogic.SearchTermHistoryLogic;

internal static class SearchTermHistoryMappers
{
	internal static SearchTermHistoryDto? MapToDto(this SearchTermHistoryEntity? value)
	{
		if (value is null) return null;

		return new SearchTermHistoryDto()
		{
			Id = value.Id,
			Date = value.Date,
			SearchEngineUsed = value.SearchEngineUsed,
			Positions = value.Positions,
		};
	}
}