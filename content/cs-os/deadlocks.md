---
id: os-deadlocks
title: Locks and Deadlocks
track: cs-os
module: "01 Concurrency"
order: 2
languages: [java, csharp]
summary: Mutexes, deadlock conditions, lock ordering, and DB deadlocks you’ll see in prod.
---

## Why this matters

Deadlocks show up in code **and** databases. Seniors should prevent them with ordering and detect them with logs/metrics.

## Definitions

- **Mutex / lock:** Sync primitive allowing only one thread into a critical section protecting shared state.
- **Deadlock:** Permanent wait cycle — each party holds a resource another needs, so none can proceed.
- **Coffman conditions:** Four necessary conditions: mutual exclusion, hold-and-wait, no preemption, circular wait.
- **Lock ordering:** Acquire multiple locks in one global order to break circular wait (primary prevention tactic).
- **Livelock:** Parties keep reacting/changing state but still make no useful progress.
- **Starvation:** A thread repeatedly loses scheduling/lock acquisition and never runs its critical work.
- **Reentrant lock:** Same thread may acquire again without self-deadlock (ownership/count tracked).
- **Database deadlock:** Transactions lock rows in opposite orders; engine aborts one — app must retry.

## Four Coffman conditions

1. Mutual exclusion  
2. Hold and wait  
3. No preemption  
4. Circular wait  

Break any one — usually **lock ordering** or try-lock + backoff.

## Worked example — ordered locks

```java
void transfer(Account a, Account b, int amt) {
  Account first = a.id < b.id ? a : b;
  Account second = a.id < b.id ? b : a;
  synchronized (first) {
    synchronized (second) {
      if (a.balance < amt) throw new IllegalStateException("funds");
      a.balance -= amt;
      b.balance += amt;
    }
  }
}
```

```csharp
void Transfer(Account a, Account b, int amt) {
  var first = a.Id < b.Id ? a : b;
  var second = a.Id < b.Id ? b : a;
  lock (first) {
    lock (second) {
      if (a.Balance < amt) throw new InvalidOperationException("funds");
      a.Balance -= amt;
      b.Balance += amt;
    }
  }
}
```

Without ordering, `A→B` and `B→A` can circular-wait.

## Other sync primitives (name-drop correctly)

- `ReentrantLock` / `Monitor`  
- `ReadWriteLock` / `ReaderWriterLockSlim`  
- Semaphores for pools  
- Concurrent collections to avoid explicit locks

## DB deadlocks

Two transactions update rows in opposite order → engine detects and aborts one.  
**App fix:** consistent lock order, shorter tx, retry on deadlock error.

## Livelock vs deadlock

Livelock: parties keep changing state but make no progress (always yielding).  
Starvation: a thread never gets the lock.

## Interview Q&A

- **Q:** How do you diagnose?
  **A:** Thread dumps (JVM), concurrency dumps/.NET dumps; DB deadlock graphs.
- **Q:** Are fine-grained locks always better?
  **A:** Less contention but more deadlock risk and complexity; start coarse then refine with evidence.
- **Q:** Actor/message models?
  **A:** Avoid shared mutable state — serialize access via a queue.

## Pitfalls

- Locking on `this` of a public object (external code can contend)  
- Calling unknown code while holding a lock (re-entrancy / deadlock)  
- Forgotten unlock in non-structure `lock` APIs

## 60-second answer

“Deadlocks need circular wait. I prevent them with global lock ordering, keep critical sections tiny, and retry on DB deadlocks. For many cases I’d rather use concurrent collections or single-writer queues than nested locks.”

## Further study

- [Deadlock (Wikipedia)](https://en.wikipedia.org/wiki/Deadlock) — Coffman conditions and classic prevention strategies
- [java.util.concurrent.locks](https://docs.oracle.com/en/java/javase/21/docs/api/java.base/java/util/concurrent/locks/package-summary.html) — Lock / ReentrantLock APIs used in production Java
- [PostgreSQL explicit locking](https://www.postgresql.org/docs/current/explicit-locking.html) — row locks and deadlock realities in databases
- [SQL Server locking guide](https://learn.microsoft.com/en-us/sql/relational-databases/sql-server-transaction-locking-and-row-versioning-guide) — lock types and deadlock detection in SQL Server

## Practice prompts

1. Find the deadlock in a dining philosophers sketch  
2. Design inventory reservation without deadlocks  
3. Explain why `SELECT FOR UPDATE` order matters
