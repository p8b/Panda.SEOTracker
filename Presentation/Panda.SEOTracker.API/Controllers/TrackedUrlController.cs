using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;

using Panda.SEOTracker.API.Extensions;
using Panda.SEOTracker.BusinessLogic.SearchTermHistoryLogic;
using Panda.SEOTracker.BusinessLogic.Services;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.CreateTrackedUrl;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.DeleteTrackedUrl;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.GetLatestTrackingInformation;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic.SearchTrackedUrl;
using Panda.SEOTracker.Dtos;
using Panda.SEOTracker.Dtos.Dtos;
using Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;

namespace Panda.SEOTracker.API.Controllers
{
	[Route("TrackedUrl")]
	public class TrackedUrlController
	{
		/// <summary>
		///    Create a new Tracked Url.
		/// </summary>
		[HttpPost]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
		public Task<IActionResult> CreateAsync(
			[FromServices] ITrackedUrlRepository repository,
			[FromServices] ILogger<CreateTrackedUrlCommand> logger,
			[FromBody] CreateTrackedUrlDto request,
			CancellationToken cancellationToken)
			=> new CreateTrackedUrlCommand(repository, logger)
				.ExecuteAsync(request, cancellationToken)
				.ToActionResult();

		/// <summary>
		///    Delete the given Tracked Url.
		/// </summary>
		[HttpDelete]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
		public Task<IActionResult> DeleteAsync(
			[FromServices] ITrackedUrlRepository repository,
			[FromServices] ILogger<DeleteTrackedUrlCommand> logger,
			[FromQuery] Guid request,
			CancellationToken cancellationToken)
			=> new DeleteTrackedUrlCommand(repository, logger)
				.ExecuteAsync(request, cancellationToken)
				.ToActionResult();

		/// <summary>
		///    Get Latest Tracking Information.
		/// </summary>
		[HttpGet("GetLatestTrackingInformation")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(TrackedUrlDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
		public Task<IActionResult> GetLatestTrackingInformationAsync(
			[FromServices] ITrackedUrlRepository repository,
			[FromServices] ISearchTermHistoryRepository searchTermsHistoryRepository,
			[FromServices] ISearchEngineService searchEngineService,
		[FromServices] ILogger<GetLatestTrackingInformationCommand> logger,
			[FromQuery] Guid request,
		CancellationToken cancellationToken)
			=> new GetLatestTrackingInformationCommand(
				repository,
				searchTermsHistoryRepository,
				searchEngineService,
				logger)
				.ExecuteAsync(request, cancellationToken)
				.ToActionResult();

		/// <summary>
		///    Search Tracked Urls.
		/// </summary>
		[HttpGet("SearchTrackedUrls")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(ResultPaginated<TrackedUrlDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
		public Task<IActionResult> SearchTrackedUrlsAsync(
			[FromServices] ITrackedUrlRepository repository,
		[FromServices] ILogger<SearchTrackedUrlCommand> logger,
			[FromQuery] SearchTrackedUrlDto request,
			CancellationToken cancellationToken)
			=> new SearchTrackedUrlCommand(repository, logger)
				.ExecuteAsync(request, cancellationToken)
				.ToActionResult();
	}
}