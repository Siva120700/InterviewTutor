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

## Further study

- [Depth-first search](https://en.wikipedia.org/wiki/Depth-first_search)
- [Tree traversal](https://en.wikipedia.org/wiki/Tree_traversal)
- [LeetCode Tree tag](https://leetcode.com/tag/tree/)

## Practice prompts

1. Max depth  
2. Path sum  
3. Lowest common ancestor
