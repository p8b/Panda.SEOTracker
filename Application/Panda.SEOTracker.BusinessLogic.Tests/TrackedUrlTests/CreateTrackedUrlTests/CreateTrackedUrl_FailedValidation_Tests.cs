using FluentAssertions;

using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.CreateTrackedUrl;
using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;
using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.Tests.TrackedUrlTests.CreateTrackedUrlTests
{
	public class CreateTrackedUrl_ValidationFailure_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_Create_TrackedUrl_With_Url_Above_Max_Length(
			CreateTrackedUrlCommand command,
			CreateTrackedUrlDto request)
		{
			// Arrange
			request.Url = GeneratorUtility.GetString(257);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(result);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.PropertyName == nameof(CreateTrackedUrlDto.Url));
		}

		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_Create_TrackedUrl_With_Url_Above_Min_Length(
			CreateTrackedUrlCommand command,
			CreateTrackedUrlDto request)
		{
			// Arrange
			request.Url = GeneratorUtility.GetString(0);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(result);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.PropertyName == nameof(CreateTrackedUrlDto.Url));
		}

		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_Create_TrackedUrl_When_TotalResultsToCheck_Is_Empty(
			CreateTrackedUrlCommand command,
			CreateTrackedUrlDto request)
		{
			// Arrange
			request.TotalResultsToCheck = 0;

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(result);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.PropertyName == nameof(CreateTrackedUrlDto.TotalResultsToCheck));
		}

		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_Create_TrackedUrl_When_SearchTerms_Is_Empty(
			CreateTrackedUrlCommand command,
			CreateTrackedUrlDto request)
		{
			// Arrange
			request.SearchTerms = new List<string>();

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(result);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.PropertyName == nameof(CreateTrackedUrlDto.SearchTerms));
		}

		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_Create_TrackedUrl_When_SearchTerms_Contains_Duplicates(
			CreateTrackedUrlCommand command,
			CreateTrackedUrlDto request)
		{
			// Arrange
			request.SearchTerms = new List<string>() { "A", "A" };

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(result);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.PropertyName == nameof(CreateTrackedUrlDto.SearchTerms));
		}

		private static void Assert(
			IResult<Guid> result)
		{
			result.Status.Should().Be(ResultStatus.ValidationFailed);
			result.Data.Should().Be(Guid.Empty);
		}
	}
}