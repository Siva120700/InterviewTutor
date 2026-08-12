---
id: pattern-union-find
title: "Pattern: Union Find"
track: dsa-patterns
module: "02 Tree and Graph Patterns"
order: 24
languages: [java, csharp]
summary: Disjoint sets for connectivity, provinces, and redundant edges.
---

## Why this matters

Dynamic connectivity questions scream Union-Find — often simpler than full graph DFS.

## Definitions

- **Union-Find (DSU):** Structure supporting `find` (component id) and `union` (merge sets).
- **Path compression:** Flatten parents during `find` for near O(1) ops.
- **Union by rank/size:** Attach smaller tree under larger to keep depth small.

## Recognition cues

- Number of connected components / provinces  
- Redundant connection  
- Accounts merge / similar string groups  
- Kruskal MST edges

## Template

```java
class DSU {
  int[] p, r;
  DSU(int n) { p = new int[n]; r = new int[n]; for (int i=0;i<n;i++) p[i]=i; }
  int find(int x){ return p[x]==x?x:(p[x]=find(p[x])); }
  boolean union(int a,int b){
    int ra=find(a), rb=find(b);
    if (ra==rb) return false;
    if (r[ra]<r[rb]) p[ra]=rb; else if (r[ra]>r[rb]) p[rb]=ra; else { p[rb]=ra; r[ra]++; }
    return true;
  }
}
```

```csharp
class Dsu {
  readonly int[] p, r;
  public Dsu(int n){ p=Enumerable.Range(0,n).ToArray(); r=new int[n]; }
  public int Find(int x)=> p[x]==x?x:p[x]=Find(p[x]);
  public bool Union(int a,int b){
    int ra=Find(a), rb=Find(b);
    if (ra==rb) return false;
    if (r[ra]<r[rb]) p[ra]=rb; else if (r[ra]>r[rb]) p[rb]=ra; else { p[rb]=ra; r[ra]++; }
    return true;
  }
}
```


## Pattern-wise problems (solve in order)

| # | Problem | Level |
|---|---------|-------|
| 1 | [Number of Provinces](https://leetcode.com/problems/number-of-provinces/) | Medium |
| 2 | [Redundant Connection](https://leetcode.com/problems/redundant-connection/) | Medium |
| 3 | [Graph Valid Tree](https://leetcode.com/problems/graph-valid-tree/) *(Premium)* | Medium |
| 4 | [Accounts Merge](https://leetcode.com/problems/accounts-merge/) | Medium |

## YouTube (watch after attempting)

- [NeetCode](https://www.youtube.com/@NeetCode) — Union Find / Redundant Connection
- [Striver DSU](https://www.youtube.com/@takeUforward)

Master index: **Pattern-Wise Problems + Best YouTube Playlists** (Start Here module).

## Further study

- [Disjoint-set data structure](https://en.wikipedia.org/wiki/Disjoint-set_data_structure)
- [LeetCode Union Find tag](https://leetcode.com/tag/union-find/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Number of provinces  
2. Redundant connection  
3. Accounts merge
