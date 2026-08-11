---
id: dsa-dp-advanced
title: "Dynamic Programming III — Advanced"
track: dsa
module: "07 Dynamic Programming"
order: 62
languages: [java, csharp]
summary: Interval DP, knapsack variants, DP on trees/graphs intro, and state-compression peek.
---

## Why this matters

Hard rounds combine intervals, bitmasks, or tree DP. You only need the patterns and when to reach for them.

## Definitions

- **Advanced DP:** Patterns beyond simple 1D/2D tables — interval, bitmask, tree, and digit DP for Hard interview rounds.
- **Interval DP:** DP over contiguous segments where `dp[l][r]` is best for (l, r), usually trying every split/last action `k`.
- **Bitmask DP:** Encoding a subset of up to ~20 items as bits in an integer mask; `dp[mask][i]` is best for that subset ending at i.
- **Tree DP:** Computing answers bottom-up by returning one or more values from each subtree (e.g., rob vs skip a node).
- **State compression:** Representing combinatorial choices compactly (often with bitmasks) so the DP state stays enumerable.
- **Digit DP:** Counting numbers with digit-position constraints by building left-to-right with tight/leading-zero flags.
- **Transition order:** The dependency direction (length ascending, mask popcount, postorder) that makes every needed sub-state ready.

## Interval DP — burst balloons / matrix chain idea

`dp[l][r]` = best for interval (l, r). Try each last cut/burst `k` inside.

```text
for length in 2..n:
  for l in 0..n-length:
    r = l + length
    for k in l+1..r-1:
      dp[l][r] = max(dp[l][r], dp[l][k] + dp[k][r] + cost(l,k,r))
```

## Bitmask DP (TSP-style peek)

`dp[mask][i]` = best path visiting `mask` ending at `i`. n ≤ 20.

```java
// sketch: n cities
int N = 1 << n;
int[][] dp = new int[N][n];
// init dp[1<<i][i] = 0
// transition: for mask, for i in mask, for j not in mask:
//   dp[mask|1<<j][j] = min(..., dp[mask][i] + dist[i][j])
```

## DP on trees

Return multiple values from subtree (e.g., rob/not-rob node).

```java
int[] dfs(TreeNode node) {
  // [skip, take]
  if (node == null) return new int[]{0, 0};
  int[] L = dfs(node.left), R = dfs(node.right);
  int take = node.val + L[0] + R[0];
  int skip = Math.max(L[0], L[1]) + Math.max(R[0], R[1]);
  return new int[]{skip, take};
}
```

```csharp
(int skip, int take) Dfs(TreeNode? node) {
  if (node is null) return (0, 0);
  var L = Dfs(node.left); var R = Dfs(node.right);
  int take = node.val + L.skip + R.skip;
  int skip = Math.Max(L.skip, L.take) + Math.Max(R.skip, R.take);
  return (skip, take);
}
```

## Digit DP / probability DP

Mention only: counting numbers with property by digit position; usually rare unless specialized.

## Interview Q&A

- **Q:** How do you invent the state?
  **A:** Include enough info so subproblems don’t need the future — mask, last index, open intervals.
- **Q:** When not DP?
  **A:** Greedy works with exchange argument; or graph shortest path is cleaner.

## Pitfalls

- Overbuilding 3D DP when two indices suffice  
- Forgetting modulo on large counts

## 60-second answer

“Advanced DP expands state: intervals for combined segments, bitmasks for small sets, and pairs of values on trees. I start from the decision that finishes a subproblem and cache it.”

## Further study

- [Dynamic programming (Wikipedia)](https://en.wikipedia.org/wiki/Dynamic_programming) — foundation for interval/tree/bitmask DP
- [Matrix chain multiplication (Wikipedia)](https://en.wikipedia.org/wiki/Matrix_chain_multiplication) — classic interval DP recurrence
- [Bit manipulation (Wikipedia)](https://en.wikipedia.org/wiki/Bit_manipulation) — masks as compact subset states
- [Tree (graph theory) (Wikipedia)](https://en.wikipedia.org/wiki/Tree_(graph_theory)) — structure underlying tree DP

## Practice prompts

1. Burst balloons  
2. House robber III  
3. Shortest path visiting all nodes (bitmask)
