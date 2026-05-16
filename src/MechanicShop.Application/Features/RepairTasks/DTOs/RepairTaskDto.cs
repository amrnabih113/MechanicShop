using MechanicShop.Domain.RepairTasks.Enums;
using MechanicShop.Domain.RepairTasks.parts;

namespace MechanicShop.Application.Features.RepairTasks.DTOs;

public class RepairTaskDto
{
    public Guid RepairTaskId { get;set; }

    public string? Name { get;set; }

    public RepairDurationInMinutes DurationInMinutes { get;set; }
    public decimal LaborCost { get;set; }

    public decimal TotalCost { get;set; }

    public List<PartDto> Parts { get;set; } = [];


}