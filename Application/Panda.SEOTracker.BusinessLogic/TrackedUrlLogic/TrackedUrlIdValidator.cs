using FluentValidation;

namespace Panda.SEOTracker.BusinessLogic.TrackedUrlLogic
{
	internal class TrackedUrlIdValidator : PandaAbstractValidator<Guid>
	{
		public TrackedUrlIdValidator(ITrackedUrlRepository repository)
		{
			RuleFor(x => x)
				.NotEmpty()
				.MustAsync(repository.IsValidId)
				.WithMessage(AppMessages.NotFound("Tracked Url"));
		}
	}
}