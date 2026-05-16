using FluentValidation;

namespace MechanicShop.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(100)
            .WithMessage("Name is required.");

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress()
            .WithMessage("Invalid email address.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("phone number must be 7-15 digits, optionally starting with +.");

        RuleFor(x => x.Vehicles)
            .NotNull().WithMessage("Vehicles list cannot be null.")
            .Must(x => x.Count > 0).WithMessage("At least one vehicle is required.");

        RuleForEach(x => x.Vehicles).SetValidator(new CreateVehicleCommandValidator());
    }
}
