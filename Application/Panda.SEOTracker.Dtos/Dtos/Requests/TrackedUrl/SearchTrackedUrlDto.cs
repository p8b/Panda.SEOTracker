namespace Panda.SEOTracker.Dtos.Dtos.Requests.TrackedUrl
{
    public class SearchTrackedUrlDto : PaginatedWithSortRequest
    {
        public string SearchValue { get; set; } = string.Empty;
    }
}