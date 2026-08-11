---
id: lld-design-patterns
title: Design Patterns for Interviews
track: lld
module: "01 Foundations"
order: 3
languages: [java, csharp]
summary: Strategy, Factory, Observer, Singleton (careful), Decorator, Adapter — when to reach for each.
---

## Why this matters

Patterns are **named solutions**. Use them when they remove duplication or clarify extension — not to sprinkle jargon.

## Definitions

- **Design pattern:** A named, reusable solution to a recurring design problem that clarifies extension points when used deliberately — not jargon padding.
- **Strategy:** Swaps interchangeable algorithms behind a common interface (e.g., fee calculation or auth providers).
- **Factory:** Centralizes object creation so callers ask for a product by type/channel without knowing concrete classes.
- **Observer:** A subject notifies subscribed listeners of state changes (e.g., occupancy display boards).
- **Decorator:** Wraps an object to stack cross-cutting behavior (logging, retry) while preserving the same interface.
- **Adapter:** Translates a foreign or legacy API into the interface your code expects.
- **Singleton:** Restricts a type to one shared instance — use sparingly for truly process-wide resources (prefer DI lifetimes).

## Pattern cheat sheet

| Pattern | Use when | Example |
|---------|----------|---------|
| Strategy | Swap algorithms | Fee calculation, auth providers |
| Factory | Centralize creation | `Notification.create(channel)` |
| Observer | Fan-out events | Occupancy display board |
| Decorator | Stack behaviors | Logging + retry around HTTP client |
| Adapter | Wrap foreign API | Legacy payment SDK → your interface |
| Singleton | One shared resource | Truly process-wide config (rare in services) |

## Strategy

See SOLID / parking fee — inject interface, swap implementations.

## Factory

```java
interface Notifier { void send(String msg); }

class NotifierFactory {
  static Notifier create(String channel) {
    return switch (channel) {
      case "email" -> new EmailNotifier();
      case "sms" -> new SmsNotifier();
      default -> throw new IllegalArgumentException(channel);
    };
  }
}
```

```csharp
static class NotifierFactory {
  public static INotifier Create(string channel) => channel switch {
    "email" => new EmailNotifier(),
    "sms" => new SmsNotifier(),
    _ => throw new ArgumentOutOfRangeException(nameof(channel))
  };
}
```

## Observer

```java
interface LotListener { void onChange(int free); }

class ParkingLot {
  private final List<LotListener> listeners = new CopyOnWriteArrayList<>();
  void subscribe(LotListener l) { listeners.add(l); }
  private void emit(int free) { for (var l : listeners) l.onChange(free); }
}
```

```csharp
class ParkingLot {
  public event Action<int>? Changed;
  void Emit(int free) => Changed?.Invoke(free);
}
```

## Decorator

```java
class RetryingNotifier implements Notifier {
  private final Notifier inner;
  RetryingNotifier(Notifier inner) { this.inner = inner; }
  public void send(String msg) {
    for (int i = 0; i < 3; i++) {
      try { inner.send(msg); return; }
      catch (RuntimeException ex) { if (i == 2) throw ex; }
    }
  }
}
```

```csharp
class RetryingNotifier(INotifier inner) : INotifier {
  public async Task SendAsync(string msg) {
    for (int i = 0; i < 3; i++) {
      try { await inner.SendAsync(msg); return; }
      catch when (i < 2) { /* retry */ }
    }
  }
}
```

## Singleton — interview nuance

In Spring/ASP.NET you register a **singleton lifetime** via DI. Hand-rolled double-checked locking is rarely needed and hard to test. Prefer DI.

## Interview Q&A

- **Q:** Strategy vs polymorphism?
  **A:** Strategy is composition of a behavior object; often cleaner than deep inheritance.
- **Q:** When is Observer the wrong tool?
  **A:** When you need reliable transactional side effects — use outbox/message bus.
- **Q:** Factory vs DI container?
  **A:** Factory encodes domain creation rules; container wires graphs.

## Pitfalls

- Pattern fishing (“can I use a Bridge here?”) without a pain point  
- Eager Singleton holding DB connections in tests  
- Observer leaks (forgot unsubscribe) in long-lived apps

## 60-second answer

“I reach for Strategy when a rule varies, Factory when creation rules matter, Observer for fan-out UI/metrics, Decorator to stack cross-cutting behavior. I avoid Singleton singletons — I use DI lifetimes instead.”

## Further study

- [Design Patterns Catalog (Refactoring Guru)](https://refactoring.guru/design-patterns) — Strategy, Factory, Observer, Decorator, Adapter
- [Software design pattern (Wikipedia)](https://en.wikipedia.org/wiki/Software_design_pattern) — what patterns are (and are not)
- [SOLID (Wikipedia)](https://en.wikipedia.org/wiki/SOLID) — principles that patterns often support
- [Observer pattern (Wikipedia)](https://en.wikipedia.org/wiki/Observer_pattern) — classic pub-notify structure

## Practice prompts

1. Wrap a third-party email SDK with Adapter + Decorator(retry)  
2. Design a chess piece move Strategy  
3. Compare Observer vs pub/sub queue for order events
