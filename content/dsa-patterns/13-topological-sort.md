---
id: pattern-topo-sort
title: "Pattern: Topological Sort"
track: dsa-patterns
module: "02 Tree and Graph Patterns"
order: 23
languages: [java, csharp]
summary: Kahn’s algorithm and DFS postorder for dependency graphs.
---

## Why this matters

Course schedule, build order, and alien dictionary are topo-sort in disguise.

## Definitions

- **Topological sort:** Linear order of a DAG where every edge u→v has u before v.
- **Kahn’s algorithm:** Queue nodes with indegree 0; reduce neighbors’ indegrees.
- **Cycle:** If not all nodes are ordered, a cycle exists.

## Recognition cues

- Prerequisites / dependencies  
- Course schedule  
- Task ordering  
- Alien dictionary

## Template — Kahn

```java
Queue<Integer> q = new ArrayDeque<>();
for (int i = 0; i < n; i++) if (indeg[i] == 0) q.add(i);
List<Integer> order = new ArrayList<>();
while (!q.isEmpty()) {
  int u = q.poll(); order.add(u);
  for (int v : g[u]) if (--indeg[v] == 0) q.add(v);
}
return order.size() == n ? order : List.of();
```

```csharp
var q = new Queue<int>();
for (int i = 0; i < n; i++) if (indeg[i] == 0) q.Enqueue(i);
var order = new List<int>();
while (q.Count > 0) {
  int u = q.Dequeue(); order.Add(u);
  foreach (var v in g[u]) if (--indeg[v] == 0) q.Enqueue(v);
}
return order.Count == n ? order : new List<int>();
```


## Pattern-wise problems (solve in order)

| # | Problem | Level |
|---|---------|-------|
| 1 | [Course Schedule](https://leetcode.com/problems/course-schedule/) | Medium |
| 2 | [Course Schedule II](https://leetcode.com/problems/course-schedule-ii/) | Medium |
| 3 | [Find Eventual Safe States](https://leetcode.com/problems/find-eventual-safe-states/) | Medium |
| 4 | [Alien Dictionary](https://leetcode.com/problems/alien-dictionary/) *(Premium)* | Hard |

## YouTube (watch after attempting)

- [NeetCode Graphs](https://www.youtube.com/playlist?list=PLot-Xpze53ldBT_7QA8NVot219jFNr_GI) — Course Schedule
- [Striver Graph — Topo Sort](https://www.youtube.com/@takeUforward/playlists)

Master index: **Pattern-Wise Problems + Best YouTube Playlists** (Start Here module).

## Further study

- [Topological sorting](https://en.wikipedia.org/wiki/Topological_sorting)
- [LeetCode Topological Sort tag](https://leetcode.com/tag/topological-sort/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Course schedule  
2. Course schedule II  
3. Alien dictionary (hard)
