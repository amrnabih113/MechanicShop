using MechanicShop.Application.common.Interfaces;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Identity.Queries.GenerateTokens;


public class GenerateTokensHandler(ILogger<GenerateTokensHandler> logger,
    ITokenProvider tokenProvider,
    IIdentityService identityService) : IRequestHandler<GenerateTokensQuery, Result<TokenResponse>>
{
    private readonly ILogger<GenerateTokensHandler> _logger = logger;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<TokenResponse>> Handle(GenerateTokensQuery query, CancellationToken cancellationToken)
    {
         var userResponse = await _identityService.AuthenticateAsync(query.Email, query.Password);

        if (userResponse.IsError)
        {
            return userResponse.Errors!;
        }

        var generatedToken = await _tokenProvider.GenerateJwtTokenAsync(userResponse.Value, cancellationToken);
        if (generatedToken.IsError)
        {
            _logger.LogError("Failed to generate JWT token for user {Email}. Errors: {Errors}", query.Email, string.Join(", ", generatedToken.Errors!.Select(e => e.ToString())));
            return generatedToken.Errors!;
        }

        return generatedToken.Value;   
    }
}