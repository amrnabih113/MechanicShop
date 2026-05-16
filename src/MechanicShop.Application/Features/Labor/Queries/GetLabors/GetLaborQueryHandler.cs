using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Labor.DTOs;
using MechanicShop.Application.Features.Labor.Mappers;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Labor.Queries.GetLabors;


public class GetLaborsQueryHandler(IAppDbContext context) : IRequestHandler<GetLaborsQuery, Result<List<LaborDto>>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<List<LaborDto>>> Handle(GetLaborsQuery request, CancellationToken cancellationToken)
    {
        var labors = await _context.Employees.AsNoTracking().ToListAsync(cancellationToken);
        var laborDtos = labors.ToDtos();
        return laborDtos;
    }
}