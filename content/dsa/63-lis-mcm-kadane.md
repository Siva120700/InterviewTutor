---
id: dsa-lis-mcm-kadane
title: "DP Classics — LIS, LCS Deep Dive, Kadane, MCM"
track: dsa
module: "07 Dynamic Programming"
order: 64
languages: [java, csharp]
summary: Interview DP staples: longest increasing subsequence, Kadane, and matrix-chain / interval DP.
---

## Why this matters

These four patterns recycle constantly. If you can derive them cold, Medium DP rounds get much easier.

## Definitions

- **LIS:** Longest increasing subsequence — not necessarily contiguous.
- **Patience / tails method:** Maintain smallest tail of all increasing subsequences of each length — O(n log n) LIS length.
- **Kadane:** Max subarray sum via running best ending here.
- **LCS:** Longest common subsequence of two strings (already in DP II — quick refresh).
- **MCM (matrix chain multiplication):** Interval DP choosing optimal parenthesization / split order.

## Worked example 1 — LIS length O(n log n)

```java
int lengthOfLIS(int[] a) {
  List<Integer> tails = new ArrayList<>();
  for (int x : a) {
    int i = Collections.binarySearch(tails, x);
    if (i < 0) i = -i - 1;
    if (i == tails.size()) tails.add(x); else tails.set(i, x);
  }
  return tails.size();
}
```

```csharp
int LengthOfLIS(int[] a) {
  var tails = new List<int>();
  foreach (var x in a) {
    int i = tails.BinarySearch(x);
    if (i < 0) i = ~i;
    if (i == tails.Count) tails.Add(x); else tails[i] = x;
  }
  return tails.Count;
}
```

Classic O(n²): `dp[i] = 1 + max{dp[j] | j<i, a[j]<a[i]}`.

## Worked example 2 — Kadane

```java
int maxSubArray(int[] a) {
  int best = a[0], cur = a[0];
  for (int i = 1; i < a.length; i++) {
    cur = Math.max(a[i], cur + a[i]);
    best = Math.max(best, cur);
  }
  return best;
}
```

```csharp
int MaxSubArray(int[] a) {
  int best = a[0], cur = a[0];
  for (int i = 1; i < a.Length; i++) {
    cur = Math.Max(a[i], cur + a[i]);
    best = Math.Max(best, cur);
  }
  return best;
}
```

## Worked example 3 — MCM style interval DP

Matrices dims `p[0..n]` → matrix i has size `p[i]*p[i+1]`.

```java
int mcm(int[] p) {
  int n = p.length - 1;
  int[][] dp = new int[n][n];
  for (int len = 2; len <= n; len++) {
    for (int i = 0; i + len - 1 < n; i++) {
      int j = i + len - 1;
      dp[i][j] = Integer.MAX_VALUE / 4;
      for (int k = i; k < j; k++) {
        int cost = dp[i][k] + dp[k + 1][j] + p[i] * p[k + 1] * p[j + 1];
        dp[i][j] = Math.min(dp[i][j], cost);
      }
    }
  }
  return dp[0][n - 1];
}
```

Same skeleton as burst balloons / optimal BST: iterate by length, try split `k`.

## LCS refresh

`dp[i][j]` = LCS of `s[:i]`, `t[:j]`.  
If equal chars → `dp[i-1][j-1]+1`, else `max(dp[i-1][j], dp[i][j-1])`.

## Interview Q&A

- **Q:** LIS subsequence vs subarray?  
  **A:** Subsequence can skip; subarray is contiguous (Kadane territory).
- **Q:** Print the LIS?  
  **A:** Keep parent pointers from O(n²) DP, or reconstruct from tails carefully.
- **Q:** Empty / all-negative Kadane?  
  **A:** Initialize with `a[0]`; problem may require non-empty subarray.

## Pitfalls

- Using `<=` vs `<` for non-decreasing LIS  
- MCM index off-by-one on dimension array `p`  
- Confusing LCS with longest common *substring* (DP diagonal continuity)

## 60-second answer

“LIS: O(n²) DP or O(n log n) tails. Kadane: best ending here vs start fresh. LCS: 2D string DP. MCM: interval DP over splits by length — the parent of burst-balloons style problems.”

## Further study

- DP I / DP II / DP III  

## Practice prompts

1. LIS length + one reconstruction  
2. Max subarray / max product variant  
3. Burst Balloons as interval DP
