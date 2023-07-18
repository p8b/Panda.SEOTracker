namespace Panda.SEOTracker.Dtos
{
    public class PaginatedRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class PaginatedWithSortRequest : PaginatedRequest
    {
        public string? SortBy { get; set; }
        public SortByDirection? Direction { get; set; }
    }

    public enum SortByDirection
    { None, Ascending, Descending }
}