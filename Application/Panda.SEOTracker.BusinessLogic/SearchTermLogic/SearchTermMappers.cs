using Panda.SEOTracker.BusinessLogic.SearchTermHistoryLogic;
using Panda.SEOTracker.BusinessLogic.SearchTermLogic;
using Panda.SEOTracker.Dtos.Dtos;
using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.BusinessLogic.SearchTermLogic;

internal static class SearchTermMappers
{
	internal static SearchTermDto? MapToDto(this SearchTermEntity? value)
	{
		if (value is null) return null;

		return new SearchTermDto()
		{
			Id = value.Id,
			Term = value.Term,
			History = value.History.Select(x => x.MapToDto()!),
		};
	}
}