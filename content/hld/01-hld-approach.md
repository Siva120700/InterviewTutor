---
id: hld-approach
title: HLD Interview Approach
track: hld
module: "01 Foundations"
order: 1
languages: [java, csharp]
summary: How to run a system design interview — requirements, estimates, API, data, scale, trade-offs.
---

## Why this matters

Structure beats brilliance. A clear 45-minute framework gets you to senior signal.

## Definitions

- **High-level design (HLD):** Interview system design focused on requirements, APIs, components, data, scale, and trade-offs rather than class-level code.
- **Functional requirements:** The user-facing use cases the system must support, plus what you explicitly leave out of scope.
- **Non-functional requirements (NFRs):** Constraints like QPS, latency, availability, durability, and consistency that drive architecture choices.
- **Back-of-envelope estimate:** Rough capacity math (storage, QPS, bandwidth) used early to size components and spot bottlenecks.
- **Strong vs eventual consistency:** Whether readers always see the latest write immediately, or may see stale data for a short window.
- **CAP trade-off:** Under network partition, choosing consistency (CP) or availability (AP) for a given subsystem — rarely “having both.”
- **Idempotency:** Making retries safe so the same request produces the same effect once, often via at-least-once delivery plus dedupe.

## 8-step framework

1. **Clarify functional requirements** (use cases, out of scope)  
2. **Non-functional** (QPS, latency, consistency, availability)  
3. **Back-of-envelope** (storage, QPS, bandwidth)  
4. **API sketch**  
5. **High-level diagram**  
6. **Data model**  
7. **Deep dives** (bottlenecks you choose)  
8. **Trade-offs & evolutions**

## Estimation cheat sheet

- 1 day ≈ 10⁵ seconds  
- 1M users × 10 req/day ≈ 100 QPS average (peak 10×)  
- Text ~ bytes × rows; indexes add overhead

## Consistency vocabulary

- Strong vs eventual  
- CAP (practical: pick CP/AP under partition)  
- Idempotency, exactly-once *effects* via at-least-once + dedupe

## Interview habits

- Drive the session; ask before assuming  
- Pick 2–3 deep dives, don’t boil ocean  
- Call out monitoring, rate limits, failure modes

## 60-second answer

“I clarify requirements and NFRs, estimate load, propose API + diagram + storage, then deep-dive the hardest scaling problem with explicit trade-offs.”

## Further study

- [System Design Primer](https://github.com/donnemartin/system-design-primer) — end-to-end interview system design checklist
- [CAP theorem (Wikipedia)](https://en.wikipedia.org/wiki/CAP_theorem) — partition trade-offs for consistency vs availability
- [Consistency model (Wikipedia)](https://en.wikipedia.org/wiki/Consistency_model) — strong vs eventual consistency vocabulary
- [Idempotence (Wikipedia)](https://en.wikipedia.org/wiki/Idempotence) — safe retries under at-least-once delivery

## Practice prompts

1. Time-box a URL shortener in 30 minutes aloud  
2. Estimate Instagram storage for 1B photos  
3. List NFRs for a payments API
