---
id: platform-reliability
title: Reliability Patterns
track: platform
module: "04 Reliability"
order: 30
languages: [java, csharp]
summary: Timeouts, retries, circuit breakers, bulkheads, and graceful degradation for senior interviews.
---

## Why this matters

Auth and cache don’t matter if the system collapses under dependency failure. Seniors design for partial outage.

## Definitions

- **Reliability pattern:** A defensive technique that keeps a service useful when dependencies fail or slow down.
- **Timeout:** Upper bound on waiting for a remote call—fail fast instead of holding threads forever.
- **Retry:** Re-attempting transient failures with exponential backoff and jitter; only for safe/idempotent operations.
- **Circuit breaker:** Temporarily stop calling a failing dependency after an error threshold, then probe recovery.
- **Bulkhead:** Isolate thread pools or connection limits so one slow dependency cannot starve the whole process.
- **Graceful degradation:** Serve reduced functionality (cached/stale/feature-off) when a dependency is unavailable.
- **Backpressure:** Slow or reject producers when consumers cannot keep up, preventing unbounded queues.


## Practices

```text
outbound call:
  connect timeout + request timeout
  retry only idempotent/safe ops
  exponential backoff + jitter
  breaker opens after error threshold
```

```csharp
// Conceptual resilience pipeline
// HttpClient + Polly: timeout, retry, circuit breaker policies
```

```java
// Resilience4j: TimeLimiter, Retry, CircuitBreaker, Bulkhead
```

## Interview Q&A

- **Q:** Retry POST?
  **A:** Only with idempotency keys / safe server semantics.
- **Q:** Cache in degradation?
  **A:** Serve stale cache when origin is down if product allows.
- **Q:** How to observe?
  **A:** Error rates, latency, breaker state, saturation (threads/queue depth).

## Pitfalls

- Infinite retries without jitter (retry storm)  
- No timeouts (thread pileup)  
- Shared unbounded thread pool across dependencies

## 60-second answer

“Every outbound call has timeouts, careful retries, and isolation. Breakers and degradation keep the core experience alive when dependencies fail.”

## Further study

- [Microsoft Learn: Resilient HTTP clients with Polly](https://learn.microsoft.com/en-us/dotnet/core/resilience/) — timeouts, retries, and breakers in .NET.
- [AWS Well-Architected: Reliability](https://docs.aws.amazon.com/wellarchitected/latest/reliability-pillar/welcome.html) — failure isolation and recovery design.
- [Release It! patterns (summary via Microsoft Cloud Design Patterns)](https://learn.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker) — circuit breaker pattern write-up.
- [Google SRE Book: Handling Overload](https://sre.google/sre-book/handling-overload/) — load shedding and graceful degradation thinking.

## Practice prompts

1. Design policies for payment vs recommendations calls  
2. Explain a retry storm incident  
3. Pick metrics for breaker dashboards
