# Clean Architecture: Focused Learning Guide

This guide is a compact, code-first walkthrough of the project's Clean Architecture approach. Each section contains a short explanation, minimal examples, and usage notes — no long theory.

## 1. Project layout (what to open first)

- `src/MechanicShop.Domain` — domain entities, value objects, domain events, and result types.
- `src/MechanicShop.Application` — use-cases, MediatR handlers, DTOs for app boundary.
- `src/MechanicShop.Infrastructure` — EF/Repositories, mappings, integration code.
- `src/MechanicShop.Api` — HTTP controllers / minimal API mapping to application layer.
- `src/MechanicShop.Client` — Blazor front-end that calls the API.
- `src/MechanicShop.Contracts` — shared DTOs/interfaces.

Open order for learning: Domain → Application → Infrastructure → Api → Client.

## 2. Core shared primitives (copyable patterns)

Result / Error pattern (located under `Domain/common/Results`):

```csharp
// Result<T> (usage)
public static Result<Customer> Create(...) {
  if (string.IsNullOrWhiteSpace(name)) return CustomerErrors.NameRequired; // implicit Error -> Result<T>
  return new Customer(...); // implicit Customer -> Result<Customer>
}

// On caller side
var result = Customer.Create(...);
if (result.IsError) return result.Errors; // application maps to HTTP
var customer = result.Value;
```

Error type provides ErrorKind, Code and Description. Use `Error.Validation`, `Error.Conflict`, `Error.NotFound` for semantics.

Entity & AuditableEntity:

```csharp
public abstract class Entity { public Guid Id { get; } /* domain events */ }
public abstract class AuditableEntity : Entity { public DateTimeOffset CreatedAtUtc { get; set; } /* ... */ }
```

Domain events: `DomainEvent : INotification` and `Entity` exposes `AddDomainEvent()` for handlers to pick up.

Naming pattern: `AggregateRoot.cs`, `AggregateRootErrors.cs`, value objects in `Aggregate/ValueObjects/`.

## 3. Concrete domain coding rules (practical, repeatable)

1) Creation via static `Create(...)`

```csharp
public static Result<Vehicle> Create(Guid id, string? make, string? model, int year, string? licensePlate)
{
  if (string.IsNullOrWhiteSpace(make)) return VehicleErrors.MakeRequired;
  if (year < 1886 || year > DateTime.UtcNow.Year) return VehicleErrors.YearInvalid;
  return new Vehicle(id, make, model, year, licensePlate);
}
```

2) Mutations via instance methods returning `Result<Updated>`

```csharp
public Result<Updated> Update(...) {
  // validate; mutate fields; return Result.Updated
}
```

3) Collections: private list + read-only public view + explicit mutators

```csharp
private List<Vehicle> _vehicles = new();
public IEnumerable<Vehicle> Vehicles => _vehicles.AsReadOnly();
public Result<Updated> UpsertVehicles(List<Vehicle> incoming) { /* merge/validate */ }
```

4) Centralize errors per-aggregate

```csharp
public static class CustomerErrors {
  public static Error NameRequired => Error.Validation("Customer_Name_Required", "Customer name is required");
}
```

5) Prefer small value objects for repeated validation

```csharp
// Email value object (recommended)
public sealed class Email {
  public string Value { get; }
  private Email(string value) => Value = value;
  public static Result<Email> Create(string? input) {
    if (string.IsNullOrWhiteSpace(input)) return Error.Validation("Email required");
    try { _ = new MailAddress(input); return new Email(input); } catch { return Error.Validation("Email invalid"); }
  }
}
```

Use value objects inside `Create`/`Update` to keep validation single-sourced.
