using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Identity.Queries.GenerateTokens;

public record class GenerateTokensQuery(string Email, string Password)
 : IRequest<Result<TokenResponse>>;