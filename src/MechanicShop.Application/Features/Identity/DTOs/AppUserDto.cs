using System.Security.Claims;

namespace MechanicShop.Application.Features.Identity.DTOs;


public sealed record class AppUserDto
(string UserId, string Email, IList<string> Roles, IList<Claim> Claims
);