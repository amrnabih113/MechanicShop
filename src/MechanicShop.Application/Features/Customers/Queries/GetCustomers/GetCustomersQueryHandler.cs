using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Customers.DTOs;
using MechanicShop.Application.Features.Customers.Mappers;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Customers.Queries.GetCustomers;


public class GetCustomersQueryHandler(
    IAppDbContext context
  ) : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
{

    private readonly IAppDbContext _context = context;

    public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken ct)
    {
        var customers = await _context.Customers.Include(c => c.Vehicles).AsNoTracking().Select(c => c.ToDto()).ToListAsync(ct);


        return customers;
    }
}
