---
id: platform-rate-limit-idempotency
title: Rate Limiting and Idempotency
track: platform
module: "03 Security"
order: 22
languages: [java, csharp]
summary: Protect APIs from abuse and safe retries — limits, 429s, and idempotency keys.
---

## Why this matters

Retries, double-clicks, and bots will hit your API. Seniors design for safe replay.

## Definitions

- **Rate limiting:** Capping request rate per identity, IP, or route to protect availability, cost, and downstream dependencies.
- **429 Too Many Requests:** HTTP status when limited; often paired with `Retry-After` so clients back off correctly.
- **Idempotency key:** Client-supplied key so retried mutating requests return the same outcome without double side effects.
- **Idempotent operation:** Repeating the same request leaves the system in the same state (safe GETs; POSTs need explicit keys/design).
- **At-least-once delivery:** Networks and queues may duplicate messages—handlers must tolerate retries.
- **Token bucket:** Algorithm that allows short bursts up to a capacity while refilling at a steady rate.
- **Fixed / sliding window:** Counting algorithms that limit N requests per time window (simple vs smoother edge behavior).


## Rate limiting sketch

```text
key = ratelimit:{userId}:{route}:{minute}
INCR key; EXPIRE key 60
if count > limit → 429
```

## Idempotency

```http
POST /payments
Idempotency-Key: 8f3c-22aa-...
```

```java
Result charge(String key, ChargeCmd cmd) {
  return store.getOrCompute(key, () -> processor.charge(cmd));
}
```

```csharp
Task<Result> ChargeAsync(string key, ChargeCmd cmd) =>
  store.GetOrComputeAsync(key, () => processor.ChargeAsync(cmd));
```

Store: key → response/status, with TTL; same key + different body → **409**.

## Interview Q&A

- **Q:** Where to limit?
  **A:** Edge/gateway for coarse; service for expensive business ops; stricter on `/login`.
- **Q:** GET idempotent?
  **A:** Yes by HTTP semantics; still rate-limit to protect origin.
- **Q:** Distributed limits?
  **A:** Redis shared counters; accept approximate under failure.

## Pitfalls

- Per-pod in-memory limits (N× too high)  
- No idempotency on payment/create  
- Silent drops without 429/metrics

## 60-second answer

“I rate-limit by identity at the edge and tighten on sensitive routes. For mutating POSTs I require idempotency keys so client retries can’t double-charge.”

## Further study

- [MDN: 429 Too Many Requests](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/429) — status semantics and Retry-After.
- [Stripe: Idempotent requests](https://stripe.com/docs/api/idempotent_requests) — production-grade idempotency-key pattern.
- [Redis commands: INCR](https://redis.io/docs/latest/commands/incr/) — atomic counters commonly used for fixed-window limits.
- [IETF: Retry-After](https://www.rfc-editor.org/rfc/rfc9110.html#name-retry-after) — standard header for backoff signaling.

## Practice prompts

1. Limits matrix for login/search/checkout  
2. Design idempotency store schema  
3. Choose token bucket vs fixed window
