---
id: dotnet-csharp-essentials
title: C# Essentials
track: dotnet
module: "01 Foundations"
order: 1
languages: [csharp]
summary: Types, OOP, records, nullability, pattern matching, exceptions, and modern C# idioms for interviews.
---

## Why this matters

.NET interviews expect fluent modern C#: value vs reference semantics, immutability with records, nullable reference types, and clear error handling. This is the language floor before LINQ, async, and ASP.NET.

## Definitions

- **Reference type:** Heap-allocated type (`class` / `record` class); assignment copies a reference, not the object.
- **Value type:** `struct`/primitive/`record struct` copied by value; often inlined or stack-allocated — watch mutable-struct copy bugs.
- **Record:** Type optimized for immutable data with value-based equality and non-destructive `with` updates.
- **Nullable reference types:** Compiler nullability annotations (`string` vs `string?`) that surface likely NREs at compile time.
- **Pattern matching:** `is` patterns and switch expressions that test/extract by type, constant, or shape.
- **init accessor:** Property setter allowed only during object initialization — enables immutable-style models.
- **Span<T> / ReadOnlySpan<T>:** Stack-only views over contiguous memory for allocation-free slicing and parsing.

## Concept

### Value vs reference

| Kind | Examples | Semantics |
|------|----------|-----------|
| Reference types | `class`, `record` (class) | Identity on heap; assignment copies reference |
| Value types | `struct`, `record struct`, primitives | Copy by value; stack or inline |

Don’t cargo-cult `struct` for “performance” without measuring. Prefer `class`/`record` for most domain models; use structs for small, immutable, short-lived data.

```csharp
public sealed class User
{
    public required string Id { get; init; }
    public required string Email { get; init; }
}

public readonly record struct Money(decimal Amount, string Currency);
```

### OOP in C#

- Classes + interfaces for abstraction  
- Prefer composition; seal classes you don’t intend to extend  
- Properties over public fields  
- `init` / `required` for construct-time clarity  

```csharp
public interface IPaymentGateway
{
    Task<PaymentResult> ChargeAsync(Money amount, string customerId, CancellationToken ct);
}

public sealed class StripeGateway : IPaymentGateway
{
    public Task<PaymentResult> ChargeAsync(Money amount, string customerId, CancellationToken ct)
        => /* call Stripe */ Task.FromResult(PaymentResult.Ok());
}
```

### Records

Records give value equality, `with` expressions, and concise DTOs.

```csharp
public sealed record Order(int Id, string Sku, decimal Total);

var discounted = order with { Total = order.Total * 0.9m };
```

Use `record` for immutable data; use `class` when you need identity/mutability/inheritance hierarchies.

### Nullable reference types (NRT)

```csharp
string? name = GetName();
if (name is null) return;
Console.WriteLine(name.Length); // compiler flow analysis
```

Enable NRT in the project. Treat warnings as bugs in new code. Prefer `ArgumentNullException.ThrowIfNull` at boundaries.

### Pattern matching

```csharp
return shape switch
{
    Circle c => Math.PI * c.Radius * c.Radius,
    Rectangle { Width: var w, Height: var h } => w * h,
    _ => 0
};
```

Also: property patterns, list patterns, `is` patterns for concise null/type checks.

### Exceptions and Result-style flows

```csharp
public User GetRequired(string id)
    => repo.Find(id) ?? throw new NotFoundException($"user {id}");

try
{
    await using var stream = File.OpenRead(path);
    // ...
}
catch (IOException ex)
{
    throw new StorageException("read failed", ex);
}
```

Don’t catch `Exception` broadly in libraries. Prefer specific exceptions; preserve `InnerException`.

### Span / Memory (senior flavor)

```csharp
ReadOnlySpan<char> s = input.AsSpan().Trim();
int comma = s.IndexOf(',');
var left = s[..comma];
```

Zero-alloc slicing for parsers and hot paths. Mention when discussing performance.

### Equality basics

- Default `class` equality = reference  
- `record` equality = value (all members)  
- Override `Equals`/`GetHashCode` together for custom class equality  
- `IEquatable<T>` for typed equality without boxing  

## Worked example 1 — Domain model with records

```csharp
public sealed record Email
{
    public string Value { get; }

    public Email(string raw)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(raw);
        if (!raw.Contains('@')) throw new ArgumentException("invalid email", nameof(raw));
        Value = raw.Trim().ToLowerInvariant();
    }

    public override string ToString() => Value;
}
```

## Worked example 2 — Interface + DI-friendly design

```csharp
public interface IOrderRepository
{
    Task<Order?> GetAsync(int id, CancellationToken ct);
    Task SaveAsync(Order order, CancellationToken ct);
}

public sealed class OrderService(IOrderRepository repo, IPaymentGateway payments)
{
    public async Task<Order> PlaceAsync(CreateOrder cmd, CancellationToken ct)
    {
        var order = Order.Create(cmd);
        await payments.ChargeAsync(order.Total, cmd.CustomerId, ct);
        await repo.SaveAsync(order, ct);
        return order;
    }
}
```

Primary constructors (C# 12) keep services terse — know them even if the codebase uses classic constructors.

## Worked example 3 — Defensive parsing with patterns

```csharp
static bool TryParseStatus(string? input, out OrderStatus status)
{
    status = input?.ToLowerInvariant() switch
    {
        "created" => OrderStatus.Created,
        "paid" => OrderStatus.Paid,
        "shipped" => OrderStatus.Shipped,
        _ => default
    };
    return input is not null && status != default;
}
```

## Interview Q&A

- **Q:** When `record` over `class`?  
  **A:** Immutable DTO / value-like data with value equality and `with` updates.
- **Q:** `struct` vs `class`?  
  **A:** Structs copy; best for small immutable data. Classes for identity and most domain entities.
- **Q:** What do nullable reference types buy you?  
  **A:** Compile-time emptiness tracking — fewer NREs when taken seriously.
- **Q:** `async void`?  
  **A:** Only event handlers; otherwise return `Task`/`ValueTask`.
- **Q:** `string` equality?  
  **A:** `==` is overloaded for value equality on strings; still be careful with cultures/`StringComparison`.
- **Q:** `IDisposable` vs `IAsyncDisposable`?  
  **A:** Sync vs async cleanup; prefer `await using` for async resources.

## Pitfalls

- Mutable public setters on “immutable” models  
- Boxing in hot paths with poorly used structs/interfaces  
- Disabling NRT instead of fixing warnings  
- Catching `Exception` and swallowing  
- Deep inheritance for reuse  
- Confusing reference equality with value equality on classes

## 60-second answer

“I write modern C# with sealed classes or records, constructor injection, and nullable reference types on. I pick record for immutable values, class for identity, and I’m intentional about equality. Pattern matching keeps control flow clear; Span shows up when parsing is hot. Exceptions stay specific and chained.”

## Further study

- [C# documentation](https://learn.microsoft.com/en-us/dotnet/csharp/) — language overview and modern C# features
- [Nullable reference types](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references) — nullability annotations and compiler flow analysis
- [Records](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record) — value equality, `with`, and positional syntax
- [Pattern matching](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching) — `is` / switch patterns used in interviews

## Practice prompts

1. Refactor anemic DTOs into records + `with` expressions  
2. Implement `Money` with currency-safe addition and value equality  
3. Parse a CSV line using `ReadOnlySpan<char>` without allocations  
4. Explain a bug caused by mutable struct copying
