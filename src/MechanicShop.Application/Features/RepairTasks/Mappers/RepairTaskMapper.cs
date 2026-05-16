using MechanicShop.Application.Features.RepairTasks.DTOs;
using MechanicShop.Domain.RepairTasks;
using MechanicShop.Domain.RepairTasks.parts;

namespace MechanicShop.Application.Features.RepairTasks.Mappers;

public static class RepairTaskMapper
{
    public static RepairTaskDto ToDto(this RepairTask repairTask) => new RepairTaskDto
    {
        RepairTaskId = repairTask.Id,
        Name = repairTask.Name,
        DurationInMinutes = repairTask.EstimatedDurationInMinutes,
        LaborCost = repairTask.LaborCost,
        TotalCost = repairTask.TotalCost,
        Parts = repairTask.Parts.ToDtos()
    };

    public static List<RepairTaskDto> ToDtos(this IEnumerable<RepairTask> repairTasks) 
    => [.. repairTasks.Select(rt => rt.ToDto())];

    public static PartDto ToDto(this Part part) => new PartDto
    {
        PartId = part.Id,
        Name = part.Name,
        Cost = part.Cost,
        Quantity = part.Quantity
    };

    public static List<PartDto> ToDtos(this IEnumerable<Part> parts) 
    => [.. parts.Select(p => p.ToDto())];
}