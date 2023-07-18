namespace Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl;

public class CreateTrackedUrlDto
{
	public string Url { get; set; } = string.Empty;
	public int TotalResultsToCheck { get; set; }
	public List<string> SearchTerms { get; set; } = new();
}