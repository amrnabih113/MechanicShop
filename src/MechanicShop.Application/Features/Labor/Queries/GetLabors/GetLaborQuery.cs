using MechanicShop.Application.Features.Labor.DTOs;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Labor.Queries.GetLabors;

public sealed record GetLaborsQuery : IRequest<Result<List<LaborDto>>>;