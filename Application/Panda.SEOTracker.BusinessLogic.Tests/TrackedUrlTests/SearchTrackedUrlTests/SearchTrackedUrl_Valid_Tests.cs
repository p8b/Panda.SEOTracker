using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.SearchTrackedUrl;
using Panda.SEOTracker.Dtos;
using Panda.SEOTracker.Dtos.Dtos;
using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;
using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;
using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.BusinessLogic.Tests.TrackedUrlTests.SearchTrackedUrlTests
{
	public class SearchTrackedUrl_Valid_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_SearchTrackedUrl(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ILogger<SearchTrackedUrlCommand>> logger,
			SearchTrackedUrlDto request)
		{
			// Arrange
			var command = Setup(
				repositoryMock,
				logger);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(repositoryMock,
				request,
				result);
		}

		private static void Assert(
			Mock<ITrackedUrlRepository> repositoryMock,
			SearchTrackedUrlDto request,
			IResultPaginated<TrackedUrlDto> result)
		{
			repositoryMock.Verify(x => x.Search(
				It.Is<SearchTrackedUrlDto>((x) =>
					x.SearchValue == request.SearchValue &&
					x.SortBy == request.SortBy &&
					x.Direction == request.Direction &&
					x.PageNumber == request.PageNumber &&
					x.PageSize == request.PageSize),
				It.IsAny<CancellationToken>()
				),
				Times.Once);

			result.Status.Should().Be(ResultStatus.Succeeded);
			result.Errors.Should().BeEmpty();
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
				.Returns((SearchTrackedUrlDto request, CancellationToken _) =>
				{
					Guid id = Guid.NewGuid();

					return Task.FromResult<IResultPaginated<TrackedUrlEntity>>(new ResultPaginated<TrackedUrlEntity>()
					{
						Data = new List<TrackedUrlEntity>()
						{
							new TrackedUrlEntity
							{
								Id = id,
								Url = GeneratorUtility.GetString(1),
								TotalResultsToCheck = 100,
								SearchTerms = new List<SearchTermEntity>
								{
									new SearchTermEntity()
									{
										Id = Guid.NewGuid(),
										Term = GeneratorUtility.GetString(1),
										History = new List<SearchTermHistoryEntity>(),
										TrackedUrlId = id,
									}
								}
							}
						},
						CurrentPage = request.PageNumber,
						TotalPages = 1,
						TotalCount = 1,
						PageSize = request.PageSize,
					});
				}).Verifiable();

			return new(repositoryMock.Object, logger.Object);
		}
	}
}