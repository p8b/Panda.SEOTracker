using FluentValidation;
using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;

namespace Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.SearchTrackedUrl
{
    internal class SearchTrackedUrlValidator : PandaAbstractValidator<SearchTrackedUrlDto>
	{
		public SearchTrackedUrlValidator()
		{
			RuleFor(x => x.SearchValue)
				.MaximumLength(256);

			RuleFor(x => x.SortBy)
				.MaximumLength(256);

			RuleFor(x => x.PageNumber)
				.GreaterThanOrEqualTo(1);

			RuleFor(x => x.PageSize)
				.GreaterThanOrEqualTo(10);

			RuleFor(x => x.Direction)
				.IsInEnum();
		}
	}
}