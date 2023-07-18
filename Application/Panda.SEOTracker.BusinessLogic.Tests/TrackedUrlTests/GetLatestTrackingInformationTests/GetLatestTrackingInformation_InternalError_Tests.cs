using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Panda.SEOTracker.BusinessLogic.SearchTermHistoryLogic;
using Panda.SEOTracker.BusinessLogic.Services;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.GetLatestTrackingInformation;
using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.Tests.TrackedUrlTests.GetLatestTrackingInformationTests
{
	public class GetLatestTrackingInformation_InternalError_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_GetLatestTrackingInformation_When_Repository_Throws_Exception(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ISearchTermHistoryRepository> searchTermsHistoryRepositoryMock,
			Mock<ISearchEngineService> searchEngineServiceMock,
			Mock<ILogger<GetLatestTrackingInformationCommand>> logger,
			Guid request)
		{
			// Arrange
			var command = Setup(
				repositoryMock,
				searchTermsHistoryRepositoryMock,
				searchEngineServiceMock,
				logger);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(result);
		}

		private static void Assert(
			IResult result)
		{
			result.Status.Should().Be(ResultStatus.InternalError);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.Message == AppMessages.UnableToDelete("Id"));
		}

		private static GetLatestTrackingInformationCommand Setup(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ISearchTermHistoryRepository> searchTermsHistoryRepositoryMock,
			Mock<ISearchEngineService> searchEngineServiceMock,
			Mock<ILogger<GetLatestTrackingInformationCommand>> logger)
		{
			//Arrange
			repositoryMock.Setup(x => x.Get(
				It.IsAny<Guid>(),
				It.IsAny<CancellationToken>()
				))
				.Throws(new Exception())
				.Verifiable();

			return new(repositoryMock.Object, searchTermsHistoryRepositoryMock.Object, searchEngineServiceMock.Object, logger.Object);
		}
	}
}