namespace Application.Common.Messaging;

using ErrorOr;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<ErrorOr<TResult>> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}