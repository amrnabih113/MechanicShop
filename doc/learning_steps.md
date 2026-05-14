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

## 4. Application layer: minimal patterns

- Use MediatR handlers for commands/queries. Handlers call repositories or domain factories and return `Result<T>`.

Example command handler skeleton:

```csharp
public class CreateCustomerHandler : IRequestHandler<CreateCustomer, Result<CustomerDto>>
{
  public async Task<Result<CustomerDto>> Handle(CreateCustomer req, CancellationToken ct)
  {
    var vehicles = req.Vehicles.Select(v => Vehicle.Create(...)).ToList(); // map/validate
    var customerResult = Customer.Create(Guid.NewGuid(), req.Name, req.Phone, req.Email, vehicles);
    if (customerResult.IsError) return MapErrors(customerResult.Errors);
    await _repository.Add(customerResult.Value);
    return MapToDto(customerResult.Value);
  }
}
```

Note: repository Add/Save populates `CreatedAtUtc`/`CreatedBy` and persists events.

## 5. Infrastructure & API mapping (concise)

- Repositories implement interfaces defined in `Application`/`Contracts`. Keep EF models and mapping local to Infrastructure.
- API controllers should translate `Result` -> HTTP responses:

  - `ErrorKind.Validation` -> 400 Bad Request
  - `ErrorKind.Conflict` -> 409 Conflict
  - `ErrorKind.NotFound` -> 404 Not Found
  - `Success/Created` -> 200/201

Example API mapping (pseudo):

```csharp
var result = await _mediator.Send(cmd);
if (result.IsError) return ProblemFromErrors(result.Errors);
return Ok(result.Value);
```

## 6. Quick unit-test examples

Customer creation test (xUnit-like):

```csharp
[Fact]
public void CreateCustomer_InvalidEmail_ReturnsError()
{
  var res = Customer.Create(Guid.NewGuid(), "Name", "+123456789", "bad-email", new List<Vehicle>());
  Assert.True(res.IsError);
  Assert.Equal("Customer_Email_Invalid", res.TopError.Code);
}
```

## 7. Recommended short refactors you can do now

- Extract `Email` value object and update `Customer` to use it.
- Move phone regex into `PhoneNumber` helper/value object.
- Add one unit test per `Create` and `Update` on core entities.

## 8. Usage / run notes

Run API:

```powershell
dotnet run --project src/MechanicShop.Api
```

Run client:

```powershell
dotnet run --project src/MechanicShop.Client
```

## 9. Next actions (choose one)

- I can add `Email` and `PhoneNumber` value objects and update `Customer` accordingly.
- I can add unit tests for `Customer` and `Vehicle` create/update flows.
- I can add concise doc comments to `Domain/common/Results` and `Entity` files.

---
Minimal, example-first learning doc added. Tell me which of the Next actions to perform and I'll implement it.
