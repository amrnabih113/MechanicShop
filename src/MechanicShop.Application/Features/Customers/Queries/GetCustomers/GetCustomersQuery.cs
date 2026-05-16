using MechanicShop.Application.common.Interfaces;
using MechanicShop.Application.Features.Customers.DTOs;
using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Application.Features.Customers.Queries.GetCustomers;

public sealed record class GetCustomersQuery()
: ICachedQuery<Result<List<CustomerDto>>> 
{
    public string CacheKey => "customers";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["customer"];
}