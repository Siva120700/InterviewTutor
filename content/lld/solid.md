---
id: lld-solid
title: SOLID in Practice
track: lld
module: "01 Foundations"
order: 2
languages: [java, csharp]
summary: SOLID with real refactor examples — what to say in LLD rounds without buzzword soup.
---

## Why this matters

Senior LLD rounds expect you to **name a principle and show a small design change**. SOLID is a vocabulary for trade-offs, not a checklist to tattoo on every class.

## Definitions

- **SOLID:** Five OOP design principles (SRP, OCP, LSP, ISP, DIP) used in interviews to name trade-offs and justify small, extensible designs.
- **Single Responsibility Principle (SRP):** A class should have one reason to change — one cohesive job, not a god service that prices, persists, emails, and metrics.
- **Open/Closed Principle (OCP):** Software should be open for extension (new types/strategies) and closed for modification of existing working code.
- **Liskov Substitution Principle (LSP):** Subtypes must honor the base contract so callers can use them without surprising failures or weakened guarantees.
- **Interface Segregation Principle (ISP):** Prefer small, focused interfaces so clients are not forced to depend on methods they do not use.
- **Dependency Inversion Principle (DIP):** High-level policy should depend on abstractions (interfaces), not concrete low-level implementations — typically via constructor injection.
- **Composition over inheritance:** Prefer “has-a” collaborators (strategies, services) over deep “is-a” hierarchies when behavior varies.

## Concept (one line each)

| Letter | Meaning | Interview signal |
|--------|---------|------------------|
| **S** | Single Responsibility | One reason to change |
| **O** | Open/Closed | Extend via new types, not edits everywhere |
| **L** | Liskov | Subtypes must honor the contract |
| **I** | Interface Segregation | Don’t force fat interfaces |
| **D** | Dependency Inversion | Depend on abstractions |

## Worked example — SRP + DIP (orders)

**Before (god service):** prices, persists, sends email, logs metrics.

**After:**

```java
interface OrderRepository { void save(Order o); }
interface Notifier { void send(String to, String msg); }
interface PriceCalculator { Money price(Cart cart); }

class OrderService {
  private final OrderRepository repo;
  private final Notifier notifier;
  private final PriceCalculator pricing;

  OrderService(OrderRepository repo, Notifier notifier, PriceCalculator pricing) {
    this.repo = repo; this.notifier = notifier; this.pricing = pricing;
  }

  Order place(Cart cart, String email) {
    Money total = pricing.price(cart);
    Order o = new Order(cart, total);
    repo.save(o);
    notifier.send(email, "Order " + o.id() + " placed");
    return o;
  }
}
```

```csharp
interface IOrderRepository { Task SaveAsync(Order o); }
interface INotifier { Task SendAsync(string to, string msg); }
interface IPriceCalculator { Money Price(Cart cart); }

class OrderService(
  IOrderRepository repo,
  INotifier notifier,
  IPriceCalculator pricing) {
  public async Task<Order> PlaceAsync(Cart cart, string email) {
    var total = pricing.Price(cart);
    var o = new Order(cart, total);
    await repo.SaveAsync(o);
    await notifier.SendAsync(email, $"Order {o.Id} placed");
    return o;
  }
}
```

**Say in interview:** constructor injection makes the service testable; email failures shouldn’t corrupt persistence (outbox/async later).

## Worked example — OCP with Strategy (fees)

```java
interface FeeStrategy { Money fee(Ticket t); }

class HourlyFee implements FeeStrategy {
  public Money fee(Ticket t) { /* hours * rate */ return Money.ZERO; }
}

class ParkingLot {
  private final FeeStrategy fees;
  ParkingLot(FeeStrategy fees) { this.fees = fees; }
  Money checkout(Ticket t) { return fees.fee(t); }
}
```

```csharp
interface IFeeStrategy { Money Fee(Ticket t); }

class HourlyFee : IFeeStrategy {
  public Money Fee(Ticket t) => Money.Zero; /* hours * rate */
}

class ParkingLot(IFeeStrategy fees) {
  public Money Checkout(Ticket t) => fees.Fee(t);
}
```

New weekend pricing → **new class**, not a growing `if/switch` in `ParkingLot`.

## Worked example — LSP violation

```java
class Rectangle { int w, h; void setW(int w){this.w=w;} void setH(int h){this.h=h;} }
class Square extends Rectangle {
  void setW(int w){ this.w=this.h=w; } // breaks callers expecting independent h
}
```

**Fix:** don’t inherit Square from Rectangle; share a `Shape` with `area()` only.

## ISP sketch

Prefer `interface Lock { void lock(); void unlock(); }` and separate `interface TimedLock` over one mega `DeviceOps` with 40 methods.

## Interview Q&A

- **Q:** Isn’t this over-engineering for a parking lot?
  **A:** Extract seams where requirements already fork (fees, spot types). Don’t invent plugins for a coding exercise.
- **Q:** How do you test OrderService?
  **A:** Fake repository/notifier; assert `place` calls save then notify with right args.
- **Q:** DIP vs DI framework?
  **A:** DIP is the design rule; Spring/ASP.NET DI is a mechanism.

## Pitfalls

- Interface per class with one implementation forever (noise)  
- “Utility” classes that hide SRP violations  
- Claiming OCP while still editing a 200-line switch

## 60-second answer

“SOLID helps me place change. I keep orchestration thin, push varying rules into strategies, and depend on interfaces at boundaries so tests and new channels (email/SMS) don’t rewrite core flow.”

## Further study

- [SOLID (Wikipedia)](https://en.wikipedia.org/wiki/SOLID) — SRP, OCP, LSP, ISP, and DIP definitions
- [Dependency injection (Wikipedia)](https://en.wikipedia.org/wiki/Dependency_injection) — common DIP implementation technique
- [Design Patterns (Refactoring Guru)](https://refactoring.guru/design-patterns) — patterns that embody SOLID (Strategy, etc.)
- [Strategy pattern (Refactoring Guru)](https://refactoring.guru/design-patterns/strategy) — OCP-friendly interchangeable algorithms

## Practice prompts

1. Refactor a report exporter that mixes CSV/PDF/email  
2. Design payment methods with Strategy + Factory  
3. Find an LSP bug in a shape hierarchy
