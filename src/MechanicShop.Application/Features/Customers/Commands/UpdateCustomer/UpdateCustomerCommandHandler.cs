using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.Customers;
using MechanicShop.Domain.Customers.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Customers.Commands.UpdateCustomer;


public class UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommandHandler> logger,
                                          IAppDbContext context,
                                          IMemoryCache cache) : IRequestHandler<UpdateCustomerCommand, Result<Updated>>
{
    private readonly ILogger<UpdateCustomerCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IMemoryCache _cache = cache;

    public async Task<Result<Updated>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
                                    .Include(rt => rt.Vehicles)
                                    .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);
        if (customer is null)
        {
            _logger.LogWarning("Customer with id {CustomerId} not found.", request.CustomerId);
            return ApplicationErrors.CustomerNotFound;
        }
        var validatedVehicles = new List<Vehicle>();
        foreach (var v in request.Vehicles)
        {
            var vehicleId = v.VehicleId ?? Guid.NewGuid();
            var vehicle = Vehicle.Create(vehicleId, v.Make, v.Model, v.Year, v.LicensePlate);
            if (vehicle.IsError)
            {
                _logger.LogWarning("Invalid vehicle data for vehicle with id {VehicleId}. Errors: {Errors}", vehicleId, vehicle.Errors);
                return vehicle.Errors!;
            }
            validatedVehicles.Add(vehicle.Value!);
        }

        var updateResult = customer.Update(request.Name, request.Email, request.PhoneNumber);
        if (updateResult.IsError)
        {
            _logger.LogWarning("Failed to update customer with id {CustomerId}. Errors: {Errors}", request.CustomerId, updateResult.Errors);
            return updateResult.Errors!;
        }

        var upsertResult = customer.UpsertParts(validatedVehicles);
        if (upsertResult.IsError)
        {
            _logger.LogWarning("Failed to upsert vehicles for customer with id {CustomerId}. Errors: {Errors}", request.CustomerId, upsertResult.Errors);
            return upsertResult.Errors!;
        } 
        await _context.SaveChangesAsync(cancellationToken);
        _cache.Remove("customers");
        return Result.Updated;
    }
}