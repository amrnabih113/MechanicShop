using FluentValidation;

namespace MechanicShop.Application.Features.RepairTasks.Queries.GetRepairTaskById;


public class GetRepairTaskByIdValidator : AbstractValidator<GetRepairTaskByIdQuery>
{
    public GetRepairTaskByIdValidator()
    {
        RuleFor(q => q.RepairTaskId).NotEmpty().WithMessage("Repair task ID is required.");
    }
}