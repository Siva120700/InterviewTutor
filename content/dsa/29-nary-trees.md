---
id: dsa-nary-trees
title: General and N-ary Trees
track: dsa
module: "04 Trees"
order: 29
languages: [java, csharp]
summary: N-ary parenting nodes, child arrays/lists, and traversal patterns beyond binary trees.
---

## Why this matters

File systems, org charts, DOM, and many interview “n-ary tree” problems aren’t binary. You need a child *list* mental model and multi-child recursion/BFS.

## Definitions

- **General (n-ary) tree:** Each node may have any number of children (unconstrained branching factor).
- **Parent / child / sibling:** Hierarchical relations; siblings share a parent.
- **Rooted tree:** One designated root; every other node has exactly one parent.
- **Forest:** A set of disjoint trees.
- **Degree:** Number of children of a node (or edges in graph terms).
- **Serialization:** Encoding an n-ary tree (e.g. children count + values) for clone/transmit problems.

## Concept

```text
        A
     /  |  \
    B   C   D
   / \      |
  E   F     G
```

Binary trees are the special case with at most two named children (`left`/`right`). N-ary nodes usually store `List<Node> children`.

## Worked example 1 — Max depth

```java
class Node {
  int val;
  List<Node> children = new ArrayList<>();
  Node(int v) { val = v; }
}

int maxDepth(Node root) {
  if (root == null) return 0;
  int best = 0;
  for (Node c : root.children) best = Math.max(best, maxDepth(c));
  return best + 1;
}
```

```csharp
class Node {
  public int Val;
  public List<Node> Children = new();
  public Node(int v) => Val = v;
}

int MaxDepth(Node? root) {
  if (root is null) return 0;
  int best = 0;
  foreach (var c in root.Children) best = Math.Max(best, MaxDepth(c));
  return best + 1;
}
```

## Worked example 2 — Level order

```java
List<List<Integer>> levelOrder(Node root) {
  List<List<Integer>> res = new ArrayList<>();
  if (root == null) return res;
  Queue<Node> q = new ArrayDeque<>();
  q.add(root);
  while (!q.isEmpty()) {
    int sz = q.size();
    List<Integer> level = new ArrayList<>(sz);
    for (int i = 0; i < sz; i++) {
      Node u = q.poll();
      level.add(u.val);
      q.addAll(u.children);
    }
    res.add(level);
  }
  return res;
}
```

```csharp
IList<IList<int>> LevelOrder(Node? root) {
  var res = new List<IList<int>>();
  if (root is null) return res;
  var q = new Queue<Node>();
  q.Enqueue(root);
  while (q.Count > 0) {
    int sz = q.Count;
    var level = new List<int>(sz);
    for (int i = 0; i < sz; i++) {
      var u = q.Dequeue();
      level.Add(u.Val);
      foreach (var c in u.Children) q.Enqueue(c);
    }
    res.Add(level);
  }
  return res;
}
```

## Binary encoding trick

Any n-ary tree can map to a binary tree: `left` = first child, `right` = next sibling — useful trivia, rarely needed to implement.

## Interview Q&A

- **Q:** DFS on n-ary?  
  **A:** Recurse each child, or iterative stack pushing children (order depends on push sequence).
- **Q:** Space for storing children?  
  **A:** Array if fixed arity; `List`/`ArrayList` when dynamic.
- **Q:** LCA on n-ary?  
  **A:** Same recursive idea as binary — search children, combine “found left/right” signals carefully for multi-way.

## Pitfalls

- Forgetting empty `children` list vs null  
- Mutating the child list while iterating  
- Assuming binary templates (`left`/`right`) still apply

## 60-second answer

“An n-ary node holds a list of children. Depth and level-order are the same recursion/BFS patterns as binary trees, but loops replace left/right. Most ‘general tree’ interview problems are LeetCode-style Node with `children`.”

## Further study

- [Tree (data structure)](https://en.wikipedia.org/wiki/Tree_(data_structure))
- LeetCode N-ary Tree templates (max depth, level order, serialize)

## Practice prompts

1. Serialize / deserialize an n-ary tree  
2. Find the diameter (longest path) in an n-ary tree  
3. Convert n-ary → binary (left-child right-sibling)
