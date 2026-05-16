namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask.CreateParts;

using MechanicShop.Domain.Common.Results;
using MediatR;

public sealed record class CreatePartCommand(string Name, decimal Cost, int Quantity) : IRequest<Result<Guid>>;