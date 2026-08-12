---
id: dsa-graph-cycles-flood
title: Graph Cycles, Flood Fill, Multi-Source BFS
track: dsa
module: "06 Graphs and Search"
order: 48
languages: [java, csharp]
summary: Detect cycles in directed/undirected graphs, flood fill / islands, and multi-source BFS.
---

## Why this matters

Cycle checks appear in course schedules and dependency graphs. Flood fill powers island/matrix problems. Multi-source BFS is the clean way to “nearest distance from any of several starts.”

## Definitions

- **Undirected cycle:** A back-edge to a visited neighbor that is not the parent in the DFS/BFS tree.
- **Directed cycle:** A back-edge to a node on the current recursion stack (three-color DFS) or leftover nodes in Kahn’s topo.
- **Flood fill:** DFS/BFS from a cell to paint/count a connected component of equal values.
- **Island:** Connected component of land cells (usually 4-direction).
- **Multi-source BFS:** Seed the queue with all sources at distance 0 so the wavefront expands from every start at once.

## Worked example 1 — undirected cycle (DFS)

```java
boolean hasCycle(List<Integer>[] g) {
  int n = g.length; boolean[] seen = new boolean[n];
  for (int i = 0; i < n; i++) if (!seen[i] && dfs(g, i, -1, seen)) return true;
  return false;
}
boolean dfs(List<Integer>[] g, int u, int parent, boolean[] seen) {
  seen[u] = true;
  for (int v : g[u]) {
    if (v == parent) continue;
    if (seen[v] || dfs(g, v, u, seen)) return true;
  }
  return false;
}
```

## Worked example 2 — directed cycle (colors)

```java
// 0=unseen, 1=active, 2=done
boolean hasDirectedCycle(List<Integer>[] g) {
  int[] col = new int[g.length];
  for (int i = 0; i < g.length; i++) if (col[i] == 0 && dfs(g, i, col)) return true;
  return false;
}
boolean dfs(List<Integer>[] g, int u, int[] col) {
  col[u] = 1;
  for (int v : g[u]) {
    if (col[v] == 1) return true;
    if (col[v] == 0 && dfs(g, v, col)) return true;
  }
  col[u] = 2;
  return false;
}
```

## Worked example 3 — multi-source BFS (rotting oranges style)

```java
int multiSource(int[][] grid) {
  int m = grid.length, n = grid[0].length;
  Queue<int[]> q = new ArrayDeque<>();
  int fresh = 0;
  for (int i = 0; i < m; i++)
    for (int j = 0; j < n; j++) {
      if (grid[i][j] == 2) q.add(new int[]{i, j});
      if (grid[i][j] == 1) fresh++;
    }
  int[][] dirs = {{1,0},{-1,0},{0,1},{0,-1}};
  int minutes = 0;
  while (!q.isEmpty() && fresh > 0) {
    int sz = q.size();
    for (int k = 0; k < sz; k++) {
      int[] cur = q.poll();
      for (int[] d : dirs) {
        int ni = cur[0] + d[0], nj = cur[1] + d[1];
        if (ni < 0 || nj < 0 || ni >= m || nj >= n || grid[ni][nj] != 1) continue;
        grid[ni][nj] = 2; fresh--; q.add(new int[]{ni, nj});
      }
    }
    minutes++;
  }
  return fresh == 0 ? minutes : -1;
}
```

```csharp
// Same structure: enqueue all sources first, then level-order expand
```

## Flood fill / islands

```java
void dfsFill(char[][] g, int i, int j) {
  if (i < 0 || j < 0 || i >= g.length || j >= g[0].length || g[i][j] != '1') return;
  g[i][j] = '0';
  dfsFill(g, i + 1, j); dfsFill(g, i - 1, j);
  dfsFill(g, i, j + 1); dfsFill(g, i, j - 1);
}
```

## Interview Q&A

- **Q:** BFS cycle check undirected?  
  **A:** Yes — parent map; visited neighbor ≠ parent ⇒ cycle.
- **Q:** When multi-source over single BFS from each?  
  **A:** Same asymptotic often, but one pass is simpler and faster in practice.
- **Q:** 8-direction islands?  
  **A:** Clarify; add diagonals to `dirs`.

## Pitfalls

- Treating undirected edge as a cycle (forgetting parent)  
- Using only visited (not stack color) for directed cycles  
- Mutating grid without cloning when reuse matters

## 60-second answer

“Undirected cycles: visited + parent. Directed: recursion-stack / three colors or failed topo. Flood fill DFS/BFS components. Multi-source BFS seeds all starts at distance 0.”

## Further study

- Graph representations · BFS · DFS/Backtracking · Topo sort  

## Practice prompts

1. Number of Islands  
2. Course Schedule (cycle)  
3. 01 Matrix / Rotting Oranges
