using FluentAssertions;

using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.SearchTrackedUrl;
using Panda.SEOTracker.Dtos.Dtos;
using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;
using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.Tests.TrackedUrlTests.SearchTrackedUrlTests
{
	public class SearchTrackedUrl_ValidationFailure_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_SearchTrackedUrl_With_SearchValue_Above_Max_Length(
			SearchTrackedUrlCommand command,
			SearchTrackedUrlDto request)
		{
			// Arrange
			request.SearchValue = GeneratorUtility.GetString(257);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(result);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.PropertyName == nameof(SearchTrackedUrlDto.SearchValue));
		}

		private static void Assert(
			IResultPaginated<TrackedUrlDto> result)
		{
			result.Status.Should().Be(ResultStatus.ValidationFailed);
		}
	}
}