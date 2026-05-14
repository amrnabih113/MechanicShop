
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


    public static Error RoleInvalid => Error.Validation(
        "InvalidRole",
        "Employee role is invalid.");
   }