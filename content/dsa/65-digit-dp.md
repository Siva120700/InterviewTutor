---
id: dsa-digit-dp
title: Digit DP
track: dsa
module: "07 Dynamic Programming"
order: 65
languages: [java, csharp]
summary: Count numbers with digit constraints using position / tight / leading-zero state.
---

## Why this matters

“How many numbers ≤ R have property P on digits?” is awkward with math alone. Digit DP builds the number left→right with a small state.

## Definitions

- **Digit DP:** DP over digit positions of a number’s decimal representation.
- **Tight flag:** Whether the prefix so far matches the upper bound’s prefix (limits the next digit).
- **Leading zero flag:** Whether we are still placing leading zeros (often so they don’t count as digits for the property).
- **State:** Typically `(pos, tight, lead, …extra property…)`.
- **Range [L,R]:** `f(R) - f(L-1)` with careful `L=0`.

## Template

```java
char[] bound;
Integer[][][] memo; // pos, tight, lead (+ more dims as needed)

int solve(String s) {
  bound = s.toCharArray();
  memo = new Integer[bound.length][2][2];
  return dfs(0, 1, 1);
}
int dfs(int pos, int tight, int lead) {
  if (pos == bound.length) return lead == 1 ? 0 : 1; // count this number (adjust)
  if (memo[pos][tight][lead] != null) return memo[pos][tight][lead];
  int limit = tight == 1 ? bound[pos] - '0' : 9;
  int ans = 0;
  for (int d = 0; d <= limit; d++) {
    int nTight = (tight == 1 && d == limit) ? 1 : 0;
    int nLead = (lead == 1 && d == 0) ? 1 : 0;
    // update extra property from d / nLead
    ans += dfs(pos + 1, nTight, nLead);
  }
  return memo[pos][tight][lead] = ans;
}
```

```csharp
// Same memoized DFS over (pos, tight, lead, …)
```

## Worked intuition — count numbers ≤ R with no digit `4`

At each position choose digit `0..limit` except skip `4` (unless still in leading zeros, depending on statement). Memoize remaining positions under tight/lead.

## Interview Q&A

- **Q:** Why memoize tight?  
  **A:** Same `(pos,tight,lead,prop)` repeats across branches — exponential without cache.
- **Q:** Lower bound L?  
  **A:** Compute `F(R) - F(L-1)`; implement `F` for a single upper bound string.
- **Q:** 64-bit R?  
  **A:** Use the decimal string of R; pos ≤ 19.

## Pitfalls

- Counting leading-zero “numbers” as valid when they shouldn’t  
- Forgetting to clear memo when bound changes  
- Off-by-one on `F(L-1)` when L=0

## 60-second answer

“Digit DP builds numbers digit-by-digit against an upper bound. State is position + tight + leading-zero + problem-specific bits. Answer `[L,R]` as `F(R)-F(L-1)`.”

## Further study

- DP Advanced · Combinatorics  

## Practice prompts

1. Count numbers with unique digits  
2. Digit DP: numbers with given digit sum  
3. Hard: numbers without consecutive 1s in binary (related binary digit DP)
