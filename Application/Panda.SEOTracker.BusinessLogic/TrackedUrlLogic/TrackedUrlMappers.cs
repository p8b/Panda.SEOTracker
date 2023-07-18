using Panda.SEOTracker.BusinessLogic.SearchTermLogic;
using Panda.SEOTracker.Dtos.Dtos;
using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;

internal static class TrackedUrlMappers
{
	internal static TrackedUrlDto? MapToDto(this TrackedUrlEntity? value)
	{
		if (value is null) return null;

		return new TrackedUrlDto()
		{
			Id = value.Id,
			Url = value.Url,
			TotalResultsToCheck = value.TotalResultsToCheck,
			SearchTerms = value.SearchTerms.Select(x => x.MapToDto()!),
		};
	}
}