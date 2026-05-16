namespace MechanicShop.Application.Features.Customers.DTOs;

public sealed record class VehicleDto
(
    Guid Id,
    string Make,
    string Model,
    int Year,
    string LicensePlate
);
