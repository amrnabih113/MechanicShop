using MechanicShop.Application.Features.Labor.DTOs;
using MechanicShop.Domain.Employees;

namespace MechanicShop.Application.Features.Labor.Mappers;

public static class LaborMapper
{
    public static LaborDto ToDto(this Employee employee) => new LaborDto(employee.Id, employee.FullName);

    public static List<LaborDto> ToDtos(this IEnumerable<Employee> employees) => employees.Select(e => e.ToDto()).ToList();
}