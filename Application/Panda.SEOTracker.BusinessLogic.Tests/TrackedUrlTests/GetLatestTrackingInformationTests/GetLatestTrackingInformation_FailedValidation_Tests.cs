using FluentAssertions;

using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.GetLatestTrackingInformation;
using Panda.SEOTracker.Dtos.Dtos;
using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.Tests.TrackedUrlTests.GetLatestTrackingInformationTests
{
	public class GetLatestTrackingInformation_ValidationFailure_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_GetLatestTrackingInformation_With_Empty_Id(
			GetLatestTrackingInformationCommand command,
			Guid request)
		{
			// Arrange
			request = Guid.Empty;

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(result);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.PropertyName == nameof(TrackedUrlDto.Id));
		}

		private static void Assert(
			IResult result)
		{
			result.Status.Should().Be(ResultStatus.ValidationFailed);
		}
	}
}