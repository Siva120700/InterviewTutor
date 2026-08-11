---
id: pattern-knapsack-dp
title: "Pattern: 0/1 Knapsack DP"
track: dsa-patterns
module: "06 DP Patterns"
order: 60
languages: [java, csharp]
summary: Capacity DP template — 0/1 vs unbounded, and common interview mappings.
---

## Why this matters

Many DP Mediums are knapsack in disguise: subset sum, partition, target sum.

## Definitions

- **0/1 knapsack:** Each item used at most once — iterate capacity downward.
- **Unbounded knapsack:** Item reusable — iterate capacity upward (coin change).
- **State:** `dp[c] = best value achievable with capacity c`.

## Recognition cues

- Subset sum / partition equal subset  
- Target sum (± assignment)  
- Coin change (unbounded)  
- Count ways / min items with capacity

## Template — 0/1

```java
int[] dp = new int[W + 1];
for (int i = 0; i < n; i++)
  for (int c = W; c >= w[i]; c--)
    dp[c] = Math.max(dp[c], dp[c - w[i]] + val[i]);
```

```csharp
var dp = new int[W + 1];
for (int i = 0; i < n; i++)
  for (int c = W; c >= w[i]; c--)
    dp[c] = Math.Max(dp[c], dp[c - w[i]] + val[i]);
```

## Further study

- [Knapsack problem](https://en.wikipedia.org/wiki/Knapsack_problem)
- [Dynamic programming](https://en.wikipedia.org/wiki/Dynamic_programming)
- [LeetCode Dynamic Programming tag](https://leetcode.com/tag/dynamic-programming/)

## Practice prompts

1. Partition equal subset sum  
2. Coin change  
3. Target sum
