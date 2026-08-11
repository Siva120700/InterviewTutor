---
id: pattern-island-matrix
title: "Pattern: Islands (Matrix Traversal)"
track: dsa-patterns
module: "02 Tree and Graph Patterns"
order: 22
languages: [java, csharp]
summary: DFS/BFS on grids — islands, flood fill, surrounded regions.
---

## Why this matters

Grids are graphs. Island pattern is DFS/BFS with 4/8 directions and visited marking.

## Definitions

- **Island pattern:** Treat each cell as a node; explore connected component of land.
- **Flood fill:** Recolor/mark a connected region.
- **In-place visit:** Often mark grid cell as visited (`'0'`/`-1`) to save a `visited[][]`.

## Recognition cues

- Number of islands  
- Max area of island  
- Flood fill / surrounded regions  
- Shortest path in binary matrix (BFS)

## Template — DFS island count

```java
int numIslands(char[][] g) {
  int m = g.length, n = g[0].length, count = 0;
  for (int i = 0; i < m; i++)
    for (int j = 0; j < n; j++)
      if (g[i][j] == '1') { count++; dfs(g, i, j); }
  return count;
}
void dfs(char[][] g, int i, int j) {
  if (i < 0 || j < 0 || i >= g.length || j >= g[0].length || g[i][j] != '1') return;
  g[i][j] = '0';
  dfs(g, i+1, j); dfs(g, i-1, j); dfs(g, i, j+1); dfs(g, i, j-1);
}
```

```csharp
void Dfs(char[][] g, int i, int j) {
  if (i < 0 || j < 0 || i >= g.Length || j >= g[0].Length || g[i][j] != '1') return;
  g[i][j] = '0';
  Dfs(g, i + 1, j); Dfs(g, i - 1, j); Dfs(g, i, j + 1); Dfs(g, i, j - 1);
}
```

## Further study

- [Connected component](https://en.wikipedia.org/wiki/Component_(graph_theory))
- [LeetCode Matrix / BFS tags](https://leetcode.com/tag/matrix/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Number of islands  
2. Max area of island  
3. Surrounded regions
