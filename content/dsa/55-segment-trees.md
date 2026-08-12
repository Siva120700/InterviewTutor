---
id: dsa-segment-trees
title: Segment Trees
track: dsa
module: "08 Advanced Tools"
order: 71
languages: [java, csharp]
summary: Array partitioning for range queries and point/range updates in O(log n).
---

## Why this matters

When you need many range-sum / range-min queries with updates, prefix sums alone aren’t enough. Segment trees (and Fenwick) are the interview standard for \(O(\log n)\) dual operations.

## Definitions

- **Segment tree:** A binary tree over an array where each node stores an aggregate (sum, min, gcd, …) for a contiguous segment.
- **Leaf:** Corresponds to a single array index.
- **Internal node:** Combines left/right child aggregates for \([L,R] = [L,M] \cup [M+1,R]\).
- **Point update:** Change one index and recompute ancestors.
- **Range query:** Merge O(log n) disjoint node segments that exactly cover \([L,R]\).
- **Lazy propagation:** Deferred range updates stored at nodes (advanced follow-up).

## Concept

```text
Array indices:     0   1   2   3
Tree (sums):          [0,3]
                    /       \
                [0,1]       [2,3]
               /    \       /    \
            [0]    [1]   [2]    [3]
```

Build \(O(n)\), query/update \(O(\log n)\), memory ~ \(4n\).

## Worked example 1 — Sum segment tree

```java
class SegTree {
  final int n;
  final int[] t;
  SegTree(int[] a) {
    n = a.length; t = new int[4 * n];
    build(1, 0, n - 1, a);
  }
  void build(int v, int l, int r, int[] a) {
    if (l == r) { t[v] = a[l]; return; }
    int m = (l + r) >>> 1;
    build(v * 2, l, m, a);
    build(v * 2 + 1, m + 1, r, a);
    t[v] = t[v * 2] + t[v * 2 + 1];
  }
  void set(int v, int l, int r, int i, int val) {
    if (l == r) { t[v] = val; return; }
    int m = (l + r) >>> 1;
    if (i <= m) set(v * 2, l, m, i, val);
    else set(v * 2 + 1, m + 1, r, i, val);
    t[v] = t[v * 2] + t[v * 2 + 1];
  }
  int query(int v, int l, int r, int ql, int qr) {
    if (qr < l || r < ql) return 0;
    if (ql <= l && r <= qr) return t[v];
    int m = (l + r) >>> 1;
    return query(v * 2, l, m, ql, qr) + query(v * 2 + 1, m + 1, r, ql, qr);
  }
}
```

```csharp
class SegTree {
  readonly int n;
  readonly int[] t;
  public SegTree(int[] a) {
    n = a.Length; t = new int[4 * n];
    Build(1, 0, n - 1, a);
  }
  void Build(int v, int l, int r, int[] a) {
    if (l == r) { t[v] = a[l]; return; }
    int m = (l + r) / 2;
    Build(v * 2, l, m, a);
    Build(v * 2 + 1, m + 1, r, a);
    t[v] = t[v * 2] + t[v * 2 + 1];
  }
  public void Set(int i, int val) => Set(1, 0, n - 1, i, val);
  void Set(int v, int l, int r, int i, int val) {
    if (l == r) { t[v] = val; return; }
    int m = (l + r) / 2;
    if (i <= m) Set(v * 2, l, m, i, val);
    else Set(v * 2 + 1, m + 1, r, i, val);
    t[v] = t[v * 2] + t[v * 2 + 1];
  }
  public int Query(int ql, int qr) => Query(1, 0, n - 1, ql, qr);
  int Query(int v, int l, int r, int ql, int qr) {
    if (qr < l || r < ql) return 0;
    if (ql <= l && r <= qr) return t[v];
    int m = (l + r) / 2;
    return Query(v * 2, l, m, ql, qr) + Query(v * 2 + 1, m + 1, r, ql, qr);
  }
}
```

## Associative merges

Sum, min, max, gcd work if merge is associative. For non-invertible ops, prefer segment tree over Fenwick.

## Interview Q&A

- **Q:** Segment tree vs Fenwick?  
  **A:** Fenwick is simpler for prefix sums / point updates; segment tree handles richer queries and lazy range updates.
- **Q:** Why size `4n`?  
  **A:** Safe upper bound for the heap-style recursive layout.
- **Q:** Inclusive bounds?  
  **A:** Pick a convention and stick to it — off-by-one is the #1 bug.

## Pitfalls

- Identity for empty query wrong (0 for sum, +∞ for min)  
- Updating leaf but forgetting to push combine up  
- Using segment tree when a simple prefix sum suffices (no updates)

## 60-second answer

“A segment tree stores aggregates on dyadic ranges. Build in O(n); each query/update touches O(log n) nodes. I use it when I need flexible range queries with updates.”

## Further study

- [Segment tree](https://en.wikipedia.org/wiki/Segment_tree)
- CP-Algorithms: Segment tree

## Practice prompts

1. Range minimum query with point updates  
2. Count zeros in a range  
3. Sketch lazy add-on-range for sums
