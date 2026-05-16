using FluentValidation;

namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask.CreateParts;

public class CreatePartCommandValidator : AbstractValidator<CreatePartCommand>
{
    public CreatePartCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Part name is required.");
        RuleFor(p => p.Cost).GreaterThan(0).WithMessage("Part cost must be greater than zero.");
        RuleFor(p => p.Quantity).GreaterThan(0).WithMessage("Part quantity must be greater than zero.");
    }
}