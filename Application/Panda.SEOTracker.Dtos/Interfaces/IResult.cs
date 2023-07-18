using System.Diagnostics.CodeAnalysis;

using Panda.SEOTracker.Dtos.Enums;

namespace Panda.SEOTracker.Dtos.Interfaces;

public interface IResult
{
	public ResultStatus Status { get; }
	public IEnumerable<IValidationError> Errors { get; }
}

public interface IResult<TData> : IResult
{
	[AllowNull, MaybeNull]
	TData Data { get; }
}

public interface IResultPaginated<TData> : IResult, IPaginated<TData>
{
}