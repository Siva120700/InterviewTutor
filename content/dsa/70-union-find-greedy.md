---
id: dsa-union-find-greedy
title: Union-Find and Greedy
track: dsa
module: "08 Advanced Tools"
order: 70
languages: [java, csharp]
summary: Disjoint sets for connectivity, Kruskal mindset, and classic greedy proofs at interview depth.
---

## Why this matters

Union-Find solves dynamic connectivity. Greedy wins when local optimal choices extend to global (interval scheduling, Huffman intuition).

## Definitions

- **Union-Find (DSU):** A disjoint-set structure that tracks connected components with near-O(1) `find` and `union` (with optimizations).
- **Find:** Returning the representative (root) of the set containing an element, often with path compression.
- **Union:** Merging two sets if their roots differ; returns false when already connected (cycle / redundant edge).
- **Path compression:** Flattening parent pointers during `find` so future lookups are faster.
- **Union by rank/size:** Attaching the smaller tree under the larger to keep DSU trees shallow.
- **Greedy algorithm:** Making the locally best choice at each step when a proof structure (exchange/stay-ahead) guarantees a global optimum.
- **Interval scheduling:** A classic greedy — sort by end time and repeatedly take the next non-overlapping interval.

## Union-Find

```java
class DSU {
  int[] p, r;
  DSU(int n) { p = new int[n]; r = new int[n]; for (int i = 0; i < n; i++) p[i] = i; }
  int find(int x) { return p[x] == x ? x : (p[x] = find(p[x])); }
  boolean union(int a, int b) {
    int ra = find(a), rb = find(b);
    if (ra == rb) return false;
    if (r[ra] < r[rb]) p[ra] = rb;
    else if (r[ra] > r[rb]) p[rb] = ra;
    else { p[rb] = ra; r[ra]++; }
    return true;
  }
}
```

```csharp
class Dsu {
  readonly int[] p, r;
  public Dsu(int n) { p = Enumerable.Range(0, n).ToArray(); r = new int[n]; }
  public int Find(int x) => p[x] == x ? x : p[x] = Find(p[x]);
  public bool Union(int a, int b) {
    int ra = Find(a), rb = Find(b);
    if (ra == rb) return false;
    if (r[ra] < r[rb]) p[ra] = rb;
    else if (r[ra] > r[rb]) p[rb] = ra;
    else { p[rb] = ra; r[ra]++; }
    return true;
  }
}
```

## Greedy — interval scheduling

Sort by end time; take next compatible — classic proof via exchange.

```java
int eraseOverlap(int[][] intervals) {
  Arrays.sort(intervals, Comparator.comparingInt(a -> a[1]));
  int end = Integer.MIN_VALUE, keep = 0;
  for (int[] it : intervals)
    if (it[0] >= end) { keep++; end = it[1]; }
  return intervals.length - keep; // removals
}
```

## Interview Q&A

- **Q:** Path compression?
  **A:** Flattens trees; with union-by-rank ≈ α(n) inverse Ackermann.
- **Q:** Prove greedy?
  **A:** Exchange/stay-ahead arguments — sketch, don’t formalize fully unless asked.

## Pitfalls

- Using UF on directed graphs for topo (wrong tool)  
- Greedy without monotonic structure

## 60-second answer

“Union-Find merges components for connectivity and MST-style edges. Greedy picks the locally best choice when a proof structure exists — like earliest-finishing intervals.”

## Further study

- [Disjoint-set data structure (Wikipedia)](https://en.wikipedia.org/wiki/Disjoint-set_data_structure) — Union-Find with path compression and union by rank
- [Greedy algorithm (Wikipedia)](https://en.wikipedia.org/wiki/Greedy_algorithm) — when local choices yield global optima
- [Kruskal's algorithm (Wikipedia)](https://en.wikipedia.org/wiki/Kruskal%27s_algorithm) — MST using Union-Find for cycle checks
- [Activity selection problem (Wikipedia)](https://en.wikipedia.org/wiki/Activity_selection_problem) — interval scheduling greedy proof pattern

## Practice prompts

1. Number of provinces  
2. Accounts merge  
3. Jump game / gas station
