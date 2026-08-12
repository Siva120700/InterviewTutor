---
id: dsa-mst
title: Minimum Spanning Trees — Prim and Kruskal
track: dsa
module: "06 Graphs and Search"
order: 54
languages: [java, csharp]
summary: Cycle-free minimum-weight networks via Prim’s growing tree and Kruskal’s sorted edges + Union-Find.
---

## Why this matters

MSTs appear in network design, clustering intuition, and as the classic Union-Find application (Kruskal). Interviews expect both Prim and Kruskal sketches.

## Definitions

- **Spanning tree:** A subset of edges connecting all vertices with no cycles (\(|E|=|V|-1\)).
- **MST:** A spanning tree of minimum total edge weight (undirected, connected, weighted graphs).
- **Cut property:** For any cut, a lightest edge crossing the cut is safe for some MST.
- **Prim:** Grow a tree from a start vertex, always adding the cheapest edge leaving the tree (heap-optimized).
- **Kruskal:** Sort edges by weight; add if it joins different components (Union-Find).
- **Cycle check:** Kruskal rejects edges whose endpoints share a DSU root.

## Concept

```text
Weights:                 MST edges (example):
  0--1 (1)                 0--1 , 1--2 , 0--3
  |\/| (2,3,4)
  3--2 (5)
```

## Worked example 1 — Kruskal

```java
int kruskal(int n, int[][] edges) {
  Arrays.sort(edges, Comparator.comparingInt(e -> e[2]));
  DSU dsu = new DSU(n);
  int cost = 0, used = 0;
  for (int[] e : edges) {
    if (dsu.union(e[0], e[1])) {
      cost += e[2];
      if (++used == n - 1) break;
    }
  }
  return used == n - 1 ? cost : -1; // disconnected
}
```

```csharp
int Kruskal(int n, int[][] edges) {
  Array.Sort(edges, (a, b) => a[2].CompareTo(b[2]));
  var dsu = new Dsu(n);
  int cost = 0, used = 0;
  foreach (var e in edges) {
    if (dsu.Union(e[0], e[1])) {
      cost += e[2];
      if (++used == n - 1) break;
    }
  }
  return used == n - 1 ? cost : -1;
}
```

(Reuse the DSU from the Union-Find lesson.)

## Worked example 2 — Prim (dense O(V²) form)

```java
int prim(int[][] w) { // w[u][v] weight or INF
  int n = w.length;
  boolean[] in = new boolean[n];
  int[] key = new int[n];
  Arrays.fill(key, Integer.MAX_VALUE / 4);
  key[0] = 0;
  int cost = 0;
  for (int it = 0; it < n; it++) {
    int u = -1;
    for (int i = 0; i < n; i++)
      if (!in[i] && (u < 0 || key[i] < key[u])) u = i;
    in[u] = true;
    cost += key[u] == Integer.MAX_VALUE / 4 ? 0 : key[u];
    for (int v = 0; v < n; v++)
      if (!in[v] && w[u][v] < key[v]) key[v] = w[u][v];
  }
  return cost;
}
```

```csharp
int Prim(int[][] w) {
  int n = w.Length;
  var inn = new bool[n];
  var key = Enumerable.Repeat(int.MaxValue / 4, n).ToArray();
  key[0] = 0;
  int cost = 0;
  for (int it = 0; it < n; it++) {
    int u = -1;
    for (int i = 0; i < n; i++)
      if (!inn[i] && (u < 0 || key[i] < key[u])) u = i;
    inn[u] = true;
    if (key[u] < int.MaxValue / 4) cost += key[u];
    for (int v = 0; v < n; v++)
      if (!inn[v] && w[u][v] < key[v]) key[v] = w[u][v];
  }
  return cost;
}
```

Heap Prim: store `(weight, to)` leaving the tree — \(O(E \log V)\).

## Prim vs Kruskal

| | Prim | Kruskal |
|---|------|---------|
| Grows | One tree | Forest → tree |
| Best for | Dense (matrix) | Sparse (edge list) |
| Needs | Priority queue / matrix | Sort + DSU |

## Interview Q&A

- **Q:** Directed MST?  
  **A:** Different problem (arborescence / Edmonds) — clarify undirected.
- **Q:** Unique MST?  
  **A:** Not always; equal weights can yield multiple MSTs with same cost.
- **Q:** Relation to clustering?  
  **A:** Dropping the \(k-1\) heaviest MST edges yields \(k\) clusters (intuition).

## Pitfalls

- Running MST algorithms on directed graphs without conversion  
- Forgetting graph might be disconnected  
- Sorting edges ascending but adding until `n` edges instead of `n-1`

## 60-second answer

“An MST connects all vertices with minimum total weight and no cycles. Kruskal sorts edges and uses Union-Find; Prim grows a tree by cheapest leaving edge. Cut property justifies both.”

## Further study

- [Minimum spanning tree](https://en.wikipedia.org/wiki/Minimum_spanning_tree)
- [Kruskal's algorithm](https://en.wikipedia.org/wiki/Kruskal%27s_algorithm)
- [Prim's algorithm](https://en.wikipedia.org/wiki/Prim%27s_algorithm)

## Practice prompts

1. Trace Kruskal on a 5-edge graph  
2. Implement heap-based Prim from an adjacency list  
3. Prove (sketch) why a lightest cut edge is safe
