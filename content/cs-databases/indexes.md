---
id: db-indexes
title: Indexes That Matter
track: cs-databases
module: "01 Storage"
order: 1
languages: [java, csharp]
summary: B-trees, selectivity, composites, covering indexes, and when indexes hurt writes.
---

## Why this matters

Most “the API is slow” stories end in a missing or wrong index. Interviewers want you to reason about **access paths**, not recite “indexes make reads fast”.

## Definitions

- **Index:** Separate sorted structure (usually B-tree) mapping key values → row locations so lookups/ranges avoid full table scans.
- **B-tree index:** Balanced tree supporting equality **and** range scans; the default general-purpose index in most RDBMSs.
- **Selectivity:** How strongly a column/predicate filters rows; high selectivity makes an index more attractive to the planner.
- **Composite index:** Multi-column index; **leftmost-prefix** order decides which `WHERE`/`ORDER BY` shapes it can serve.
- **Covering index:** Index containing every column the query needs so the engine can answer without heap/table visits.
- **Clustered index:** Index that defines physical/primary row order (e.g. InnoDB PK); table data follows this order.
- **Secondary index:** Non-clustered index pointing into the heap/PK; every write must maintain it.
- **EXPLAIN / EXPLAIN ANALYZE:** Planner estimate (and measured runtime) showing the chosen access path — verify, don’t guess.

## Concept

An index is a separate sorted structure (usually B-tree) mapping key → row location (or primary key).

**Trade-off:** faster lookups/ranges vs slower writes + storage + maintenance (VACUUM/reorg).

```mermaid
flowchart LR
  Query --> Planner
  Planner --> Idx[BTree_Index]
  Idx --> Heap[Table_Rows]
```

## Selectivity

Good index keys filter strongly (`user_id`, `email`).  
Bad: boolean `is_active` alone on a 50/50 split — planner may skip the index.

## Composite indexes

Order matters: `(user_id, created_at)` helps:
- `WHERE user_id = ?`
- `WHERE user_id = ? ORDER BY created_at`
- `WHERE user_id = ? AND created_at > ?`

Does **not** help well: `WHERE created_at > ?` alone (leftmost prefix rule).

```sql
CREATE INDEX ix_orders_user_created ON orders(user_id, created_at);
```

## Covering indexes

Include columns so the query never visits the heap:

```sql
-- PostgreSQL
CREATE INDEX ix_orders_cover ON orders(user_id) INCLUDE (status, total);
```

## Worked example — ORM query that needs an index

```java
// Spring Data
List<Order> findByUserIdAndCreatedAtAfterOrderByCreatedAtDesc(long userId, Instant after);
// → wants (user_id, created_at)
```

```csharp
await db.Orders.AsNoTracking()
  .Where(o => o.UserId == userId && o.CreatedAt > after)
  .OrderByDescending(o => o.CreatedAt)
  .Take(50)
  .ToListAsync();
```

## When indexes hurt

- High-write tables with many secondary indexes  
- Random UUIDs as clustering keys (page splits) — discuss carefully per engine  
- Over-indexing “just in case”

## EXPLAIN mindset

Look for Seq Scan on large tables, high cost, or Nested Loop explosions. Mention `EXPLAIN ANALYZE` in Postgres.

## Interview Q&A

- **Q:** Clustered vs secondary?
  **A:** Clustered/primary determines row order (InnoDB PK, SQL Server clustered). Secondary points into heap/PK.
- **Q:** Hash index?
  **A:** Equality only; less common as general default than B-tree.
- **Q:** How many indexes on a table?
  **A:** As few as cover real query shapes; measure write impact.

## Pitfalls

- Function on column `WHERE LOWER(email)=` without matching expression index  
- Leading wildcard `LIKE '%foo'` can’t use normal B-tree well  
- Assuming index exists because “we have a PK”

## 60-second answer

“I’d find the query shape, check selectivity, and design a composite matching filter+sort order. I’d consider a covering index for hot read paths and always weigh write amplification. I’d verify with EXPLAIN ANALYZE.”

## Further study

- [PostgreSQL indexes](https://www.postgresql.org/docs/current/indexes.html) — official overview of index types and when the planner uses them
- [Multicolumn indexes](https://www.postgresql.org/docs/current/indexes-multicolumn.html) — leftmost-prefix rules for composite keys
- [Using EXPLAIN](https://www.postgresql.org/docs/current/using-explain.html) — how to read plans and verify index usage
- [Index-only scans](https://www.postgresql.org/docs/current/indexes-index-only-scans.html) — covering indexes and heap visits

## Practice prompts

1. Design indexes for a feed query `(follower lookups + time)`  
2. Explain why `OR` across columns often defeats one composite  
3. Choose PK for a time-series table
