using FluentValidation;

namespace MechanicShop.Application.Features.Customers.Commands.UpdateCustomer;


public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
    public UpdateVehicleCommandValidator()
    {
        RuleFor(v => v.VehicleId).NotEmpty().WithMessage("Vehicle id is required.");

        RuleFor(v => v.Make).NotEmpty().WithMessage("Vehicle make is required.")
            .MaximumLength(50).WithMessage("Vehicle make must not exceed 50 characters.")
            .MinimumLength(2).WithMessage("Vehicle make must be at least 2 characters long.");

        RuleFor(v => v.Model).NotEmpty().WithMessage("Vehicle model is required.")
            .MaximumLength(50).WithMessage("Vehicle model must not exceed 50 characters.")
            .MinimumLength(1).WithMessage("Vehicle model must be at least 1 character long.");

        RuleFor(v => v.Year).InclusiveBetween(1886, DateTime.Now.Year + 1)
            .WithMessage($"Vehicle year must be between 1886 and {DateTime.Now.Year + 1}.");

        RuleFor(v => v.LicensePlate).NotEmpty().WithMessage("License plate is required.")
            .MaximumLength(10).WithMessage("License plate must not exceed 20 characters.")
            .MinimumLength(1).WithMessage("License plate must be at least 1 character long.");
    }
}