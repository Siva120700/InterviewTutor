---
id: pattern-tree-bfs
title: "Pattern: Tree BFS"
track: dsa-patterns
module: "02 Tree and Graph Patterns"
order: 20
languages: [java, csharp]
summary: Level-order traversal with a queue — zigzag, right side view, level averages.
---

## Why this matters

Any “level by level” tree question is Tree BFS. Same queue idea as graph BFS.

## Definitions

- **Tree BFS:** Explore nodes level-by-level using a queue.
- **Level size trick:** Snapshot `queue.size()` at the start of each level.
- **Zigzag / right view:** Variants that pick order or last node per level.

## Recognition cues

- Level order traversal  
- Min depth / level averages  
- Right/left side view  
- Connect nodes at same level  
- Shortest path in binary tree (unweighted)

## Template

```java
Queue<TreeNode> q = new ArrayDeque<>();
q.add(root);
while (!q.isEmpty()) {
  int n = q.size();
  for (int i = 0; i < n; i++) {
    TreeNode cur = q.poll();
    if (cur.left != null) q.add(cur.left);
    if (cur.right != null) q.add(cur.right);
  }
}
```

```csharp
var q = new Queue<TreeNode>();
q.Enqueue(root);
while (q.Count > 0) {
  int n = q.Count;
  for (int i = 0; i < n; i++) {
    var cur = q.Dequeue();
    if (cur.left is not null) q.Enqueue(cur.left);
    if (cur.right is not null) q.Enqueue(cur.right);
  }
}
```

## Further study

- [Breadth-first search](https://en.wikipedia.org/wiki/Breadth-first_search)
- [LeetCode BFS tag](https://leetcode.com/tag/breadth-first-search/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Binary tree level order  
2. Binary tree right side view  
3. Zigzag level order
