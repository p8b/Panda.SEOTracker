using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.CreateTrackedUrl;
using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;
using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.Tests.TrackedUrlTests.CreateTrackedUrlTests
{
	public class CreateTrackedUrl_InternalError_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_Create_TrackedUrl_When_Repository_Throws_Exception(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ILogger<CreateTrackedUrlCommand>> logger,
			CreateTrackedUrlDto request)
		{
			// Arrange
			var command = Setup(
				repositoryMock,
				logger);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(repositoryMock, request, result);
		}

		private static void Assert(
			Mock<ITrackedUrlRepository> repositoryMock,
			CreateTrackedUrlDto request,
			IResult<Guid> result)
		{
			repositoryMock.Verify(x => x.Create(
				It.Is<CreateTrackedUrlDto>((x) =>
					x.Url == request.Url &&
					x.SearchTerms == request.SearchTerms &&
					x.TotalResultsToCheck == request.TotalResultsToCheck),
				It.IsAny<CancellationToken>()
				),
				Times.Once);

			result.Status.Should().Be(ResultStatus.InternalError);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.Message == AppMessages.UnableToCreate("TrackedUrl"));
			result.Data.Should().Be(Guid.Empty);
		}

		private static CreateTrackedUrlCommand Setup(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ILogger<CreateTrackedUrlCommand>> logger)
		{
			//Arrange
			repositoryMock.Setup(x => x.Create(
				It.IsAny<CreateTrackedUrlDto>(),
				It.IsAny<CancellationToken>()
				))
				.Throws(new Exception())
				.Verifiable();

			return new(repositoryMock.Object, logger.Object);
		}
	}
}