using System.Text.RegularExpressions;

using Microsoft.Extensions.Logging;

using Panda.SEOTracker.BusinessLogic.Services;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.Core.Enums;
using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.Infrastructure.Services
{
	public partial class SearchEngineService : ISearchEngineService
	{
		private readonly ITrackedUrlRepository _repository;
		private readonly ILogger<SearchEngineService> _logger;

		public SearchEngineService(
			ITrackedUrlRepository repository,
			ILogger<SearchEngineService> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		public async Task<IEnumerable<SearchTermHistoryEntity>> GetSearchTermHistoriesAsync(
			TrackedUrlEntity trackUrl,
			CancellationToken cancellationToken = default)
		{
			if (trackUrl is null) return Array.Empty<SearchTermHistoryEntity>();

			var result = new List<SearchTermHistoryEntity>();
			foreach (var term in trackUrl.SearchTerms)
			{
				var googleResult = await GetTrackingInfo(SearchEngines.Google, trackUrl, term, cancellationToken);
				if (googleResult is not null)
					result.Add(googleResult);
			}

			return result;
		}

		private async Task<SearchTermHistoryEntity?> GetTrackingInfo(
			SearchEngines searchEngine,
			TrackedUrlEntity trackedUrl,
			SearchTermEntity searchTerm,
			CancellationToken cancellationToken = default)
		{
			using var httpClient = new HttpClient();
			var searchUrl = searchEngine switch
			{
				SearchEngines.Google => $"https://www.google.com/search?q={Uri.EscapeDataString(searchTerm.Term)}&num={trackedUrl.TotalResultsToCheck}",
				_ => throw new ArgumentOutOfRangeException("Search Engine", "Invalid Search Engine.")
			};
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, searchUrl);
				request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");
				request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

				var response = await httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				var htmlContent = await response.Content.ReadAsStringAsync(cancellationToken);
				var matches = MatchRegex().Matches(htmlContent);

				var searchHistory = new SearchTermHistoryEntity
				{
					Id = Guid.Empty,
					Date = DateTime.UtcNow,
					SearchEngineUsed = SearchEngines.Google,
					SearchTermId = searchTerm.Id
				};
				for (var i = 0; i < matches.Count; i++)
				{
					var match = matches[i];
					var url = match.Groups[1].Value;

					// Website found in search results
					if (url.Contains(trackedUrl.Url))
					{
						searchHistory.Positions.Add(i + 1);
					}
				}
				return searchHistory;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to search google for terms");
				return null;
			}
		}

		[GeneratedRegex(@"<cite.*?>(.*?)<span|<cite.*?>(.*?)<cite", RegexOptions.IgnoreCase)]
		private static partial Regex MatchRegex();
	}
}