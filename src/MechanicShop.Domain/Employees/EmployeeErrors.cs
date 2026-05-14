
using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.Employees;

public sealed class EmployeeErrors
{
 public static Error IdRequired =>  Error.Validation(
        "IdRequired",
        "Employee ID is required.");

    public static Error FirstNameRequired =>  Error.Validation(
        "FirstNameRequired",
        "Employee first name is required.");

    public static Error LastNameRequired =>  Error.Validation(
        "LastNameRequired",
        "Employee last name is required.");

    public static Error EmailRequired =>  Error.Validation(
        "EmailRequired",
        "Employee email is required.");
    public static Error InvalidEmail => Error.Validation(
        "InvalidEmail",
        "Employee email must be a valid email address.");

    public static Error InvalidPhoneNumber => Error.Validation(
        "InvalidPhoneNumber",
        "Employee phone number must be a valid phone number.");
}