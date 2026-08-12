---
id: dsa-sparse-table
title: Sparse Table
track: dsa
module: "08 Advanced Tools"
order: 77
languages: [java, csharp]
summary: Static RMQ in O(1) after O(n log n) preprocess using idempotent range merges.
---

## Why this matters

When the array never updates and you need many range-min (or gcd) queries, a sparse table is simpler and faster than a segment tree.

## Definitions

- **Sparse table:** `st[k][i]` = aggregate of the range starting at i of length \(2^k\).
- **Idempotent / overlap-friendly op:** `min`, `max`, `gcd` — overlapping ranges OK for O(1) queries.
- **Static structure:** No efficient point updates — rebuild if data changes.
- **Range min query (RMQ):** Classic sparse-table use case.

## Build and query (min)

```java
class SparseMin {
  final int[][] st;
  final int[] log;
  SparseMin(int[] a) {
    int n = a.length;
    log = new int[n + 1];
    for (int i = 2; i <= n; i++) log[i] = log[i / 2] + 1;
    int K = log[n] + 1;
    st = new int[K][n];
    System.arraycopy(a, 0, st[0], 0, n);
    for (int k = 1; k < K; k++)
      for (int i = 0; i + (1 << k) <= n; i++)
        st[k][i] = Math.min(st[k - 1][i], st[k - 1][i + (1 << (k - 1))]);
  }
  int query(int l, int r) { // inclusive
    int k = log[r - l + 1];
    return Math.min(st[k][l], st[k][r - (1 << k) + 1]);
  }
}
```

```csharp
class SparseMin {
  readonly int[][] st; readonly int[] log;
  public SparseMin(int[] a) {
    int n = a.Length;
    log = new int[n + 1];
    for (int i = 2; i <= n; i++) log[i] = log[i / 2] + 1;
    int K = log[n] + 1;
    st = new int[K][];
    st[0] = (int[])a.Clone();
    for (int k = 1; k < K; k++) {
      st[k] = new int[n];
      for (int i = 0; i + (1 << k) <= n; i++)
        st[k][i] = Math.Min(st[k - 1][i], st[k - 1][i + (1 << (k - 1))]);
    }
  }
  public int Query(int l, int r) {
    int k = log[r - l + 1];
    return Math.Min(st[k][l], st[k][r - (1 << k) + 1]);
  }
}
```

## vs Segment / Fenwick

| Need | Prefer |
|------|--------|
| Static RMQ/gcd | Sparse table |
| Point updates + range sum | Fenwick / segment |
| Lazy range updates | Segment tree |

## Interview Q&A

- **Q:** Why not for sums with O(1)?  
  **A:** Overlapping sum double-counts — need disjoint cover (segment) or prefix sums.
- **Q:** Memory?  
  **A:** \(O(n \log n)\).

## Pitfalls

- Inclusive vs exclusive bounds  
- Using sparse table when updates exist  
- `log` array off-by-one

## 60-second answer

“Sparse tables precompute power-of-two ranges. For min/gcd I answer any range in O(1) after O(n log n) build — perfect when the array is static.”

## Further study

- CP-Algorithms: Sparse Table  
- Segment tree / Fenwick lessons for dynamic cases

## Practice prompts

1. Range minimum queries offline  
2. Range GCD queries  
3. Explain why sum needs a different structure
