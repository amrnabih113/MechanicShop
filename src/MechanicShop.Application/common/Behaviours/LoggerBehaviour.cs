using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.common.Behaviours;

public class LoggerBehaviour<TRequest>(ILogger<TRequest> logger) 
: IRequestPreProcessor<TRequest>
where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger = logger;

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Handling request of type {RequestType} with content: {@Request}", requestName, request);
        return Task.CompletedTask;
    }
}