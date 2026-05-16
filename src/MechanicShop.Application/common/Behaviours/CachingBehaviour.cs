using MechanicShop.Application.common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.common.Behaviours;


public class CachingBehaviour<TRequest, TResponse>(
    HybridCache cache,
    ILogger<CachingBehaviour<TRequest, TResponse>> logger)
: IPipelineBehavior<TRequest, TResponse> 
where TRequest : notnull

{
    private readonly HybridCache _cache = cache;
    private readonly ILogger<CachingBehaviour<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request,
                                         RequestHandlerDelegate<TResponse> next,
                                         CancellationToken cancellationToken)
    {
        if (request is not ICachedQuery cachedQuery)
        {
            return await next(cancellationToken);
        }
        _logger.LogInformation("Checking cache for key {CacheKey}", cachedQuery.CacheKey);

        var result = await _cache.GetOrCreateAsync(key: cachedQuery.CacheKey,
        factory: async ct =>
        {
            var innerResult = await next(ct);
            if (innerResult is IResult r && r.IsSuccess)
            {
                return innerResult;
            }
            return default!;
        },
        options: new HybridCacheEntryOptions
        {
            Expiration = cachedQuery.Expiration,
        },
        tags: cachedQuery.Tags,
        cancellationToken: cancellationToken
        );
        if (result is not null)
        {
            _logger.LogInformation("Cache hit for key {CacheKey}", cachedQuery.CacheKey);
            return result;
        }
        _logger.LogInformation("Cache miss for key {CacheKey}", cachedQuery.CacheKey);
        return await next(cancellationToken);
       }
}
