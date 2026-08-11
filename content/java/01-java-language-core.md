---
id: java-language-core
title: Java Language Core
track: java
module: "01 Foundations"
order: 1
languages: [java]
summary: Syntax, OOP, equals/hashCode, generics, and exceptions — the foundations every Java interview assumes.
---

## Why this matters

Before collections, concurrency, or Spring, interviewers check that you speak fluent Java: encapsulation, polymorphism, correct equality, type-safe generics, and exception discipline. Weak answers here sink senior conversations fast.

## Definitions

- **Encapsulation:** Hiding an object’s internal state and exposing behavior only through a controlled API (getters/methods), so invariants stay enforceable.
- **Polymorphism:** Calling through a shared interface or superclass type so different implementations can be substituted at runtime without changing callers.
- **equals/hashCode contract:** Equal objects must have equal hash codes; breaking this silently corrupts `HashMap`/`HashSet` lookups.
- **Generics:** Compile-time type parameters that keep collections and APIs type-safe and remove most casts at call sites.
- **PECS (Producer Extends, Consumer Super):** Wildcard rule — `? extends T` when you only read `T`, `? super T` when you only write `T`.
- **Checked exception:** A recoverable failure the compiler forces callers to handle or declare (`Exception` but not `RuntimeException`).
- **try-with-resources:** `try (Resource r = …)` that auto-closes `AutoCloseable` resources and suppresses secondary close exceptions correctly.
- **Record:** A compact immutable data carrier with canonical constructor and value-based `equals`/`hashCode`/`toString` from its components.

## Concept

### OOP essentials

- **Encapsulation** — hide state; expose behavior through methods  
- **Inheritance** — `extends` for is-a; prefer composition for has-a  
- **Polymorphism** — same interface, different implementations (`override`)  
- **Abstraction** — interfaces define contracts; classes implement them  

```java
public interface PaymentGateway {
  PaymentResult charge(Money amount, String customerId);
}

public final class StripeGateway implements PaymentGateway {
  @Override
  public PaymentResult charge(Money amount, String customerId) {
    // call Stripe
    return PaymentResult.ok();
  }
}
```

Prefer **interfaces + composition** over deep class hierarchies. Use `final` on classes/methods when you don’t want extension.

### Classes, records, enums

```java
public record Money(BigDecimal amount, Currency currency) {
  public Money {
    if (amount.signum() < 0) throw new IllegalArgumentException("negative");
  }
}

public enum OrderStatus { CREATED, PAID, SHIPPED, CANCELLED }
```

Records give immutable data + value-based `equals`/`hashCode`/`toString`. Enums are type-safe constants (and can hold behavior).

### equals and hashCode contract

If two objects are `equals`, they **must** have the same `hashCode`. Break this and `HashMap`/`HashSet` silently misbehave.

```java
public final class UserId {
  private final String value;

  public UserId(String value) {
    this.value = Objects.requireNonNull(value);
  }

  @Override
  public boolean equals(Object o) {
    if (this == o) return true;
    if (!(o instanceof UserId other)) return false;
    return value.equals(other.value);
  }

  @Override
  public int hashCode() {
    return value.hashCode();
  }
}
```

Rules to recite:
1. Reflexive, symmetric, transitive, consistent  
2. `equals` ↔ `hashCode` consistency  
3. Don’t mutate fields used in equality after inserting into a hash structure  
4. Prefer `Objects.equals` / `Objects.hash` for multi-field types  

### Generics

Generics give **compile-time type safety** and erase to raw types at runtime (type erasure).

```java
public class Box<T> {
  private T value;
  public void set(T value) { this.value = value; }
  public T get() { return value; }
}

// Bounded
public static <T extends Comparable<T>> T max(T a, T b) {
  return a.compareTo(b) >= 0 ? a : b;
}
```

**Wildcards:**
- `? extends T` — producer (read as T)  
- `? super T` — consumer (write T)  
- PECS: Producer Extends, Consumer Super  

```java
void copy(List<? extends Number> src, List<? super Number> dest) {
  for (Number n : src) dest.add(n);
}
```

Avoid raw types (`List` instead of `List<String>`). Prefer bounded type parameters over casting.

### Exceptions

| Kind | When | Examples |
|------|------|----------|
| Checked | Recoverable / force caller to handle | `IOException`, `SQLException` |
| Unchecked (`RuntimeException`) | Programming bugs / domain failures often mapped here | `IllegalArgumentException`, `NullPointerException` |
| Error | JVM-level; don’t catch casually | `OutOfMemoryError` |

```java
public User findRequired(String id) {
  return repo.findById(id)
      .orElseThrow(() -> new NotFoundException("user " + id));
}

public void save(Path path, byte[] data) throws IOException {
  Files.write(path, data); // checked — propagate or wrap
}
```

Patterns interviewers want:
- Fail fast on invalid args  
- Don’t swallow exceptions (`catch (Exception e) {}`)  
- Wrap low-level checked exceptions into domain/unchecked at boundaries when appropriate  
- Use try-with-resources for `AutoCloseable`  

```java
try (InputStream in = Files.newInputStream(path)) {
  return in.readAllBytes();
}
```

## Worked example 1 — Value object equality

```java
public final class Email {
  private final String normalized;

  public Email(String raw) {
    if (raw == null || !raw.contains("@")) {
      throw new IllegalArgumentException("invalid email");
    }
    this.normalized = raw.trim().toLowerCase(Locale.ROOT);
  }

  public String value() { return normalized; }

  @Override
  public boolean equals(Object o) {
    return o instanceof Email e && normalized.equals(e.normalized);
  }

  @Override
  public int hashCode() {
    return normalized.hashCode();
  }
}
```

Interview angle: domain identity vs technical identity — emails equal by normalized value, not reference.

## Worked example 2 — Generic repository sketch

```java
public interface Repository<T, ID> {
  Optional<T> findById(ID id);
  T save(T entity);
}

public final class InMemoryRepository<T, ID> implements Repository<T, ID> {
  private final Map<ID, T> store = new HashMap<>();
  private final Function<T, ID> idFn;

  public InMemoryRepository(Function<T, ID> idFn) {
    this.idFn = idFn;
  }

  @Override
  public Optional<T> findById(ID id) {
    return Optional.ofNullable(store.get(id));
  }

  @Override
  public T save(T entity) {
    store.put(idFn.apply(entity), entity);
    return entity;
  }
}
```

## Worked example 3 — Exception translation at a boundary

```java
public Order placeOrder(CreateOrderCommand cmd) {
  try {
    return paymentClient.charge(cmd.total());
  } catch (IOException e) {
    throw new PaymentUnavailableException("payment gateway down", e);
  }
}
```

Keep stack traces (`cause`); don’t lose the original error.

## Interview Q&A

- **Q:** Interface vs abstract class?  
  **A:** Interface = capability/contract (multiple). Abstract class = shared implementation + identity. Prefer interfaces; use abstract class when you truly share state/behavior.
- **Q:** Why override both `equals` and `hashCode`?  
  **A:** Hash-based collections use `hashCode` to find the bucket, then `equals` to find the entry. Inconsistent pair → lost entries / duplicates.
- **Q:** What is type erasure?  
  **A:** Generic type parameters are removed at compile time; runtime sees raw types (with bridges/casts). That’s why you can’t do `new T()` or `instanceof List<String>` cleanly.
- **Q:** Checked vs unchecked — what do you use in APIs?  
  **A:** Domain/service APIs often use unchecked for not-found/conflict; use checked sparingly at I/O boundaries or wrap them.
- **Q:** `==` vs `equals` for strings?  
  **A:** `==` is reference identity; `equals` is value. Always `equals` (or `Objects.equals`) for content.

## Pitfalls

- Mutable keys in `HashMap` after insert  
- Breaking `equals`/`hashCode` symmetry with inheritance (`getClass()` vs `instanceof` debates — prefer final value types)  
- Raw types and unchecked casts silencing real bugs  
- Catching `Exception`/`Throwable` too broadly  
- Empty catch blocks that hide production failures  
- Overusing inheritance for code reuse

## 60-second answer

“I model with small immutable types, interfaces for behavior, and composition over deep hierarchies. Equality is value-based with a matching hashCode. Generics keep APIs type-safe with PECS for wildcards. Exceptions: fail fast, use try-with-resources, and translate infrastructure errors at boundaries without swallowing causes.”

## Further study

- [Java documentation portal](https://docs.oracle.com/en/java/) — official entry point for language, SE, and tooling docs
- [Java SE 21 API](https://docs.oracle.com/en/java/javase/21/docs/api/) — authoritative class/interface reference
- [Object as a Superclass](https://docs.oracle.com/javase/tutorial/java/IandI/objectclass.html) — `equals`, `hashCode`, and identity basics
- [Generics tutorial](https://docs.oracle.com/javase/tutorial/java/generics/index.html) — type parameters, wildcards, and PECS

## Practice prompts

1. Implement `Money` with currency-safe `plus` and strict `equals`/`hashCode`  
2. Write a generic `Result<T>` (success/failure) without exceptions for expected failures  
3. Explain what breaks if `hashCode` always returns `42`  
4. Refactor a checked-exception-heavy API into a clearer service boundary
