---
id: dsa-tsp-bitmask
title: Travelling Salesman — Bitmask DP
track: dsa
module: "07 Dynamic Programming"
order: 63
languages: [java, csharp]
summary: Classic Held–Karp TSP / Hamiltonian path DP with bitmasks for n ≤ 20.
---

## Why this matters

TSP is the flagship bitmask DP. Once you can write `dp[mask][i]`, digit DP and assignment-style Hard problems feel familiar.

## Definitions

- **TSP:** Visit each city once and return to start with minimum total distance (NP-hard).
- **Held–Karp DP:** Exact DP in \(O(n^2 2^n)\) — feasible for n ≤ ~20.
- **State `dp[mask][i]`:** Min cost to visit exactly the set `mask`, ending at city `i` (`i ∈ mask`).
- **Transition:** Try previous city `j` in `mask \ {i}`: `dp[mask][i] = min_j dp[mask^(1<<i)][j] + dist[j][i]`.
- **Hamiltonian path:** Same DP without returning to start (or without the final edge).

## Worked example — min Hamiltonian path (no return)

```java
int tspPath(int[][] dist) {
  int n = dist.length, N = 1 << n, INF = Integer.MAX_VALUE / 4;
  int[][] dp = new int[N][n];
  for (int[] row : dp) Arrays.fill(row, INF);
  for (int i = 0; i < n; i++) dp[1 << i][i] = 0; // start at any
  for (int mask = 0; mask < N; mask++) {
    for (int i = 0; i < n; i++) if ((mask & (1 << i)) != 0 && dp[mask][i] < INF) {
      for (int j = 0; j < n; j++) if ((mask & (1 << j)) == 0) {
        int next = mask | (1 << j);
        dp[next][j] = Math.min(dp[next][j], dp[mask][i] + dist[i][j]);
      }
    }
  }
  int best = INF;
  for (int i = 0; i < n; i++) best = Math.min(best, dp[N - 1][i]);
  return best;
}
```

```csharp
int TspPath(int[][] dist) {
  int n = dist.Length, N = 1 << n, INF = int.MaxValue / 4;
  var dp = new int[N, n];
  for (int m = 0; m < N; m++) for (int i = 0; i < n; i++) dp[m, i] = INF;
  for (int i = 0; i < n; i++) dp[1 << i, i] = 0;
  for (int mask = 0; mask < N; mask++)
    for (int i = 0; i < n; i++) if ((mask & (1 << i)) != 0 && dp[mask, i] < INF)
      for (int j = 0; j < n; j++) if ((mask & (1 << j)) == 0) {
        int next = mask | (1 << j);
        dp[next, j] = Math.Min(dp[next, j], dp[mask, i] + dist[i][j]);
      }
  int best = INF;
  for (int i = 0; i < n; i++) best = Math.Min(best, dp[N - 1, i]);
  return best;
}
```

**Closed tour:** after full mask, add `dist[i][start]` and minimize; often fix start=0 and init only `dp[1][0]=0`.

## Complexity

Time \(O(n^2 2^n)\), memory \(O(n 2^n)\). If n=22 you’re usually too big for interviews.

## Interview Q&A

- **Q:** Reconstruct path?  
  **A:** Keep `parent[mask][i]` = previous city.  
- **Q:** Asymmetric distances?  
  **A:** Fine — `dist[i][j]` need not equal `dist[j][i]`.  
- **Q:** vs nearest-neighbor greedy?  
  **A:** Greedy is approximate; interviews wanting exact ask for DP/backtracking.

## Pitfalls

- Forgetting `i` must be inside `mask`  
- Overflow when summing distances — use INF/4  
- Allowing revisit by not checking bit already set

## 60-second answer

“Exact TSP for small n uses bitmask DP: `dp[mask][i]` is the best way to cover `mask` ending at `i`. Transitions try the previous city. That’s Held–Karp — exponential in n, polynomial in the mask size.”

## Further study

- DP Advanced (interval / digit peek)  
- Bit manipulation · Graphs  

## Practice prompts

1. Shortest Path Visiting All Nodes (LeetCode)  
2. Find the Shortest Superstring (bitmask DP)  
3. Hand-simulate n=3 TSP table
