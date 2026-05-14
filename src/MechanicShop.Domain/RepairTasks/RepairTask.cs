using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks.Enums;
using MechanicShop.Domain.RepairTasks.parts;


namespace MechanicShop.Domain.RepairTasks;


public sealed class RepairTask : AuditableEntity
{
    public string? Name { get; private set; }
    public decimal LaborCost { get; private set; }
    public RepairDurationInMinutes EstimatedDurationInMinutes { get; private set; }

    private List<Part> _parts = [];

    public IEnumerable<Part> Parts => _parts.AsReadOnly();

    public decimal TotalCost => LaborCost + Parts.Sum(p => p.Cost * p.Quantity);

    private RepairTask()
    {
    }

    private RepairTask(Guid id, string? name, decimal laborCost, RepairDurationInMinutes estimatedDurationInMinutes, List<Part> parts) : base(id)
    {
        Name = name;
        LaborCost = laborCost;
        EstimatedDurationInMinutes = estimatedDurationInMinutes;
        _parts = parts;

    }

    public static Result<RepairTask> Create(Guid id, string? name, decimal laborCost, RepairDurationInMinutes estimatedDurationInMinutes, List<Part> parts)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return RepairTaskErrors.NameRequired;
        }
        if (laborCost < 0)
        {
            return RepairTaskErrors.LaborCostInvalid;
        }
        if (parts == null)
        {
            return RepairTaskErrors.PartsRequired;
        }
        if (laborCost <= 0)
        {
            return RepairTaskErrors.LaborCostInvalid;
        }

        if (!Enum.IsDefined(estimatedDurationInMinutes))
        {
            return RepairTaskErrors.DurationInvalid;
        }

        return new RepairTask(id, name, laborCost, estimatedDurationInMinutes, parts);


    }
      public Result<Updated> UpsertParts(List<Part> incomingParts)
    {
        _parts.RemoveAll(existing => incomingParts.All(p => p.Id != existing.Id));

        foreach (var incoming in incomingParts)
        {
            var existing = _parts.FirstOrDefault(p => p.Id == incoming.Id);
            if (existing is null)
            {
                _parts.Add(incoming);
            }
            else
            {
                var updatePartResult = existing.Update(incoming.Name, incoming.Cost, incoming.Quantity);
                if (updatePartResult.IsError)
                {
                    return updatePartResult.Errors!;
                }
            }
        }

        return Result.Updated;
    }

}
