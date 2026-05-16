using MediatR;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.common.Behaviours;



public class UnhandeledExceptionBehaviour<TRequest, TResponse>(
    ILogger<UnhandeledExceptionBehaviour<TRequest, TResponse>> logger) :
IPipelineBehavior<TRequest, TResponse>
where TRequest : notnull
{
    private readonly ILogger<UnhandeledExceptionBehaviour<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "An unhandled exception occurred while processing request of type {RequestType}", requestName);
            throw;
        }
    }
}