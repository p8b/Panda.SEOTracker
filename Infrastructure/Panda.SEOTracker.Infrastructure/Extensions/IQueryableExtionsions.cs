using Panda.SEOTracker.Dtos;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Microsoft.EntityFrameworkCore
{
	internal static class IQueryableExtionsions
	{
		internal static async Task<IResultPaginated<T>> ToPaginatedListAsync<T>(
			this IQueryable<T> source,
			PaginatedRequest request,
			CancellationToken cancellationToken = default)
			where T : class
		{
			if (source == null)
				throw new NullReferenceException();
			var pageNumber = request.PageNumber;
			var pageSize = request.PageSize;

			pageNumber = pageNumber <= 0 ? 1 : pageNumber;
			pageSize = pageSize <= 0 ? 10 : pageSize;

			int count = await source.CountAsync(cancellationToken);
			int skip = (pageNumber - 1) * pageSize;

			return new ResultPaginated<T>
			{
				Data = await source.Skip(skip).Take(pageSize).ToListAsync(cancellationToken),
				CurrentPage = pageNumber,
				TotalPages = (int)Math.Ceiling(count / (double)pageSize),
				TotalCount = count,
				PageSize = pageSize
			};
		}
	}
}