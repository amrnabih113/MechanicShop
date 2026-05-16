using MediatR;

namespace MechanicShop.Application.common.Interfaces;


public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
    string[] Tags { get; }
}

public interface ICachedQuery<TRequest> : IRequest<TRequest>, ICachedQuery
{
}