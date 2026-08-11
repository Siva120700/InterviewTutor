---
id: dsa-dfs-backtracking
title: DFS and Backtracking
track: dsa
module: "06 Graphs and Search"
order: 51
languages: [java, csharp]
summary: Graph DFS, subsets/permutations, grid search, and prune strategies.
---

## Why this matters

Backtracking builds candidates incrementally and undoes choices — subsets, combinations, sudoku, path search.

## Definitions

- **DFS (depth-first search):** Exploring as far as possible along each branch before backtracking; implemented with recursion or an explicit stack.
- **Backtracking:** Building a candidate solution step by step, then undoing (“popping”) a choice when it cannot lead to a valid answer.
- **State / path:** The partial solution currently under construction (chosen elements, board cells, or path nodes).
- **Pruning:** Cutting off a branch early when constraints show it cannot succeed, reducing exponential search.
- **Subsets / permutations:** Exhaustive generation — subsets choose include/skip (or start-index loops); permutations try every unused element.
- **Visited / seen set:** Markers that prevent revisiting nodes in a graph or reusing the same cell in a grid path.
- **Undo step:** Restoring state after exploring a choice (pop from path, unmark cell) so sibling branches stay correct.

## Graph DFS

```java
void dfs(int u, boolean[] seen, Map<Integer, List<Integer>> g) {
  seen[u] = true;
  for (int v : g.getOrDefault(u, List.of()))
    if (!seen[v]) dfs(v, seen, g);
}
```

```csharp
void Dfs(int u, bool[] seen, Dictionary<int, List<int>> g) {
  seen[u] = true;
  if (!g.TryGetValue(u, out var nbrs)) return;
  foreach (var v in nbrs) if (!seen[v]) Dfs(v, seen, g);
}
```

## Worked example — Subsets

```java
List<List<Integer>> subsets(int[] nums) {
  List<List<Integer>> res = new ArrayList<>();
  backtrack(0, nums, new ArrayList<>(), res);
  return res;
}
void backtrack(int i, int[] nums, List<Integer> path, List<List<Integer>> res) {
  res.add(new ArrayList<>(path));
  for (int j = i; j < nums.length; j++) {
    path.add(nums[j]);
    backtrack(j + 1, nums, path, res);
    path.remove(path.size() - 1);
  }
}
```

```csharp
IList<IList<int>> Subsets(int[] nums) {
  var res = new List<IList<int>>();
  void Bt(int i, List<int> path) {
    res.Add(path.ToList());
    for (int j = i; j < nums.Length; j++) {
      path.Add(nums[j]);
      Bt(j + 1, path);
      path.RemoveAt(path.Count - 1);
    }
  }
  Bt(0, new List<int>());
  return res;
}
```

## Template

```text
def bt(state):
  if done: record; return
  for choice in choices:
    apply choice
    bt(new state)
    undo choice
```

## Interview Q&A

- **Q:** Time for permutations?
  **A:** O(n · n!) to build all.
- **Q:** Pruning?
  **A:** Skip invalid early (sudoku constraints, sum overflow).

## Pitfalls

- Forgetting undo  
- Mutating shared path without copy on save

## 60-second answer

“DFS explores deep paths; backtracking adds undo for combinatorial generation. I state complexity from the decision tree and prune aggressively.”

## Further study

- [Depth-first search (Wikipedia)](https://en.wikipedia.org/wiki/Depth-first_search) — recursive/stack exploration of graphs and trees
- [Backtracking (Wikipedia)](https://en.wikipedia.org/wiki/Backtracking) — build, prune, and undo candidate solutions
- [Graph theory (Wikipedia)](https://en.wikipedia.org/wiki/Graph_theory) — adjacency models for DFS
- [Combinatorial search (Wikipedia)](https://en.wikipedia.org/wiki/Combinatorial_search) — subsets/permutations style exhaustive search

## Practice prompts

1. Combination sum  
2. N-Queens  
3. Word search in grid
