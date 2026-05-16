using System.Data;
using FluentValidation;

namespace MechanicShop.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty().WithMessage("Customer id is required.");

        RuleFor(c => c.Name).NotEmpty().WithMessage("Customer name is required.").
            MaximumLength(100).WithMessage("Customer name must not exceed 100 characters.")
            .MinimumLength(2).WithMessage("Customer name must be at least 2 characters long.");

        RuleFor(c => c.Email).NotEmpty().WithMessage("Customer email is required.")
        .EmailAddress().WithMessage("Customer email must be a valid email address.");

        RuleFor(c => c.PhoneNumber).NotEmpty()
        .WithMessage("Customer phone number is required.")
        .Matches(@"^\+?[1-9]\d{1,14}$")
        .WithMessage("Customer phone number must be 7-15 digits, optionally starting with a + .");

        RuleFor(c => c.Vehicles).NotNull().WithMessage("Customer vehicles list cannot be null.")
        .Must(p => p.Count > 0).WithMessage("At least one vehicle is required.");
        RuleForEach(c => c.Vehicles).SetValidator(new UpdateVehicleCommandValidator());

    }
}