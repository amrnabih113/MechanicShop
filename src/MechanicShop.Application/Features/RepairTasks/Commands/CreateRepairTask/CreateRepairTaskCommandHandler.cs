using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.RepairTasks.DTOs;
using MechanicShop.Application.Features.RepairTasks.Mappers;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks;
using MechanicShop.Domain.RepairTasks.parts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public class CreateRepairTaskCommandHandler(ILogger<CreateRepairTaskCommandHandler> logger,
                                            IAppDbContext context,
                                            MemoryCache cache) : IRequestHandler<CreateRepairTaskCommand, Result<RepairTaskDto>>
{
    private readonly ILogger<CreateRepairTaskCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly MemoryCache _cache = cache;

    public async Task<Result<RepairTaskDto>> Handle(CreateRepairTaskCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await _context.RepairTasks.AnyAsync(rt => rt.Name == request.Name);

        if (nameExists)
        {
            _logger.LogWarning("Repair task with name {RepairTaskName} already exists.", request.Name);
            return RepairTaskErrors.DuplicateName;
        }
        var ValidatedParts = new List<Part>();
        foreach (var p in request.Parts)
        {
            var partId = Guid.NewGuid();
            var part = Part.Create(partId, p.Name, p.Cost, p.Quantity);
            if (part.IsError)
            {
                _logger.LogWarning("Invalid part data for part with id {PartId}. Errors: {Errors}", partId, part.Errors);
                return part.Errors!;
            }
            ValidatedParts.Add(part.Value!);
        }
        var repairTask = RepairTask.Create(Guid.NewGuid(), request.Name, request.LaborCost, request.DurationInMinutes, ValidatedParts);
        if (repairTask.IsError)
        {
            _logger.LogWarning("Invalid repair task data. Errors: {Errors}", repairTask.Errors);
            return repairTask.Errors!;
        }
        await _context.RepairTasks.AddAsync(repairTask.Value!);
        await _context.SaveChangesAsync(cancellationToken);
        return repairTask.Value!.ToDto();
    }
}