---
id: dsa-prefix-sum-line-sweep
title: Prefix Sum and Line Sweep
track: dsa
module: "02 Patterns"
order: 13
languages: [java, csharp]
summary: Range sums, difference arrays, and sweep-line events for intervals on a line.
---

## Why this matters

Prefix sums turn many range queries into O(1). Difference arrays batch range updates. Line sweep processes intervals/events in sorted order — classic for coverage and skyline-style problems.

## Definitions

- **Prefix sum:** `p[i] = a[0]+…+a[i-1]`; range `[L,R]` = `p[R+1]-p[L]`.
- **Difference array:** Encode range add as `d[L]+=x`, `d[R+1]-=x`, then prefix to materialize.
- **Line sweep:** Sort events (start/end) and scan left→right maintaining active state.
- **Coverage count:** How many intervals cover the current x while sweeping.
- **Immutable prefix vs Fenwick:** Prefix alone can’t support arbitrary point updates efficiently.

## Worked example 1 — Subarray sum equals k (hash + prefix)

```java
int subarraySum(int[] a, int k) {
  Map<Integer, Integer> freq = new HashMap<>();
  freq.put(0, 1);
  int sum = 0, ans = 0;
  for (int x : a) {
    sum += x;
    ans += freq.getOrDefault(sum - k, 0);
    freq.merge(sum, 1, Integer::sum);
  }
  return ans;
}
```

```csharp
int SubarraySum(int[] a, int k) {
  var freq = new Dictionary<int, int> { [0] = 1 };
  int sum = 0, ans = 0;
  foreach (var x in a) {
    sum += x;
    if (freq.TryGetValue(sum - k, out var c)) ans += c;
    freq[sum] = freq.GetValueOrDefault(sum) + 1;
  }
  return ans;
}
```

## Worked example 2 — Difference array range updates

```java
void rangeAdd(int[] d, int l, int r, int x) { // inclusive l..r on array size n
  d[l] += x;
  if (r + 1 < d.length) d[r + 1] -= x;
}
int[] materialize(int[] d) {
  int[] a = new int[d.length];
  int run = 0;
  for (int i = 0; i < d.length; i++) { run += d[i]; a[i] = run; }
  return a;
}
```

## Worked example 3 — Sweep meeting rooms intuition

Sort starts and ends; scan with a counter of open intervals — max counter = rooms needed.

## Interview Q&A

- **Q:** Prefix vs sliding window?  
  **A:** Window for contiguous constraints with monotonic growth; prefix+hash for arbitrary target sums (including negatives).
- **Q:** 2D prefix?  
  **A:** Inclusion-exclusion on rectangle corners after O(mn) preprocess.

## Pitfalls

- Off-by-one on inclusive ranges  
- Integer overflow — use `long`  
- Forgetting `freq[0]=1` for subarray-sum

## 60-second answer

“Prefix sums answer range totals fast. Difference arrays batch range adds. Line sweep sorts boundary events and maintains active coverage as x advances.”

## Further study

- [Prefix sum](https://en.wikipedia.org/wiki/Prefix_sum)
- [Sweep line algorithm](https://en.wikipedia.org/wiki/Sweep_line_algorithm)

## Practice prompts

1. Corporate flight bookings (difference array)  
2. Car pooling  
3. Maximum population year
