---
id: dsa-bst
title: Binary Search Trees
track: dsa
module: "04 Trees"
order: 31
languages: [java, csharp]
summary: BST invariants, search/insert/delete, validate BST, and inorder properties.
---

## Why this matters

BST gives O(log n) average search when balanced. Interviews test the invariant: left < node < right (per subtree).

## Definitions

- **Binary search tree (BST):** A binary tree where every node’s value is greater than all in its left subtree and less than all in its right.
- **BST invariant:** The ordering property left < node < right that enables binary-search-style navigation.
- **Inorder traversal of BST:** Visiting left → root → right yields values in sorted ascending order.
- **Validate BST:** Checking that every node lies within a valid (low, high) value range inherited from its ancestors.
- **Inorder successor:** The next larger value after a node — leftmost in its right subtree (used when deleting a two-child node).
- **Balanced BST:** A BST kept height-balanced (AVL/Red-Black) so ops stay O(log n) instead of degenerating to O(n).
- **Search path:** The unique root-to-node route determined by comparisons; length is the cost of find/insert.

## Worked example 1 — Validate BST

```java
boolean isValidBST(TreeNode root) {
  return valid(root, Long.MIN_VALUE, Long.MAX_VALUE);
}
boolean valid(TreeNode n, long lo, long hi) {
  if (n == null) return true;
  if (n.val <= lo || n.val >= hi) return false;
  return valid(n.left, lo, n.val) && valid(n.right, n.val, hi);
}
```

```csharp
bool IsValidBST(TreeNode? root) => Valid(root, long.MinValue, long.MaxValue);
bool Valid(TreeNode? n, long lo, long hi) {
  if (n is null) return true;
  if (n.val <= lo || n.val >= hi) return false;
  return Valid(n.left, lo, n.val) && Valid(n.right, n.val, hi);
}
```

## Worked example 2 — Kth smallest

Inorder traversal yields sorted order — stop at k.

```java
int kthSmallest(TreeNode root, int k) {
  Deque<TreeNode> st = new ArrayDeque<>();
  while (true) {
    while (root != null) { st.push(root); root = root.left; }
    root = st.pop();
    if (--k == 0) return root.val;
    root = root.right;
  }
}
```

## Interview Q&A

- **Q:** Unbalanced BST?
  **A:** Degenerates to O(n); mention AVL/Red-Black/TreeMap.
- **Q:** Delete node?
  **A:** 0/1 child easy; 2 children → replace with inorder successor.

## Pitfalls

- Using wrong bounds (`Integer.MIN_VALUE` with equal edge cases)  
- Assuming any binary tree is a BST

## 60-second answer

“BST invariant enables binary search down the tree. I validate with value ranges, use inorder for sorted views, and call out balance for complexity guarantees.”

## Further study

- [Binary search tree (Wikipedia)](https://en.wikipedia.org/wiki/Binary_search_tree) — BST invariant and operations
- [Tree traversal (Wikipedia)](https://en.wikipedia.org/wiki/Tree_traversal) — inorder yielding sorted order
- [Self-balancing binary search tree (Wikipedia)](https://en.wikipedia.org/wiki/Self-balancing_binary_search_tree) — why height balance matters for O(log n)
- [AVL tree (Wikipedia)](https://en.wikipedia.org/wiki/AVL_tree) — classic balanced BST reference

## Practice prompts

1. Lowest common ancestor in BST  
2. Convert sorted array to BST  
3. Recover BST with two swapped nodes
