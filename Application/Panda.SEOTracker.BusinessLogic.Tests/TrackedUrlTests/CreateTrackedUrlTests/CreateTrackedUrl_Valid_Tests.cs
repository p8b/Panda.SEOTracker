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
	public class CreateTrackedUrl_Valid_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_Create_TrackedUrl(
			Mock<ITrackedUrlRepository> categoryRepositoryMock,
			Mock<ILogger<CreateTrackedUrlCommand>> logger,
			CreateTrackedUrlDto request)
		{
			var command = Setup(categoryRepositoryMock, logger);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(categoryRepositoryMock, request, result);
		}

		[Theory]
		[AutoMockData]
		public async Task Should_Create_TrackedUrl_With_Max_Length_Url(
			Mock<ITrackedUrlRepository> categoryRepositoryMock,
			Mock<ILogger<CreateTrackedUrlCommand>> logger,
			CreateTrackedUrlDto request)
		{
			var command = Setup(categoryRepositoryMock, logger);

			request.Url = GeneratorUtility.GetString(256);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(categoryRepositoryMock, request, result);
		}

		[Theory]
		[AutoMockData]
		public async Task Should_Create_TrackedUrl_With_Min_Length_Url(
			Mock<ITrackedUrlRepository> categoryRepositoryMock,
			Mock<ILogger<CreateTrackedUrlCommand>> logger,
			CreateTrackedUrlDto request)
		{
			var command = Setup(categoryRepositoryMock, logger);

			request.Url = GeneratorUtility.GetString(1);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(categoryRepositoryMock, request, result);
		}

		private static void Assert(
			Mock<ITrackedUrlRepository> categoryRepositoryMock,
			CreateTrackedUrlDto request,
			IResult<Guid> result)
		{
			categoryRepositoryMock.Verify(x => x.Create(
				It.Is<CreateTrackedUrlDto>((x) =>
					x.Url == request.Url &&
					x.SearchTerms == request.SearchTerms &&
					x.TotalResultsToCheck == request.TotalResultsToCheck),
				It.IsAny<CancellationToken>()
				),
				Times.Once);

			result.Status.Should().Be(ResultStatus.Created);
			result.Errors.Should().BeEmpty();
			result.Data.Should().NotBe(Guid.Empty);
		}

		private static CreateTrackedUrlCommand Setup(
			Mock<ITrackedUrlRepository> repository,
			Mock<ILogger<CreateTrackedUrlCommand>> logger)
		{
			//Arrange
			repository.Setup(x => x.Create(
				It.IsAny<CreateTrackedUrlDto>(),
				It.IsAny<CancellationToken>()
				))
				.Returns((CreateTrackedUrlDto _, CancellationToken _) => Task.FromResult(Guid.NewGuid()))
				.Verifiable();

			return new(repository.Object, logger.Object);
		}
	}
}