namespace Panda.SEOTracker.Dtos.Interfaces;

public interface IPaginated<TData>
{
	IEnumerable<TData> Data { get; }
	int CurrentPage { get; }
	int TotalPages { get; }
	int TotalCount { get; }
	int PageSize { get; }
}