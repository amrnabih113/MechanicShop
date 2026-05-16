using MechanicShop.Application.Features.Identity.DTOs;
using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Application.common.Interfaces;


public interface IIdentityService
{
    Task<bool> IsInRoleAsync(Guid userId, string role);

    Task<bool> AuthorizeAsync(Guid userId, string? policyName);

    Task<Result<AppUserDto>> AuthenticateAsync(string email, string password);

    Task<Result<AppUserDto>> GetUserByIdAsync(string userId);

    Task<string?> GetUserNameAsync(string userId);
}