---
id: dsa-graphs-advanced
title: Graphs Advanced — Topo and Dijkstra
track: dsa
module: "06 Graphs and Search"
order: 52
languages: [java, csharp]
summary: Topological sort, cycle detection in directed graphs, and Dijkstra shortest paths.
---

## Why this matters

Course schedules, build systems, and routing use topo sort and Dijkstra — senior-ready graph topics.

## Definitions

- **Directed graph (DAG when acyclic):** A graph with one-way edges; a DAG admits a topological order.
- **Topological sort:** A linear ordering of nodes such that every directed edge u → v has u before v — valid only for DAGs.
- **Indegree:** The number of incoming edges to a node; Kahn’s algorithm repeatedly processes indegree-0 nodes.
- **Kahn’s algorithm:** BFS-style topo sort using a queue of zero-indegree nodes; finishing with fewer than n nodes means a cycle.
- **Dijkstra’s algorithm:** Computing shortest paths from a source with non-negative edge weights using a priority queue.
- **Relaxation:** Updating a neighbor’s best distance when a cheaper path through the current node is found (`dist[v] > dist[u] + w`).
- **Non-negative weights:** Dijkstra’s correctness requirement; negative edges need Bellman-Ford or other algorithms instead.

## Topological sort (Kahn)

```java
List<Integer> topo(int n, int[][] edges) {
  List<Integer>[] g = new List[n];
  Arrays.setAll(g, i -> new ArrayList<>());
  int[] indeg = new int[n];
  for (int[] e : edges) { g[e[0]].add(e[1]); indeg[e[1]]++; }
  Queue<Integer> q = new ArrayDeque<>();
  for (int i = 0; i < n; i++) if (indeg[i] == 0) q.add(i);
  List<Integer> order = new ArrayList<>();
  while (!q.isEmpty()) {
    int u = q.poll(); order.add(u);
    for (int v : g[u]) if (--indeg[v] == 0) q.add(v);
  }
  return order.size() == n ? order : List.of(); // cycle if incomplete
}
```

```csharp
List<int> Topo(int n, int[][] edges) {
  var g = Enumerable.Range(0, n).Select(_ => new List<int>()).ToArray();
  var indeg = new int[n];
  foreach (var e in edges) { g[e[0]].Add(e[1]); indeg[e[1]]++; }
  var q = new Queue<int>();
  for (int i = 0; i < n; i++) if (indeg[i] == 0) q.Enqueue(i);
  var order = new List<int>();
  while (q.Count > 0) {
    int u = q.Dequeue(); order.Add(u);
    foreach (var v in g[u]) if (--indeg[v] == 0) q.Enqueue(v);
  }
  return order.Count == n ? order : new List<int>();
}
```

## Dijkstra (non-negative weights)

```java
int[] dijkstra(int n, List<int[]>[] g, int src) {
  int[] dist = new int[n];
  Arrays.fill(dist, Integer.MAX_VALUE / 4);
  dist[src] = 0;
  PriorityQueue<int[]> pq = new PriorityQueue<>(Comparator.comparingInt(a -> a[0]));
  pq.add(new int[]{0, src}); // dist, node
  while (!pq.isEmpty()) {
    int[] cur = pq.poll();
    int d = cur[0], u = cur[1];
    if (d != dist[u]) continue;
    for (int[] e : g[u]) {
      int v = e[0], w = e[1];
      if (dist[v] > d + w) {
        dist[v] = d + w;
        pq.add(new int[]{dist[v], v});
      }
    }
  }
  return dist;
}
```

```csharp
int[] Dijkstra(int n, List<(int v, int w)>[] g, int src) {
  var dist = Enumerable.Repeat(int.MaxValue / 4, n).ToArray();
  dist[src] = 0;
  var pq = new PriorityQueue<int, int>();
  pq.Enqueue(src, 0);
  while (pq.Count > 0) {
    pq.TryDequeue(out int u, out int d);
    if (d != dist[u]) continue;
    foreach (var (v, w) in g[u]) {
      if (dist[v] > d + w) {
        dist[v] = d + w;
        pq.Enqueue(v, dist[v]);
      }
    }
  }
  return dist;
}
```

## Interview Q&A

- **Q:** Negative weights?
  **A:** Dijkstra fails; use Bellman-Ford / SPFA carefully.
- **Q:** Union-Find vs topo?
  **A:** UF for undirected connectivity/components; topo for directed dependency order.

## Pitfalls

- Using DFS topo without cycle handling  
- Updating dist without “skip stale heap entries”

## 60-second answer

“Topo sort orders dependencies and detects cycles via indegrees. Dijkstra grows the closest unsettled node with a min-heap for non-negative weighted shortest paths.”

## Further study

- [Graph theory (Wikipedia)](https://en.wikipedia.org/wiki/Graph_theory) — directed graphs, paths, and cycles
- [Topological sorting (Wikipedia)](https://en.wikipedia.org/wiki/Topological_sorting) — Kahn’s algorithm and DFS-based topo
- [Dijkstra's algorithm (Wikipedia)](https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm) — shortest paths with non-negative weights
- [Directed acyclic graph (Wikipedia)](https://en.wikipedia.org/wiki/Directed_acyclic_graph) — when topological order exists

## Practice prompts

1. Course schedule I/II  
2. Network delay time  
3. Cheapest flights within K stops (Bellman-like DP)
