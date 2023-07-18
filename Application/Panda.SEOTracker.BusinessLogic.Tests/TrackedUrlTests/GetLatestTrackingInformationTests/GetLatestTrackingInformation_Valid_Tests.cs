using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Panda.SEOTracker.BusinessLogic.SearchTermHistoryLogic;
using Panda.SEOTracker.BusinessLogic.Services;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.GetLatestTrackingInformation;
using Panda.SEOTracker.Core.Enums;
using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;
using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.BusinessLogic.Tests.TrackedUrlTests.GetLatestTrackingInformationTests
{
	public class GetLatestTrackingInformation_Valid_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_GetLatestTrackingInformation(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ISearchTermHistoryRepository> searchTermsHistoryRepositoryMock,
			Mock<ISearchEngineService> searchEngineServiceMock,
			Mock<ILogger<GetLatestTrackingInformationCommand>> logger)
		{
			// Arrange

			Guid request = Guid.NewGuid();
			var command = Setup(
				repositoryMock,
				searchTermsHistoryRepositoryMock,
				searchEngineServiceMock,
				logger);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(repositoryMock,
				searchTermsHistoryRepositoryMock,
				searchEngineServiceMock,
				request,
				result);
		}

		private static void Assert(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ISearchTermHistoryRepository> searchTermsHistoryRepositoryMock,
			Mock<ISearchEngineService> searchEngineServiceMock,
			Guid request,
			IResult result)
		{
			repositoryMock.Verify(x => x.Get(
				It.Is<Guid>((x) => x == request),
				It.IsAny<CancellationToken>()
				),
				Times.AtMost(2));

			searchEngineServiceMock.Verify(x => x.GetSearchTermHistoriesAsync(
				It.IsAny<TrackedUrlEntity>(),
				It.IsAny<CancellationToken>()
				),
				Times.AtMostOnce);

			searchTermsHistoryRepositoryMock.Verify(x => x.AddRange(
				It.IsAny<IEnumerable<SearchTermHistoryEntity>>(),
				It.IsAny<CancellationToken>()
				),
				Times.AtMostOnce);

			result.Status.Should().Be(ResultStatus.Succeeded);
			result.Errors.Should().BeEmpty();
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
				.Returns((Guid id, CancellationToken _) =>
				{
					Guid searchTermId = Guid.NewGuid();
					return Task.FromResult<TrackedUrlEntity?>(new TrackedUrlEntity
					{
						Id = id,
						Url = GeneratorUtility.GetString(1),
						TotalResultsToCheck = 100,
						SearchTerms = new List<SearchTermEntity>
						{
							new SearchTermEntity()
							{
								Id = searchTermId,
								Term = GeneratorUtility.GetString(1),
								History = new List<SearchTermHistoryEntity>()
								{
									new SearchTermHistoryEntity
									{
										Id = Guid.NewGuid() ,
										Date = DateTime.UtcNow.AddDays(-10),
										SearchEngineUsed = SearchEngines.Google,
										Positions = new List<int>
										{
											1,2
										},
										SearchTermId =searchTermId
									}
								},
								TrackedUrlId = id,
							}
						}
					});
				}).Verifiable();

			searchEngineServiceMock.Setup(x => x.GetSearchTermHistoriesAsync(
				It.IsAny<TrackedUrlEntity>(),
				It.IsAny<CancellationToken>()
				))
				.Returns((TrackedUrlEntity trackUrl, CancellationToken _) =>
				{
					return Task.FromResult<IEnumerable<SearchTermHistoryEntity>>(new List<SearchTermHistoryEntity>
					{
						new SearchTermHistoryEntity
						{
							Id = Guid.NewGuid(),
							Date = DateTime.UtcNow.AddDays(-2),
							SearchEngineUsed = SearchEngines.Google,
							Positions = new List<int>
							{
								2,4
							},
							SearchTermId =trackUrl.SearchTerms.FirstOrDefault()!.Id
						}
					});
				}).Verifiable();

			searchTermsHistoryRepositoryMock.Setup(x => x.AddRange(
				It.IsAny<IEnumerable<SearchTermHistoryEntity>>(),
				It.IsAny<CancellationToken>()
				))
				.Returns(() => Task.CompletedTask)
				.Verifiable();

			return new(repositoryMock.Object, searchTermsHistoryRepositoryMock.Object, searchEngineServiceMock.Object, logger.Object);
		}
	}
}