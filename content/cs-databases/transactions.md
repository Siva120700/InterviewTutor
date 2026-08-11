---
id: db-transactions
title: Transactions and Isolation
track: cs-databases
module: "02 Correctness"
order: 10
languages: [java, csharp]
summary: ACID, isolation levels, anomalies, and practical choices for Spring / EF Core apps.
---

## Why this matters

Senior interviews probe whether you can prevent lost updates and explain isolation without hand-waving “we use SERIALIZABLE everywhere”.

## Definitions

- **ACID:** Transaction guarantees — Atomic (all-or-nothing), Consistent (constraints hold), Isolated (controlled concurrency), Durable (survives crash after commit).
- **Isolation level:** How much concurrent work one transaction can observe, and which anomalies (dirty/non-repeatable/phantom) it still allows.
- **Dirty read:** Reading another transaction’s **uncommitted** write.
- **Non-repeatable read:** Same row read twice in one tx yields different values because another commit changed it.
- **Phantom read:** Re-query in the same tx returns new rows matching the predicate (another tx inserted them).
- **Lost update:** Two concurrent read-modify-write paths overwrite each other so one update disappears.
- **Optimistic concurrency:** Detect conflicts with version/rowversion at commit time and retry — no early lock.
- **Pessimistic locking:** Lock rows early (`SELECT … FOR UPDATE`) so conflicting writers wait instead of racing.

## ACID (crisp)

- **Atomicity** — all or nothing  
- **Consistency** — constraints hold after commit  
- **Isolation** — concurrent tx don’t step on each other beyond allowed anomalies  
- **Durability** — committed data survives crashes (WAL/fsync story)

## Anomalies

| Anomaly | What you see |
|---------|----------------|
| Dirty read | Read uncommitted data |
| Non-repeatable read | Same row changes mid-tx |
| Phantom | New matching rows appear mid-tx |
| Lost update | Two read-modify-writes overwrite |

## Isolation levels (practical)

| Level | Typical use |
|-------|-------------|
| Read Uncommitted | Rare |
| Read Committed | Default in Postgres/Oracle-ish apps |
| Repeatable Read | MySQL default; stable read set |
| Snapshot / MVCC | Concurrent readers without blocking |
| Serializable | Critical money invariants when needed |

## Worked example — transfer

```java
@Transactional(isolation = Isolation.READ_COMMITTED)
public void transfer(long fromId, long toId, long cents) {
  Account from = accounts.lockById(fromId); // SELECT … FOR UPDATE
  Account to = accounts.lockById(toId);
  if (from.balance < cents) throw new InsufficientFunds();
  from.balance -= cents;
  to.balance += cents;
}
```

```csharp
public async Task TransferAsync(long fromId, long toId, long cents) {
  await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
  var from = await db.Accounts.FromSql($"SELECT * FROM accounts WHERE id={fromId} FOR UPDATE")
    .SingleAsync();
  var to = await db.Accounts.FromSql($"SELECT * FROM accounts WHERE id={toId} FOR UPDATE")
    .SingleAsync();
  if (from.Balance < cents) throw new InvalidOperationException("funds");
  from.Balance -= cents;
  to.Balance += cents;
  await db.SaveChangesAsync();
  await tx.CommitAsync();
}
```

**Say:** isolation alone may not fix lost updates — use row locks, version columns (`xmin`/optimistic concurrency), or single SQL `UPDATE balance = balance - ?`.

## Optimistic concurrency

```csharp
public class Order {
  public int Id { get; set; }
  public byte[] RowVersion { get; set; } = Array.Empty<byte>(); // EF concurrency token
}
```

Conflict → catch and retry/merge.

## Interview Q&A

- **Q:** Postgres default?
  **A:** Read Committed; also strong MVCC story. Serializable is SSI.
- **Q:** When SERIALIZABLE?
  **A:** Hard invariants hard to express with locks; be ready for retries on conflict.
- **Q:** Long transactions?
  **A:** Hold locks/versions longer → contention; keep tx short; move slow I/O out.

## Pitfalls

- Catching exceptions inside `@Transactional` and swallowing rollback  
- Nested tx assumptions (propagation)  
- Assuming MySQL RR equals true serializability for all phantoms

## 60-second answer

“I pick the weakest isolation that prevents the anomalies I care about — often RC plus explicit `FOR UPDATE` or optimistic versions for read-modify-write. I keep transactions short and push side effects (email) out via outbox.”

## Further study

- [Transaction isolation](https://www.postgresql.org/docs/current/transaction-iso.html) — isolation levels and anomalies in Postgres terms
- [Concurrency control (MVCC)](https://www.postgresql.org/docs/current/mvcc-intro.html) — how readers and writers coexist without dirty reads by default
- [Explicit locking](https://www.postgresql.org/docs/current/explicit-locking.html) — `FOR UPDATE` and when locks beat isolation alone
- [EF Core concurrency tokens](https://learn.microsoft.com/en-us/ef/core/saving/concurrency) — optimistic concurrency patterns in .NET
- [Spring transaction management](https://docs.spring.io/spring-framework/reference/data-access/transaction.html) — `@Transactional` semantics interviewers expect

## Practice prompts

1. Explain a non-repeatable read with SQL timeline  
2. Design idempotent payment capture  
3. Compare pessimistic vs optimistic locking for inventory
