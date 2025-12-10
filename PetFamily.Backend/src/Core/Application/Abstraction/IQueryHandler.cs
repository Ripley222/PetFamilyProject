using CSharpFunctionalExtensions;
using SharedKernel;

namespace Application.Abstraction;

public interface IQuery;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    Task<Result<TResponse, ErrorList>> Handle(TQuery query, CancellationToken cancellationToken = default);
}

public interface IQueryHandler<in TQuery> where TQuery : IQuery
{
    Task<UnitResult<ErrorList>> Handle(TQuery command, CancellationToken cancellationToken = default);
}