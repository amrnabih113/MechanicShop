using MechanicShop.Application.common.Interfaces;
using MechanicShop.Application.Features.RepairTasks.DTOs;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.RepairTasks.Queries.GetRepairTasks;

public sealed record GetRepairTasksQuery : ICachedQuery<Result<List<RepairTaskDto>>>
{
     public string CacheKey => "repair-tasks";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["repair-tasks"];

    TimeSpan? ICachedQuery.Expiration => Expiration;
}