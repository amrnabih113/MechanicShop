using MechanicShop.Domain.Customers.Vehicles;
using MechanicShop.Domain.Employees;
using MechanicShop.Domain.Identity;
using MechanicShop.Domain.RepairTasks;
using MechanicShop.Domain.RepairTasks.parts;
using MechanicShop.Domain.Workorders;
using MechanicShop.Domain.Workorders.Billing;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Application.Common.Interfaces;


public interface IAppDbContext
{
    DbSet<Customer> Customers { get; }

    DbSet<Employee> Employees { get; }

    DbSet<Vehicle> Vehicles { get; }

    DbSet<Part> Parts { get; }

    DbSet<WorkOrder> WorkOrders { get; }

    DbSet<RepairTask> RepairTasks { get; }

    DbSet<Invoice> Invoices { get; }

    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}