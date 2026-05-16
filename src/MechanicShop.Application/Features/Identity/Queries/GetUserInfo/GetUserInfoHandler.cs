using MechanicShop.Application.common.Interfaces;
using MechanicShop.Application.Features.Identity.DTOs;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Identity.Queries.GetUserInfo;

public class GetUserInfoHandler(
    ILogger<GetUserInfoHandler> logger,
    IIdentityService identityService
) : IRequestHandler<GetUserInfoQuery, Result<AppUserDto>>
{
    private readonly ILogger<GetUserInfoHandler> _logger = logger;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<AppUserDto>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userResponse = await _identityService.GetUserByIdAsync(request.UserId!);
        if (userResponse.IsError)
        {
            _logger.LogError("Failed to retrieve user info for user ID {UserId}. Errors: {Errors}", request.UserId, string.Join(", ", userResponse.Errors!.Select(e => e.ToString())));
            return userResponse.Errors!;
        }

        return userResponse.Value;
    }
}