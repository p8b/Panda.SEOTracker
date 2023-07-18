using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.SearchTrackedUrl;
using Panda.SEOTracker.Dtos.Dtos;
using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;
using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.Tests.TrackedUrlTests.SearchTrackedUrlTests
{
	public class SearchTrackedUrl_InternalError_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_SearchTrackedUrl_When_Repository_Throws_Exception(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ILogger<SearchTrackedUrlCommand>> logger,
			SearchTrackedUrlDto request)
		{
			// Arrange
			request.PageNumber = 1;
			request.PageSize = 10;
			var command = Setup(
				repositoryMock,
				logger);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(result);
		}

		private static void Assert(
			IResultPaginated<TrackedUrlDto> result)
		{
			result.Status.Should().Be(ResultStatus.InternalError);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.Message == AppMessages.UnableToPerformSearchOn("Tracked Urls"));
		}

		private static SearchTrackedUrlCommand Setup(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ILogger<SearchTrackedUrlCommand>> logger)
		{
			//Arrange
			repositoryMock.Setup(x => x.Search(
				It.IsAny<SearchTrackedUrlDto>(),
				It.IsAny<CancellationToken>()
				))
				.Throws(new Exception())
				.Verifiable();

			return new(repositoryMock.Object, logger.Object);
		}
	}
}