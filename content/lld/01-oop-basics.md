---
id: lld-oop-basics
title: OOP Basics for Interviews
track: lld
module: "01 Foundations"
order: 1
languages: [java, csharp]
summary: Classes, encapsulation, inheritance vs composition, interfaces — the LLD starting line.
---

## Why this matters

LLD interviews assume fluent OOP. Weak modeling shows up before patterns do.

## Definitions

- **Object-oriented programming (OOP):** Modeling software as objects with state and behavior, using encapsulation, abstraction, inheritance, and polymorphism.
- **Encapsulation:** Hiding internal state behind well-defined methods so callers cannot corrupt invariants via public fields.
- **Abstraction:** Exposing meaningful interfaces and concepts while hiding irrelevant implementation detail.
- **Inheritance:** An is-a relationship where a subtype reuses/extends a base type — use sparingly to avoid fragile base classes.
- **Polymorphism:** Calling the same contract on different implementations so behavior can vary without changing the caller.
- **Composition:** Building behavior by holding (“has-a”) collaborator objects — preferred over deep inheritance for flexibility.
- **Interface vs abstract class:** Interfaces declare capabilities (often many); abstract classes share base state/implementation (usually one).

## Pillars

- **Encapsulation** — hide state, expose behavior  
- **Abstraction** — meaningful interfaces  
- **Inheritance** — is-a (use sparingly)  
- **Polymorphism** — same contract, different impl

## Prefer composition

```java
class Engine { void start() {} }
class Car {
  private final Engine engine;
  Car(Engine engine) { this.engine = engine; }
  void start() { engine.start(); }
}
```

```csharp
class Engine { public void Start() {} }
class Car(Engine engine) {
  public void Start() => engine.Start();
}
```

“Has-a” beats deep “is-a” trees for flexibility.

## Interface vs abstract class

| | Interface | Abstract class |
|---|-----------|----------------|
| Multi | Many | One (Java/C#) |
| State | Limited/default methods | Fields OK |
| Use | Capabilities | Shared base implementation |

## Interview Q&A

- **Q:** Inheritance smell?
  **A:** Fragile base class; prefer interfaces + composition.
- **Q:** Anemic domain models?
  **A:** Entities with only getters — push behavior next to data when it belongs there.

## Pitfalls

- God classes  
- Leaking internals via public fields  
- Inheritance for code reuse only

## 60-second answer

“I model with clear responsibilities, encapsulate state, depend on interfaces, and favor composition over deep inheritance so designs stay testable and extensible.”

## Further study

- [Object-oriented programming (Wikipedia)](https://en.wikipedia.org/wiki/Object-oriented_programming) — encapsulation, inheritance, polymorphism
- [Composition over inheritance (Wikipedia)](https://en.wikipedia.org/wiki/Composition_over_inheritance) — preferred flexibility in LLD
- [SOLID (Wikipedia)](https://en.wikipedia.org/wiki/SOLID) — principles that refine OOP designs
- [Design Patterns (Refactoring Guru)](https://refactoring.guru/design-patterns) — named solutions for recurring structure

## Practice prompts

1. Model a library book loan without a god `Library`  
2. Refactor a subclass hierarchy into strategies  
3. Draw CRC cards for a vending machine
