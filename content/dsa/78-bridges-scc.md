---
id: dsa-bridges-scc
title: Bridges, Articulation Points, and SCCs
track: dsa
module: "06 Graphs and Search"
order: 78
languages: [java, csharp]
summary: Tarjan-style discovery times for bridges/cut vertices, and Kosaraju/Tarjan SCCs.
---

## Why this matters

“Critical connections,” network reliability, and strongly connected components show up in Hard graph rounds and system-design-flavored coding.

## Definitions

- **Bridge:** An undirected edge whose removal increases the number of connected components.
- **Articulation point (cut vertex):** A vertex whose removal increases component count.
- **Discovery time `disc[u]`:** DFS timestamp when `u` is first visited.
- **Low-link `low[u]`:** Smallest discovery time reachable from `u`’s subtree including back-edges.
- **SCC (strongly connected component):** Maximal set of vertices in a digraph where each can reach every other.
- **Kosaraju:** Two DFS passes — order by finish time, then DFS on reversed graph.
- **Tarjan SCC:** One DFS with stack + low-link to emit components.

## Bridges (Tarjan idea)

```java
List<int[]> bridges(int n, List<Integer>[] g) {
  int[] disc = new int[n], low = new int[n];
  Arrays.fill(disc, -1);
  List<int[]> ans = new ArrayList<>();
  int[] timer = {0};
  for (int i = 0; i < n; i++) if (disc[i] < 0) dfs(i, -1, g, disc, low, timer, ans);
  return ans;
}
void dfs(int u, int p, List<Integer>[] g, int[] disc, int[] low, int[] timer, List<int[]> ans) {
  disc[u] = low[u] = timer[0]++;
  for (int v : g[u]) {
    if (v == p) continue;
    if (disc[v] >= 0) low[u] = Math.min(low[u], disc[v]);
    else {
      dfs(v, u, g, disc, low, timer, ans);
      low[u] = Math.min(low[u], low[v]);
      if (low[v] > disc[u]) ans.add(new int[]{u, v}); // bridge
    }
  }
}
```

**Articulation:** root with ≥2 DFS children, or non-root with child `v` where `low[v] >= disc[u]`.

## Kosaraju SCC (sketch)

```text
1) DFS on G, push nodes to stack by finish time
2) Build reversed graph G^T
3) Pop stack; DFS on G^T marking each tree as one SCC
```

```java
void dfsFinish(int u, boolean[] seen, List<Integer>[] g, Deque<Integer> st) {
  seen[u] = true;
  for (int v : g[u]) if (!seen[v]) dfsFinish(v, seen, g, st);
  st.push(u);
}
```

## Interview Q&A

- **Q:** Bridge condition why `low[v] > disc[u]`?  
  **A:** Subtree of `v` can’t reach `u` or ancestors — edge `u-v` is the only connection.
- **Q:** Directed bridges?  
  **A:** Different notion; interviews usually mean undirected bridges / directed SCCs.
- **Q:** Condensation graph?  
  **A:** SCCs contracted to DAG — useful for “path exists” after components.

## Pitfalls

- Forgetting parent skip → every edge looks like a back-edge  
- Multiple edges between same pair  
- Using `>=` vs `>` incorrectly for bridges vs articulations

## 60-second answer

“I run Tarjan DFS with disc/low: bridges when `low[child] > disc[u]`, articulations with the root/child rules. For directed graphs I find SCCs with Kosaraju or Tarjan, then reason on the DAG of components.”

## Further study

- Graph representations · DFS · Topo sort  

## Practice prompts

1. Critical Connections in a Network  
2. Number of SCCs / Kosaraju implement  
3. Articulation points on a small graph by hand
