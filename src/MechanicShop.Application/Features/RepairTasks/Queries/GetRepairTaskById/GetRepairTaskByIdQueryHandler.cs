using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.RepairTasks.DTOs;
using MechanicShop.Application.Features.RepairTasks.Mappers;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Application.Features.RepairTasks.Queries.GetRepairTaskById;

public class GetRepairTaskByIdQueryHandler(IAppDbContext context) : 
IRequestHandler<GetRepairTaskByIdQuery, Result<RepairTaskDto>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<RepairTaskDto>> Handle(GetRepairTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var repairTask = await _context.RepairTasks
                                .Include(p => p.Parts)
                                .FirstOrDefaultAsync(p => p.Id == request.RepairTaskId, cancellationToken);

        if (repairTask is null)
        {
            return ApplicationErrors.RepairTaskNotFound;
        }                      
        
        return repairTask.ToDto();
    }
}
