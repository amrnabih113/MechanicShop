using System.Security.Claims;
using System.Security.Principal;
using MechanicShop.Application.common.Interfaces;
using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Identity.Queries.RefreshToken;

public class RefreshTokenQueryHandler(ILogger<RefreshTokenQueryHandler> logger,
                                      IIdentityService service,
                                      IAppDbContext context,
                                      ITokenProvider tokenProvider) :
                                      IRequestHandler<RefreshTokenQuery, Result<TokenResponse>>

{
    private readonly ILogger<RefreshTokenQueryHandler> _logger = logger;
    private readonly IIdentityService _service = service;
    private readonly IAppDbContext _context = context;      
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<Result<TokenResponse>> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var princple = _tokenProvider.GetPrincipalFromExpiredToken(request.RefreshToken);
        if (princple is null)
        {
            _logger.LogWarning("Invalid refresh token provided");
            return ApplicationErrors.ExpiredAccessTokenInvalid;
        }

        var userId = princple.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            _logger.LogWarning("User ID claim not found in expired token");
            return ApplicationErrors.UserIdClaimInvalid;
        }

        var userResult = await _service.GetUserByIdAsync(userId);
        if (userResult.IsError)
        {
            _logger.LogWarning("User with ID {UserId} not found during token refresh", userId);
            return userResult.Errors!;
        }

        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == request.RefreshToken && r.UserId == userId, cancellationToken);
        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            _logger.LogWarning("Refresh token is Expired");
            return ApplicationErrors.RefreshTokenExpired;
        }
        var newTokenResult = await _tokenProvider.GenerateJwtTokenAsync(userResult.Value, cancellationToken);
        if (newTokenResult.IsError)
        {
            _logger.LogError("Failed to generate new JWT token for user {UserId} during refresh. Errors: {Errors}", userId, string.Join(", ", newTokenResult.Errors!.Select(e => e.ToString())));
            return newTokenResult.Errors!;
        }

        return newTokenResult.Value;


    }
}