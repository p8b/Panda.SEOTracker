using FluentValidation;

using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;

namespace Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.CreateTrackedUrl
{
	internal class CreateTrackedUrlValidator : PandaAbstractValidator<CreateTrackedUrlDto>
	{
		public CreateTrackedUrlValidator()
		{
			RuleFor(x => x.Url)
			  .NotEmpty()
			  .MaximumLength(256);

			RuleFor(x => x.TotalResultsToCheck)
				.NotEmpty();

			RuleFor(x => x.SearchTerms)
				.NotEmpty();

			RuleFor(x => x.SearchTerms)
				.Must((x) => x.Distinct().Count() == x.Count)
				.WithMessage("List Contains duplicated values");
		}
	}
}