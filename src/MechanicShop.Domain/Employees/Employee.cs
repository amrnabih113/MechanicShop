using System.Net.Mail;
using System.Text.RegularExpressions;
using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.Employees;

public sealed class Employee : AuditableEntity
{
    public string? FirstName { get; }
    public string? LastName { get; }
    public string? PhoneNumber { get; }
    public string? Email { get; }

    public string FullName => $"{FirstName} {LastName}".Trim();

    private Employee()
    {
    }

    private Employee(Guid id, string? firstName, string? lastName, string? phoneNumber, string? email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public static Result<Employee> Create(Guid id, string? firstName, string? lastName, string phoneNumber, string? email)
    {
        if (id == Guid.Empty)
        {
            return EmployeeErrors.IdRequired;
        }
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return EmployeeErrors.FirstNameRequired;
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            return EmployeeErrors.LastNameRequired;
        }
       
        if (!Regex.IsMatch(phoneNumber, @"^\+?\d{7,15}$"))
        {
            return EmployeeErrors.InvalidPhoneNumber;
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            return EmployeeErrors.EmailRequired;
        }

        try
        {
            _ = new MailAddress(email);
        }
        catch (FormatException)
        {
            return EmployeeErrors.InvalidEmail;
        }

        return new Employee(id, firstName, lastName, phoneNumber, email);
     
    }

}