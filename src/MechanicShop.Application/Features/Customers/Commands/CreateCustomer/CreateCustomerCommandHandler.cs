

using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Customers.DTOs;
using MechanicShop.Application.Features.Customers.Mappers;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.Customers;
using MechanicShop.Domain.Customers.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler (IAppDbContext Context,
                                                  ILogger<CreateCustomerCommandHandler> Logger,
                                                  HybridCache cache) : 
IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    private readonly IAppDbContext _context = Context;
    private readonly ILogger<CreateCustomerCommandHandler> _logger = Logger;
    private readonly HybridCache _cache = cache;

    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand command, CancellationToken ct)
    {
        var email = command.Email.Trim().ToLower();

        var existingCustomer = await _context.Customers.AnyAsync(c => c.Email!.ToLower() == email, ct);
        if (existingCustomer)
        {
            _logger.LogWarning("Attempt to create customer with An existing email");
            return CustomerErrors.CustomerExists;
        }
        List<Vehicle> vehicles = [];
        foreach (var v in command.Vehicles)
        {
            var vehicle = Vehicle.Create(Guid.NewGuid(), v.Make, v.Model, v.Year, v.LicensePlate);
            if (vehicle.IsError)
            {
                return vehicle.Errors!;
            }
            vehicles.Add(vehicle.Value);

        }
        
        var customer = Customer.Create(Guid.NewGuid(), command.Name, email, command.PhoneNumber, vehicles);
        if (customer.IsError)
        {
            return customer.Errors!;
        }

        await _context.Customers.AddAsync(customer.Value, ct);
        await _context.SaveChangesAsync(ct);

        var result = customer.Value;
        _logger.LogInformation("Customer with id {CustomerId} created successfully", result.Id);
        await _cache.RemoveByTagAsync("Customer", ct);
        return result.ToDto();
    }
}
