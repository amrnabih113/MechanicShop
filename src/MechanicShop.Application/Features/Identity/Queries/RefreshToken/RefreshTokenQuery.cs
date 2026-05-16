using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Identity.Queries.RefreshToken;

public sealed record class RefreshTokenQuery(string RefreshToken) : IRequest<Result<TokenResponse>>;