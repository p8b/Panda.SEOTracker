using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.DeleteTrackedUrl;
using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.Tests.TrackedUrlTests.DeleteTrackedUrlTests
{
	public class DeleteTrackedUrl_Valid_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_Delete_TrackedUrl(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ILogger<DeleteTrackedUrlCommand>> logger,
			Guid request)
		{
			var command = Setup(repositoryMock, logger);

			// Action
			var result = await command.ExecuteAsync(request, CancellationToken.None);

			// Assert
			Assert(repositoryMock, request, result);
		}

		private static void Assert(
			Mock<ITrackedUrlRepository> repositoryMock,
			Guid request,
			IResult result)
		{
			repositoryMock.Verify(x => x.Delete(
				It.Is<Guid>((x) => x == request),
				It.IsAny<CancellationToken>()
				),
				Times.Once);

			result.Status.Should().Be(ResultStatus.Succeeded);
			result.Errors.Should().BeEmpty();
		}

		private static DeleteTrackedUrlCommand Setup(
			Mock<ITrackedUrlRepository> repository,
			Mock<ILogger<DeleteTrackedUrlCommand>> logger)
		{
			//Arrange
			repository.Setup(x => x.Delete(
				It.IsAny<Guid>(),
				It.IsAny<CancellationToken>()
				))
				.Returns((Guid _, CancellationToken _) => Task.FromResult(Guid.NewGuid()))
				.Verifiable();

			return new(repository.Object, logger.Object);
		}
	}
}