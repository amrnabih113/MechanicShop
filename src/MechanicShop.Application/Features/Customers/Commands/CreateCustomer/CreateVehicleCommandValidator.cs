using FluentValidation;

namespace MechanicShop.Application.Features.Customers.Commands.CreateCustomer;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.Make)
            .NotEmpty().MaximumLength(50)
            .WithMessage("Make is required.");

        RuleFor(x => x.Model)
            .NotEmpty().MaximumLength(50)
            .WithMessage("Model is required.");

        RuleFor(x => x.Year)
            .InclusiveBetween(1886, DateTime.Now.Year + 1)
            .WithMessage($"Year must be between 1886 and {DateTime.Now.Year + 1}.");

        RuleFor(x => x.LicensePlate)
            .NotEmpty()
            .WithMessage("License plate is required.");
    }
}