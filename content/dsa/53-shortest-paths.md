---
id: dsa-shortest-paths
title: Shortest Paths — Bellman-Ford and Floyd-Warshall
track: dsa
module: "06 Graphs and Search"
order: 53
languages: [java, csharp]
summary: Complete the shortest-path toolkit beyond Dijkstra — negative edges, detection, and all-pairs.
---

## Why this matters

Dijkstra needs non-negative weights. Real routing and arbitrage-style problems need Bellman-Ford; all-pairs dense graphs use Floyd-Warshall. Together with BFS (unweighted) and Dijkstra, this is the full interview set.

## Definitions

- **Shortest path tree:** Edges used by best paths from a source (or between all pairs).
- **Relaxation:** If `dist[v] > dist[u] + w(u,v)`, improve `dist[v]`.
- **Bellman-Ford:** \(|V|-1\) rounds of relaxing *all* edges; handles negative weights; extra round detects negative cycles.
- **Floyd-Warshall:** DP over intermediate vertices for all-pairs shortest paths in \(O(V^3)\).
- **Negative cycle:** A cycle with total weight &lt; 0 — distances unbounded if reachable.
- **Dijkstra reminder:** Priority-queue algorithm for non-negative weights — \(O((V+E)\log V)\) typical.

## Algorithm cheat sheet

| Algorithm | Weights | From | Time (typical) |
|-----------|---------|------|----------------|
| BFS | Unit / unweighted | 1 source | \(O(V+E)\) |
| Dijkstra | ≥ 0 | 1 source | \(O((V+E)\log V)\) |
| Bellman-Ford | Any (detect neg cycle) | 1 source | \(O(VE)\) |
| Floyd-Warshall | Any (detect neg cycle) | All pairs | \(O(V^3)\) |

## Worked example 1 — Bellman-Ford

```java
int[] bellmanFord(int n, int[][] edges, int src) {
  int INF = Integer.MAX_VALUE / 4;
  int[] dist = new int[n];
  Arrays.fill(dist, INF);
  dist[src] = 0;
  for (int i = 0; i < n - 1; i++) {
    boolean changed = false;
    for (int[] e : edges) {
      int u = e[0], v = e[1], w = e[2];
      if (dist[u] < INF && dist[u] + w < dist[v]) {
        dist[v] = dist[u] + w; changed = true;
      }
    }
    if (!changed) break;
  }
  for (int[] e : edges) {
    int u = e[0], v = e[1], w = e[2];
    if (dist[u] < INF && dist[u] + w < dist[v])
      throw new IllegalStateException("negative cycle");
  }
  return dist;
}
```

```csharp
int[] BellmanFord(int n, int[][] edges, int src) {
  const int INF = int.MaxValue / 4;
  var dist = Enumerable.Repeat(INF, n).ToArray();
  dist[src] = 0;
  for (int i = 0; i < n - 1; i++) {
    bool changed = false;
    foreach (var e in edges) {
      int u = e[0], v = e[1], w = e[2];
      if (dist[u] < INF && dist[u] + w < dist[v]) {
        dist[v] = dist[u] + w; changed = true;
      }
    }
    if (!changed) break;
  }
  foreach (var e in edges) {
    int u = e[0], v = e[1], w = e[2];
    if (dist[u] < INF && dist[u] + w < dist[v])
      throw new InvalidOperationException("negative cycle");
  }
  return dist;
}
```

## Worked example 2 — Floyd-Warshall

```java
void floyd(int[][] d) {
  int n = d.length;
  for (int k = 0; k < n; k++)
    for (int i = 0; i < n; i++)
      for (int j = 0; j < n; j++)
        if (d[i][k] + d[k][j] < d[i][j])
          d[i][j] = d[i][k] + d[k][j];
  // negative cycle if any d[i][i] < 0
}
```

```csharp
void Floyd(int[][] d) {
  int n = d.Length;
  for (int k = 0; k < n; k++)
    for (int i = 0; i < n; i++)
      for (int j = 0; j < n; j++)
        if (d[i][k] + d[k][j] < d[i][j])
          d[i][j] = d[i][k] + d[k][j];
}
```

Initialize off-edges to a large INF; `d[i][i]=0`.

## Interview Q&A

- **Q:** When Bellman-Ford over Dijkstra?  
  **A:** Negative edge weights, or when you must detect negative cycles.
- **Q:** Floyd vs V× Dijkstra?  
  **A:** Floyd simpler for dense small \(V\); Dijkstra×V better for sparse if weights ≥ 0.
- **Q:** Recover path?  
  **A:** Keep `parent[v]` on relaxation (single-source) or `next[i][j]` for Floyd.

## Pitfalls

- INF overflow when adding weights — use `INF/4`  
- Forgetting the Nth iteration for cycle detection  
- Running Dijkstra with negatives anyway

## 60-second answer

“BFS for unweighted, Dijkstra for non-negative, Bellman-Ford when negatives or cycle detection matter, Floyd-Warshall for all-pairs on small dense graphs. All are built on edge relaxation.”

## Further study

- [Bellman–Ford algorithm](https://en.wikipedia.org/wiki/Bellman%E2%80%93Ford_algorithm)
- [Floyd–Warshall algorithm](https://en.wikipedia.org/wiki/Floyd%E2%80%93Warshall_algorithm)
- [Dijkstra’s algorithm](https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm)

## Practice prompts

1. Detect a negative cycle reachable from the source  
2. All-pairs distances on a 4-node weighted digraph by hand  
3. Compare runtimes for \(V=1000, E=2000\)
