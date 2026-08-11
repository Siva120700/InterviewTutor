---
id: dotnet-linq
title: LINQ and Collections
track: dotnet
module: "02 Data"
order: 10
languages: [csharp]
summary: BCL collections, LINQ deferred execution, IEnumerable vs IQueryable, and interview-ready query patterns.
---

## Why this matters

LINQ is everywhere in .NET interviews — and so are its footguns: multiple enumeration, accidental client evaluation, and picking the wrong collection. You need fluency plus judgment.

## Definitions

- **IEnumerable<T>:** In-memory sequence interface for LINQ to Objects; enumeration runs in-process (not SQL).
- **IQueryable<T>:** Expression-tree query usually translated by a provider (EF Core) into SQL — composition changes the query.
- **Deferred execution:** Building a LINQ query does not run it until enumeration or a materializing operator.
- **Materialization:** Forcing execution into a concrete result (`ToList`, `ToArray`, `First`, `Count`).
- **Multiple enumeration:** Re-enumerating a deferred query, which may re-run CPU work or hit the database again.
- **Dictionary<TKey,TValue>:** Average O(1) key lookup for associative data; keys need stable hash/equality.
- **Projection:** Selecting a smaller shape (`Select` to DTO/anonymous type) early to cut payload and enable SQL pushdown.

## Concept

### Core collections

| Type | Use |
|------|-----|
| `List<T>` | Default growable array |
| `Dictionary<TKey,TValue>` | Key lookup |
| `HashSet<T>` | Unique set membership |
| `Queue<T>` / `Stack<T>` | FIFO / LIFO |
| `LinkedList<T>` | Rarely; poor locality |
| `ConcurrentDictionary` | Concurrent map |
| `Immutable*` / `Frozen*` | Share-safe snapshots (FrozenDict .NET 8+) |

Complexity mental model matches Java: lists amortized O(1) append, dictionaries average O(1) lookup, sorted structures O(log n).

### LINQ pipeline

```text
Source → Where/Select/... (deferred) → ToList/First/... (execute)
```

LINQ to Objects operates on `IEnumerable<T>` in memory.  
EF Core uses `IQueryable<T>` — expression trees translated to SQL.

**Deferred execution:** defining a query is not running it. Enumeration (or `ToList`, `Count`, `First`) executes it.

```mermaid
flowchart LR
  Query[IQueryable] --> Provider[EF_Provider]
  Provider --> SQL
  SQL --> Rows
  Rows --> Entities
```

## Worked example 1 — Idiomatic LINQ

```csharp
var expensive = orders
    .Where(o => o.Total > 100)
    .OrderByDescending(o => o.Total)
    .Select(o => o.Sku)
    .Take(10)
    .ToList(); // materialize once
```

## Worked example 2 — Grouping and lookups

```csharp
var byCustomer = orders
    .GroupBy(o => o.CustomerId)
    .Select(g => new
    {
        CustomerId = g.Key,
        Total = g.Sum(x => x.Total),
        Count = g.Count()
    })
    .OrderByDescending(x => x.Total)
    .ToList();

Dictionary<string, Order> bySku = orders.ToDictionary(o => o.Sku);
ILookup<int, Order> lookup = orders.ToLookup(o => o.CustomerId);
```

## Worked example 3 — Deferred execution trap

```csharp
IEnumerable<Order> q = orders.Where(o => o.Total > 50);

Console.WriteLine(q.Count()); // enumerates
Console.WriteLine(q.Count()); // enumerates AGAIN

// If orders is a live EF IQueryable, that's two SQL round-trips.
var materialized = q.ToList(); // fix: materialize once when needed
```

Mutating the source while a deferred query is alive is another classic bug.

## Worked example 4 — IEnumerable vs IQueryable

```csharp
// Server-side (SQL) — good
var page = await db.Orders
    .AsNoTracking()
    .Where(o => o.Status == Status.Paid)
    .OrderByDescending(o => o.CreatedAt)
    .Select(o => new OrderListItem(o.Id, o.Sku, o.Total))
    .Skip(0).Take(20)
    .ToListAsync(ct);

// Dangerous: force client eval with unsupported ops / premature ToList
var bad = db.Orders.ToList().Where(o => ComplexFilter(o)); // pulls whole table
```

Project to DTOs early. Filter/sort/page **before** materializing.

## Worked example 5 — Dictionary patterns

```csharp
var counts = new Dictionary<string, int>();
foreach (var w in words)
{
    CollectionsMarshal.GetValueRefOrAddDefault(counts, w, out _)++;
    // or: counts[w] = counts.GetValueOrDefault(w) + 1;
}

if (counts.TryGetValue(key, out var n))
{
    // use n
}
```

Prefer `TryGetValue` over indexer + `ContainsKey` double lookup stories.

## Interview Q&A

- **Q:** Deferred execution — why care?  
  **A:** Work runs at enumeration; multiple enumerations can repeat cost or SQL; mutating sources yields surprises.
- **Q:** `IEnumerable` vs `IQueryable`?  
  **A:** In-memory iteration vs expression tree for providers (EF). Mixing them carelessly causes client evaluation.
- **Q:** `First` vs `FirstOrDefault` vs `Single`?  
  **A:** `First` expects ≥1; `Single` expects exactly one (throws otherwise); `*OrDefault` softens absence.
- **Q:** When `ToList`?  
  **A:** When you need a stable snapshot, multiple passes, or to end a DB query deliberately.
- **Q:** LINQ vs `for` loops?  
  **A:** LINQ for clarity; loops when you need complex mutation, early exit, or measured perf.
- **Q:** `SelectMany`?  
  **A:** Flatten nested sequences — LINQ’s `flatMap`.

## Pitfalls

- Multiple enumeration of expensive queries  
- `ToList()` too early → huge memory / lost SQL pushdown  
- `ToList()` too late → repeated DB hits  
- Using `Count()` when `Any()` suffices  
- Assuming `Dictionary` order (insertion order is observed now, but don’t rely for sorting needs — sort explicitly)  
- Closing over mutable loop variables in older patterns (less common now)  
- N+1 from lazy navigation after a LINQ query materializes entities

## 60-second answer

“I default to List and Dictionary, write LINQ as readable pipelines, and materialize once when needed. I know deferred execution and treat IQueryable as SQL until I ToList. I project early, page in the database, and use Any/TryGetValue idioms instead of wasteful scans.”

## Further study

- [LINQ in C#](https://learn.microsoft.com/en-us/dotnet/csharp/linq/) — query vs method syntax and composition model
- [Language Integrated Query (LINQ)](https://learn.microsoft.com/en-us/dotnet/standard/linq/) — deferred execution and provider concepts
- [Generic collections](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic) — `List`, `Dictionary`, and friends
- [IQueryable vs IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1) — when queries become SQL

## Practice prompts

1. Find hidden multiple enumeration in a service method and fix it  
2. Rewrite nested loops into `GroupBy` + `Select` without changing results  
3. Explain a case where EF client evaluation hurt production  
4. Implement top-K frequent words with `Dictionary` + heap / sort
