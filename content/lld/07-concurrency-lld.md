---
id: lld-concurrency
title: Concurrency in LLD
track: lld
module: "03 Advanced"
order: 20
languages: [java, csharp]
summary: Thread-safe class design, producer-consumer, immutable snapshots, and lock strategies.
---

## Why this matters

Senior LLD follow-ups: “make it thread-safe”. You need vocabulary and a simple correct design.

## Definitions

- **Concurrency in LLD:** Designing classes so shared state stays correct when multiple threads call methods at once, using the weakest sync that preserves invariants.
- **Immutability:** Publishing new snapshots instead of mutating shared state so readers never see half-updated objects.
- **Thread confinement:** Restricting mutable state to a single owning thread so no locks are needed for that state.
- **Critical section:** The minimal block of code that must run under a lock to keep an invariant (e.g., check-then-act) atomic.
- **Producer-consumer:** A pipeline where producers enqueue work and consumers dequeue it, typically via a bounded blocking queue.
- **Check-then-act bug:** A race where reading state then acting is not atomic, so another thread invalidates the assumption in between.
- **Concurrent collection:** A thread-safe structure (e.g., `ConcurrentHashMap`) that handles much of its own synchronization internally.

## Strategies

1. **Immutability** — publish new snapshots  
2. **Confinement** — single thread owns state  
3. **Locks** — synchronize critical sections  
4. **Concurrent collections** — `ConcurrentHashMap`, channels/queues  
5. **STM/actors** — mention only

## Producer-consumer

```java
BlockingQueue<Job> q = new ArrayBlockingQueue<>(100);
// producers q.put(job); consumers q.take();
```

```csharp
var q = new Channel<Job>(new BoundedChannelOptions(100) { FullMode = BoundedChannelFullMode.Wait });
```

## Thread-safe LRU note

Coarse `synchronized` on all public methods is acceptable in interview; discuss striping later.

## Immutable config reload

```java
final AtomicReference<Config> cfg = new AtomicReference<>(Config.load());
void reload() { cfg.set(Config.load()); }
Config get() { return cfg.get(); }
```

```csharp
Config _cfg = Config.Load();
void Reload() => Interlocked.Exchange(ref _cfg, Config.Load());
```

## Interview Q&A

- **Q:** Check-then-act bugs?
  **A:** Combine into one atomic operation / lock.
- **Q:** Read-heavy maps?
  **A:** `ConcurrentHashMap` or read-write locks; measure.

## Pitfalls

- Locking too coarse (throughput death)  
- Calling slow I/O under lock  
- Publishing mutable internals

## 60-second answer

“I pick the weakest sync that keeps invariants: immutable snapshots, concurrent queues for pipelines, and small synchronized sections for shared mutable structures. I avoid IO under locks.”

## Further study

- [Concurrent computing (Wikipedia)](https://en.wikipedia.org/wiki/Concurrent_computing) — shared-state correctness basics
- [Producer–consumer problem (Wikipedia)](https://en.wikipedia.org/wiki/Producer%E2%80%93consumer_problem) — bounded buffers and pipelines
- [ConcurrentHashMap (Java SE)](https://docs.oracle.com/en/java/javase/17/docs/api/java.base/java/util/concurrent/ConcurrentHashMap.html) — concurrent collection default in Java LLD
- [Managed threading best practices (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/standard/threading/managed-threading-best-practices) — .NET concurrency guidance

## Practice prompts

1. Thread-safe ticket counter with max capacity  
2. Rate limiter used by N threads  
3. Bounded buffer with wait/notify or Condition
