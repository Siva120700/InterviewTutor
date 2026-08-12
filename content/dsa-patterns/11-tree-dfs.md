---
id: pattern-tree-dfs
title: "Pattern: Tree DFS"
track: dsa-patterns
module: "02 Tree and Graph Patterns"
order: 21
languages: [java, csharp]
summary: Root–left–right recursion for paths, diameters, and subtree answers.
---

## Why this matters

Most Medium tree problems are “return something from left/right, combine at node”.

## Definitions

- **Tree DFS:** Recurse into children; preorder/inorder/postorder define visit timing.
- **Bottom-up DFS:** Compute child answers first, then node (height, diameter).
- **Path problem:** Track running sum/state along the root-to-leaf or any-node path.

## Recognition cues

- Path sum / max path sum  
- Diameter / balanced check  
- Validate BST (with bounds)  
- LCA  
- Serialize/deserialize

## Template

```java
int dfs(TreeNode node) {
  if (node == null) return 0;
  int L = dfs(node.left);
  int R = dfs(node.right);
  // combine L, R, node.val → answer / return value
  return 1 + Math.max(L, R);
}
```

```csharp
int Dfs(TreeNode? node) {
  if (node is null) return 0;
  int L = Dfs(node.left);
  int R = Dfs(node.right);
  return 1 + Math.Max(L, R);
}
```


## Pattern-wise problems (solve in order)

| # | Problem | Level |
|---|---------|-------|
| 1 | [Maximum Depth of Binary Tree](https://leetcode.com/problems/maximum-depth-of-binary-tree/) | Easy |
| 2 | [Path Sum](https://leetcode.com/problems/path-sum/) | Easy |
| 3 | [Diameter of Binary Tree](https://leetcode.com/problems/diameter-of-binary-tree/) | Easy |
| 4 | [Validate Binary Search Tree](https://leetcode.com/problems/validate-binary-search-tree/) | Medium |
| 5 | [Lowest Common Ancestor of a Binary Tree](https://leetcode.com/problems/lowest-common-ancestor-of-a-binary-tree/) | Medium |

## YouTube (watch after attempting)

- [NeetCode](https://www.youtube.com/@NeetCode) — Trees / DFS
- [Striver Binary Trees + BST](https://www.youtube.com/@takeUforward/playlists)
- [Blind 75 playlist](https://www.youtube.com/playlist?list=PLot-Xpze53ldVwtstag2TL4HQhAnC8ATf)

Master index: **Pattern-Wise Problems + Best YouTube Playlists** (Start Here module).

## Further study

- [Depth-first search](https://en.wikipedia.org/wiki/Depth-first_search)
- [Tree traversal](https://en.wikipedia.org/wiki/Tree_traversal)
- [LeetCode Tree tag](https://leetcode.com/tag/tree/)

## Practice prompts

1. Max depth  
2. Path sum  
3. Lowest common ancestor
