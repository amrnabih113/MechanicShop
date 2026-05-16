using MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask.CreateParts;
using MechanicShop.Application.Features.RepairTasks.DTOs;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks.Enums;
using MediatR;

namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public sealed record class CreateRepairTaskCommand(string Name, decimal LaborCost, RepairDurationInMinutes DurationInMinutes, List<CreatePartCommand> Parts) : IRequest<Result<RepairTaskDto>>;
