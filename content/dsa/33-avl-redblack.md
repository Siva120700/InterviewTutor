---
id: dsa-avl-redblack
title: AVL and Red-Black Trees
track: dsa
module: "04 Trees"
order: 33
languages: [java, csharp]
summary: Height-balanced BSTs — AVL rotations by balance factor, and red-black color invariants.
---

## Why this matters

Unbalanced BSTs degrade to O(n). AVL and red-black trees keep height O(log n). You rarely code full RB trees in interviews, but you must explain invariants and when libraries use them (`TreeMap`, sorted sets).

## Definitions

- **Balanced BST:** A binary search tree whose height stays \(O(\log n)\) under inserts/deletes.
- **AVL tree:** Rebalances after updates using the **balance factor** (\(\text{height}(L)-\text{height}(R) \in \{-1,0,1\}\)).
- **Rotation:** Local restructuring (LL, RR, LR, RL) that restores BST order and improves balance.
- **Red-black tree:** BST with a color bit per node obeying color/black-height invariants; slightly looser balance than AVL, fewer rotations on update.
- **Black height:** Number of black nodes on a path from a node to a leaf (same for all paths in a valid RB tree).

## AVL rotations (intuition)

```text
Right rotate (LL):        Left rotate (RR):
    y                         x
   / \                       / \
  x   C        →            A   y
 / \                           / \
A   B                         B   C
```

LR/RL = double rotation (left then right, or right then left).

## Red-black invariants

1. Each node is red or black  
2. Root is black  
3. No two reds in a row (red node’s children are black)  
4. Every path from a node to null leaves has the same number of black nodes  
5. Nil leaves are black (conceptual)

⇒ longest path ≤ 2× shortest ⇒ height \(O(\log n)\).

## Worked example 1 — Height / balance factor

```java
int height(TreeNode n) {
  return n == null ? 0 : 1 + Math.max(height(n.left), height(n.right));
}
int balance(TreeNode n) {
  return n == null ? 0 : height(n.left) - height(n.right);
}
// AVL: after insert, if |balance| > 1, rotate by insert-side cases
```

```csharp
int Height(TreeNode? n) =>
  n is null ? 0 : 1 + Math.Max(Height(n.left), Height(n.right));
int Balance(TreeNode? n) =>
  n is null ? 0 : Height(n.left) - Height(n.right);
```

## Worked example 2 — Right rotate

```java
TreeNode rotateRight(TreeNode y) {
  TreeNode x = y.left;
  TreeNode b = x.right;
  x.right = y;
  y.left = b;
  return x; // new subtree root
}
```

```csharp
TreeNode RotateRight(TreeNode y) {
  var x = y.left!;
  var b = x.right;
  x.right = y;
  y.left = b;
  return x;
}
```

## AVL vs red-black

| | AVL | Red-black |
|---|-----|-----------|
| Balance | Stricter (flatter) | Looser |
| Lookup | Slightly faster | Slightly slower |
| Insert/delete | More rotations | Fewer rotations |
| Library use | Less common | Very common (`TreeMap`, etc.) |

## Interview Q&A

- **Q:** Why not always keep a perfect BST?  
  **A:** Perfect rebuild is O(n); rotations are O(1) local fixes amortized into O(log n) updates.
- **Q:** Do I implement RB in coding interviews?  
  **A:** Almost never — explain invariants / use `TreeMap`/`SortedDictionary`.
- **Q:** Same ordered keys as BST?  
  **A:** Yes — inorder still sorted; balancing doesn’t break BST property.

## Pitfalls

- Rotating without updating parent links in threaded implementations  
- Confusing heap shape (“complete tree”) with BST balance  
- Claiming RB is O(1) height — it’s O(log n)

## 60-second answer

“AVL keeps |balance factor| ≤ 1 via rotations. Red-black uses color invariants so paths don’t get more than 2× apart. Both keep BST operations O(log n); languages usually ship RB-backed sorted maps.”

## Further study

- [AVL tree](https://en.wikipedia.org/wiki/AVL_tree)
- [Red–black tree](https://en.wikipedia.org/wiki/Red%E2%80%93black_tree)
- [Tree rotation](https://en.wikipedia.org/wiki/Tree_rotation)

## Practice prompts

1. Walk through inserting an ascending sequence into AVL vs plain BST  
2. Name which rotation fixes an LR imbalance  
3. List the five red-black invariants from memory
