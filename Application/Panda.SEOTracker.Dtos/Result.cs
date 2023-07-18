using Panda.SEOTracker.Dtos.Enums;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.Dtos;

public class Result : IResult
{
	public IEnumerable<IValidationError> Errors { get; protected set; } = new List<IValidationError>();
	public ResultStatus Status { get; protected set; } = ResultStatus.None;

	public static IResult Created()
		=> new Result
		{
			Status = ResultStatus.Created
		};

	public static IResult Succeeded()
		=> new Result
		{
			Status = ResultStatus.Succeeded
		};

	public static IResult ValidationFailed(params IValidationError[] errors)
		=> new Result()
		{
			Status = ResultStatus.ValidationFailed,
			Errors = errors,
		};

	public static IResult InternalError(string message)
		=> new Result()
		{
			Status = ResultStatus.InternalError,
			Errors = GetAsErrorList(message),
		};

	public static IResult Unauthorised(string message)
		=> new Result()
		{
			Status = ResultStatus.Unauthorised,
			Errors = GetAsErrorList(message),
		};

	protected static List<IValidationError> GetAsErrorList(string message)
		=> new() { new ValidationError
			{
				PropertyName = string.Empty,
				Message = message,
			}
		};
}

public class Result<TData> : Result, IResult<TData>
{
	public TData? Data { get; set; }

	public static IResult<TData> Created(TData data)
		=> new Result<TData>
		{
			Status = ResultStatus.Created,
			Data = data,
		};

	public static IResult<TData> Succeeded(TData data)
		=> new Result<TData>
		{
			Status = ResultStatus.Succeeded,
			Data = data,
		};

	public new static IResult<TData> ValidationFailed(params IValidationError[] errors)
		=> new Result<TData>()
		{
			Status = ResultStatus.ValidationFailed,
			Errors = errors,
		};

	public new static IResult<TData> InternalError(string message)
		=> new Result<TData>()
		{
			Status = ResultStatus.InternalError,
			Errors = GetAsErrorList(message),
		};

	public new static IResult<TData> Unauthorised(string message)
		=> new Result<TData>()
		{
			Status = ResultStatus.Unauthorised,
			Errors = GetAsErrorList(message),
		};
}

public class ResultPaginated<TData> : Result, IResultPaginated<TData>
{
	public IEnumerable<TData> Data { get; set; } = Enumerable.Empty<TData>();
	public int CurrentPage { get; set; }
	public int TotalPages { get; set; }
	public int TotalCount { get; set; }
	public int PageSize { get; set; }

	public static IResultPaginated<TData> Succeeded<TEntity>(
		IPaginated<TEntity> paginatedData,
		Func<TEntity, TData> mapData)
		=> Succeeded(
			paginatedData.Data.Select(mapData),
			paginatedData.CurrentPage,
			paginatedData.TotalPages,
			paginatedData.TotalCount,
			paginatedData.PageSize);

	public static IResultPaginated<TData> Succeeded(
		IEnumerable<TData> data,
		int currentPage,
		int totalPages,
		int totalCount,
		int pageSize)
		=> new ResultPaginated<TData>()
		{
			Status = ResultStatus.Succeeded,
			Data = data,
			CurrentPage = currentPage,
			TotalPages = totalPages,
			TotalCount = totalCount,
			PageSize = pageSize,
		};

	public new static IResultPaginated<TData> ValidationFailed(params IValidationError[] errors)
		=> new ResultPaginated<TData>()
		{
			Status = ResultStatus.ValidationFailed,
			Errors = errors,
		};

	public new static IResultPaginated<TData> InternalError(string message)
		=> new ResultPaginated<TData>()
		{
			Status = ResultStatus.InternalError,
			Errors = GetAsErrorList(message),
		};

	public new static IResultPaginated<TData> Unauthorised(string message)
		=> new ResultPaginated<TData>()
		{
			Status = ResultStatus.Unauthorised,
			Errors = GetAsErrorList(message),
		};
}