---
id: java-spring-boot
title: Spring Boot Interview Topics
track: java
module: "05 Spring"
order: 40
languages: [java]
summary: DI, bean scopes, @Transactional proxies, REST layering, validation, and production checklists for Spring interviews.
---

## Why this matters

Most Java backend roles assume Spring Boot. Interviews focus on **proxies, transactions, layering, and production readiness** — not annotation trivia.

## Definitions

- **Dependency injection:** The container supplies collaborators; classes do not `new` their infrastructure dependencies.
- **Bean scope:** Lifetime/sharing of a bean instance — singleton (default), prototype, request, session.
- **Constructor injection:** Required dependencies via constructor for immutability, clearer graphs, and easier tests.
- **@Transactional:** Declarative DB transaction boundary applied through a Spring AOP proxy (commit/rollback rules).
- **AOP proxy:** Wrapper around a bean that intercepts calls for cross-cutting advice (transactions, security, metrics).
- **Self-invocation:** `this.method()` inside the same class bypasses the proxy — `@Transactional` on that call will not apply.
- **Spring Data repository:** Interface-based persistence abstraction that generates query methods against JPA/JDBC backends.

## Concept

### Layering

```text
Controller → Service → Repository (Spring Data)
```

| Layer | Responsibility |
|-------|----------------|
| Controller | HTTP mapping, validation, status codes |
| Service | Business rules, transaction boundaries |
| Repository | Persistence only |

```mermaid
flowchart LR
  Client --> Controller
  Controller --> Service
  Service --> Repo[Repository]
  Repo --> DB
```

Keep controllers thin. Don’t put `@Transactional` on controllers by default — service boundary is clearer.

### Dependency injection

Prefer **constructor injection** (required deps, immutability, easy tests).

```java
@Service
public class OrderService {
  private final OrderRepository orders;
  private final PaymentClient payments;

  public OrderService(OrderRepository orders, PaymentClient payments) {
    this.orders = orders;
    this.payments = payments;
  }
}
```

**Scopes:** singleton (default), prototype, request, session.  
Repositories/services are singletons — they must be **stateless** (no request-mutable fields).

### AOP proxies and @Transactional

Spring applies transactions via **proxies** (JDK or CGLIB). Rules that matter:

- Public methods on Spring beans  
- **Self-invocation** bypasses the proxy → no new transaction  
- Default rollback on **unchecked** exceptions  
- Checked exceptions need `rollbackFor`  
- Propagation / isolation exist — don’t cargo-cult `REQUIRES_NEW`

## Worked example 1 — REST controller

```java
@RestController
@RequestMapping("/api/orders")
class OrderController {
  private final OrderService service;

  OrderController(OrderService service) {
    this.service = service;
  }

  @PostMapping
  ResponseEntity<OrderDto> create(@Valid @RequestBody CreateOrderRequest req) {
    OrderDto created = service.create(req);
    return ResponseEntity.status(HttpStatus.CREATED).body(created);
  }

  @GetMapping("/{id}")
  OrderDto get(@PathVariable String id) {
    return service.get(id);
  }
}
```

## Worked example 2 — Transaction boundaries

```java
@Service
public class OrderService {
  private final OrderRepository orders;
  private final OrderService self; // better: redesign than self-inject casually

  public OrderService(OrderRepository orders) {
    this.orders = orders;
  }

  @Transactional
  public Order create(CreateOrderRequest req) {
    Order order = Order.from(req);
    return orders.save(order);
  }

  public void oops(CreateOrderRequest req) {
    create(req); // SELF-INVOCATION: no proxy → @Transactional ignored!
  }
}
```

Fix self-invocation: move to another bean, or use AspectJ weaving — redesign is preferred.

## Worked example 3 — Validation and error shape

```java
public record CreateOrderRequest(
    @NotBlank String sku,
    @Min(1) int quantity
) {}

@RestControllerAdvice
class ApiErrors {
  @ExceptionHandler(NotFoundException.class)
  ResponseEntity<ApiError> notFound(NotFoundException ex) {
    return ResponseEntity.status(HttpStatus.NOT_FOUND)
        .body(new ApiError("NOT_FOUND", ex.getMessage()));
  }

  @ExceptionHandler(MethodArgumentNotValidException.class)
  ResponseEntity<ApiError> validation(MethodArgumentNotValidException ex) {
    return ResponseEntity.badRequest()
        .body(new ApiError("VALIDATION", "invalid request"));
  }
}
```

Map domain exceptions → stable HTTP + error codes (404/409/422). Don’t leak stack traces.

## Worked example 4 — Outbound HTTP with timeouts

```java
@Bean
WebClient paymentWebClient(WebClient.Builder builder) {
  HttpClient http = HttpClient.create()
      .responseTimeout(Duration.ofSeconds(2));
  return builder
      .baseUrl("https://payments.example")
      .clientConnector(new ReactorClientHttpConnector(http))
      .build();
}
```

Untimed clients are production incidents waiting to happen.

## Config, profiles, secrets

- `application.yml` + profiles (`dev`, `prod`)  
- Secrets from env / secret manager — **never git**  
- Feature flags / `@ConfigurationProperties` for typed config  

## Data access notes

- Spring Data JPA: derive queries carefully; watch N+1 (`@EntityGraph` / join fetch)  
- Lazy loading outside a session → `LazyInitializationException` — fetch in the transaction or project to DTOs  
- Migrations: Flyway / Liquibase as part of “done”

## Production checklist (senior signal)

- Actuator health / readiness / liveness  
- Structured logging + correlation / request IDs  
- Timeouts, retries with backoff on outbound calls  
- Connection pool sizing (HikariCP)  
- Metrics (Micrometer) + tracing  
- Graceful shutdown  

## Interview Q&A

- **Q:** Filter vs Interceptor vs `@ControllerAdvice`?  
  **A:** Filters are servlet-level; interceptors are Spring MVC; advice shapes exceptions/responses.
- **Q:** Constructor vs field injection?  
  **A:** Constructor — required deps, testable, immutable fields.
- **Q:** Why are beans singletons?  
  **A:** One instance shared; must be thread-safe/stateless. Request state belongs in method args or request-scoped beans.
- **Q:** How do you test?  
  **A:** Unit-test services with fakes; slice tests for MVC/JPA; `@SpringBootTest` sparingly for wiring.
- **Q:** Circular dependency?  
  **A:** Design smell — split the cycle; `@Lazy` is a bandage.
- **Q:** `@Transactional` rollback on checked exception?  
  **A:** Not by default — set `rollbackFor`.

## Pitfalls

- Fat controllers / business logic in REST layer  
- Self-invocation skipping transactions  
- Open session / lazy load in views after TX ends  
- Catching `Exception` inside `@Transactional` methods (swallows rollback triggers)  
- Component scan surprises / duplicate beans  
- Missing timeouts on `WebClient` / `RestTemplate`  
- Putting secrets in `application.yml` committed to git

## 60-second answer

“I keep controllers thin, put business logic and `@Transactional` on services, and remember proxy self-invocation pitfalls. Constructor DI, Bean Validation, consistent error JSON, and ops hooks — health, timeouts, pools, migrations — are part of done. I fetch data inside the transaction or project to DTOs to avoid lazy-load surprises.”

## Further study

- [Spring Boot reference](https://docs.spring.io/spring-boot/docs/current/reference/html/) — auto-config, actuators, and app structure
- [Spring transaction management](https://docs.spring.io/spring-framework/reference/data-access/transaction.html) — proxy rules, propagation, rollback
- [Spring Data JPA](https://docs.spring.io/spring-data/jpa/reference/) — repositories, query methods, and fetch pitfalls
- [Spring Framework DI](https://docs.spring.io/spring-framework/reference/core/beans/dependencies/factory-collaborators.html) — constructor injection and bean wiring

## Practice prompts

1. Design an idempotent create-order API with Spring + DB uniqueness  
2. Fix a `LazyInitializationException` three different ways and pick one  
3. Sketch retry + timeout policy for an outbound payment call  
4. Explain how you’d structure packages for a modular monolith
