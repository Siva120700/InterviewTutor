---
id: dsa-graph-representations
title: Graph Representations — Lists and Matrices
track: dsa
module: "06 Graphs and Search"
order: 49
languages: [java, csharp]
summary: Adjacency lists vs matrices, directed vs undirected edges, and how representation choice drives algorithm cost.
---

## Why this matters

Every graph algorithm starts from a representation. Wrong choice wastes memory or makes neighbor scans O(n) when they should be O(degree).

## Definitions

- **Graph \(G=(V,E)\):** Vertices (nodes) and edges (links), optionally weighted.
- **Directed graph:** Each edge \(u \rightarrow v\) is one-way.
- **Undirected graph:** Edge \(\{u,v\}\) is bidirectional (store both directions in an adj list).
- **Adjacency list:** Array/map of neighbor lists — space \(O(V+E)\), iterate neighbors in \(O(\deg(u))\).
- **Adjacency matrix:** \(V \times V\) grid; `M[u][v]` is presence/weight — space \(O(V^2)\), edge query \(O(1)\).
- **Weighted edge:** Stores a cost/length used by shortest paths and MSTs.
- **Degree:** Number of incident edges (out-degree / in-degree when directed).

## Concept

```text
Undirected: 0—1—2          Directed: 0→1→2
            |                    ↘
            3                     3

Adj list: 0: [1,3]         0: [1,3]
          1: [0,2]         1: [2]
          2: [1]           2: []
          3: [0]           3: []

Adj matrix (undirected):
    0 1 2 3
  0 0 1 0 1
  1 1 0 1 0
  2 0 1 0 0
  3 1 0 0 0
```

## Worked example 1 — Build undirected list

```java
List<Integer>[] buildUndirected(int n, int[][] edges) {
  List<Integer>[] g = new List[n];
  Arrays.setAll(g, i -> new ArrayList<>());
  for (int[] e : edges) {
    g[e[0]].add(e[1]);
    g[e[1]].add(e[0]);
  }
  return g;
}
```

```csharp
List<int>[] BuildUndirected(int n, int[][] edges) {
  var g = Enumerable.Range(0, n).Select(_ => new List<int>()).ToArray();
  foreach (var e in edges) {
    g[e[0]].Add(e[1]);
    g[e[1]].Add(e[0]);
  }
  return g;
}
```

## Worked example 2 — Weighted directed matrix

```java
int[][] buildMatrix(int n, int[][] edges) {
  int[][] m = new int[n][n];
  for (int[] row : m) Arrays.fill(row, Integer.MAX_VALUE / 4);
  for (int i = 0; i < n; i++) m[i][i] = 0;
  for (int[] e : edges) m[e[0]][e[1]] = e[2]; // u,v,w
  return m;
}
```

```csharp
int[][] BuildMatrix(int n, int[][] edges) {
  var m = new int[n][];
  for (int i = 0; i < n; i++) {
    m[i] = Enumerable.Repeat(int.MaxValue / 4, n).ToArray();
    m[i][i] = 0;
  }
  foreach (var e in edges) m[e[0]][e[1]] = e[2];
  return m;
}
```

## When to pick which

| Need | Prefer |
|------|--------|
| Sparse social/road graphs | Adjacency list |
| Dense / Floyd-Warshall | Adjacency matrix |
| Fast “is edge u–v?” | Matrix or hash-set of edges |
| BFS/DFS/Dijkstra | List + weights on edges |

## Interview Q&A

- **Q:** Space for undirected list?  
  **A:** Each undirected edge stored twice ⇒ \(O(V+E)\).
- **Q:** Self-loops / multis?  
  **A:** Clarify; matrix diagonal or multimap lists.
- **Q:** 1-indexed input?  
  **A:** Convert or allocate `n+1` — state your convention.

## Pitfalls

- Forgetting reverse edge in undirected builds  
- Using `int` matrix with 0 both as “no edge” and weight 0  
- Iterating matrix rows when graph is sparse (\(O(V^2)\) BFS)

## 60-second answer

“I store sparse graphs as adjacency lists and dense ones as matrices. Undirected edges are recorded both ways. Algorithms like BFS/DFS walk neighbor lists; Floyd wants a matrix.”

## Further study

- [Adjacency list](https://en.wikipedia.org/wiki/Adjacency_list)
- [Adjacency matrix](https://en.wikipedia.org/wiki/Adjacency_matrix)
- [Directed graph](https://en.wikipedia.org/wiki/Directed_graph)

## Practice prompts

1. Convert list → matrix and back  
2. Compute in-degree / out-degree arrays from a directed edge list  
3. Detect if an undirected graph is simple (no self-loops/multis)
