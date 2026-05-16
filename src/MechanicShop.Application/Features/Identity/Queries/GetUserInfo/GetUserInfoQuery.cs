using MechanicShop.Application.Features.Identity.DTOs;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Identity.Queries.GetUserInfo;

public sealed record class GetUserInfoQuery(string? UserId) : IRequest<Result<AppUserDto>>;