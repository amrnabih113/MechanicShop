using MechanicShop.Application.Features.Customers.DTOs;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Customers.Commands.CreateCustomer;

public sealed record class CreateCustomerCommand(
  string Name,
    string Email,
    string PhoneNumber,
    List<CreateVehicleCommand> Vehicles) : IRequest<Result<CustomerDto>>
{

}
