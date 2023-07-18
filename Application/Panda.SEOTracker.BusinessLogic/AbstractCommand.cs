namespace Panda.SEOTracker.BusinessLogic;

public abstract class AbstractCommand<TResult>
{
    public abstract Task<TResult> ExecuteAsync(CancellationToken cancellationToken = default);
}

public abstract class AbstractCommand<TResult, TRequest>
{
    public abstract Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}