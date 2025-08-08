namespace Application.Common.Messaging;

using ErrorOr;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<ErrorOr<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}