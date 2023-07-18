using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.DeleteTrackedUrl;
using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic.Tests.TrackedUrlTests.DeleteTrackedUrlTests
{
	public class DeleteTrackedUrl_InternalError_Tests
	{
		[Theory]
		[AutoMockData]
		public async Task Should_Fail_To_Delete_TrackedUrl_When_Repository_Throws_Exception(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ILogger<DeleteTrackedUrlCommand>> logger,
			Guid request)
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
			Guid request,
			IResult result)
		{
			repositoryMock.Verify(x => x.Delete(
				It.Is<Guid>((x) => x == request),
				It.IsAny<CancellationToken>()
				),
				Times.Once);

			result.Status.Should().Be(ResultStatus.InternalError);
			result.Errors.Should().OnlyHaveUniqueItems(x => x.Message == AppMessages.UnableToDelete("Id"));
		}

		private static DeleteTrackedUrlCommand Setup(
			Mock<ITrackedUrlRepository> repositoryMock,
			Mock<ILogger<DeleteTrackedUrlCommand>> logger)
		{
			//Arrange
			repositoryMock.Setup(x => x.Delete(
				It.IsAny<Guid>(),
				It.IsAny<CancellationToken>()
				))
				.Throws(new Exception())
				.Verifiable();

			return new(repositoryMock.Object, logger.Object);
		}
	}
}