---
id: dsa-binary-trees
title: Binary Trees
track: dsa
module: "04 Trees"
order: 30
languages: [java, csharp]
summary: Traversals, height/diameter, LCA intro, and recursion templates for tree interviews.
---

## Why this matters

Trees are recursion practice. Most Medium tree problems are “define answer from left/right subtrees”.

## Definitions

- **Binary tree:** A hierarchical structure where each node has at most two children — left and right.
- **Root / leaf:** The root is the topmost node with no parent; a leaf has no children.
- **Traversal:** A systematic visit order — preorder (root-left-right), inorder (left-root-right), postorder (left-right-root), or level-order (BFS).
- **Height / depth:** Depth is distance from the root to a node; height is the longest root-to-leaf path (be consistent about edges vs nodes).
- **Diameter:** The longest path between any two nodes, measured in edges; often via depths of left and right subtrees.
- **Height-balanced tree:** For every node, the height difference of left and right subtrees is at most 1.
- **LCA (lowest common ancestor):** The deepest node that has both given nodes as descendants (including themselves).

## Traversals

| Order | Visit |
|-------|-------|
| Preorder | root → left → right |
| Inorder | left → root → right |
| Postorder | left → right → root |
| Level | BFS queue |

## Worked example 1 — Max depth

```java
int maxDepth(TreeNode root) {
  if (root == null) return 0;
  return 1 + Math.max(maxDepth(root.left), maxDepth(root.right));
}
```

```csharp
int MaxDepth(TreeNode? root) =>
  root is null ? 0 : 1 + Math.Max(MaxDepth(root.left), MaxDepth(root.right));
```

## Worked example 2 — Diameter

```java
int diameter = 0;
int depth(TreeNode node) {
  if (node == null) return 0;
  int L = depth(node.left), R = depth(node.right);
  diameter = Math.max(diameter, L + R);
  return 1 + Math.max(L, R);
}
```

```csharp
int _diameter;
int Depth(TreeNode? node) {
  if (node is null) return 0;
  int L = Depth(node.left), R = Depth(node.right);
  _diameter = Math.Max(_diameter, L + R);
  return 1 + Math.Max(L, R);
}
```

## Worked example 3 — Invert tree

```java
TreeNode invert(TreeNode root) {
  if (root == null) return null;
  TreeNode tmp = root.left;
  root.left = invert(root.right);
  root.right = invert(tmp);
  return root;
}
```

## Interview Q&A

- **Q:** Recursive vs iterative?
  **A:** Recursion is clearer; know iterative inorder with stack for follow-ups.
- **Q:** Balanced?
  **A:** Height-balanced: check depth difference ≤ 1 bottom-up in O(n).

## Pitfalls

- Null checks missing  
- Recomputing height repeatedly → O(n²)

## 60-second answer

“I solve trees bottom-up: ask what each node needs from children. Traversals pick the visit order; BFS handles levels. I track global answers like diameter while returning height.”

## Further study

- [Binary tree (Wikipedia)](https://en.wikipedia.org/wiki/Binary_tree) — structure, height, and leaves
- [Tree traversal (Wikipedia)](https://en.wikipedia.org/wiki/Tree_traversal) — preorder/inorder/postorder/level-order
- [Lowest common ancestor (Wikipedia)](https://en.wikipedia.org/wiki/Lowest_common_ancestor) — LCA interview classic
- [Breadth-first search (Wikipedia)](https://en.wikipedia.org/wiki/Breadth-first_search) — level-order traversal via queue

## Practice prompts

1. Path sum  
2. Serialize/deserialize  
3. Lowest common ancestor (binary tree)
