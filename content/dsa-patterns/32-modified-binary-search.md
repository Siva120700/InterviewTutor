---
id: pattern-mod-binary-search
title: "Pattern: Modified Binary Search"
track: dsa-patterns
module: "05 Search Patterns"
order: 50
languages: [java, csharp]
summary: Binary search on arrays and on answer space — rotated arrays, bounds, feasibility.
---

## Why this matters

Beyond “find exact value”: lower bound, rotated arrays, and binary search on answer.

## Definitions

- **Modified binary search:** Same halving idea with a custom mid decision.
- **Lower/upper bound:** First position ≥ / > target.
- **Binary search on answer:** Search the value space with a monotonic `can(mid)` check.

## Recognition cues

- Sorted / rotated sorted array  
- Search insert position  
- Min in rotated array  
- Koko eating bananas / split array largest sum  
- Peak element

## Template — on answer

```java
int lo = minPossible, hi = maxPossible;
while (lo < hi) {
  int mid = lo + (hi - lo) / 2;
  if (can(mid)) hi = mid; else lo = mid + 1;
}
return lo;
```

```csharp
int lo = minPossible, hi = maxPossible;
while (lo < hi) {
  int mid = lo + (hi - lo) / 2;
  if (Can(mid)) hi = mid; else lo = mid + 1;
}
return lo;
```

## Further study

- [Binary search](https://en.wikipedia.org/wiki/Binary_search_algorithm)
- [LeetCode Binary Search tag](https://leetcode.com/tag/binary-search/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Search in rotated sorted array  
2. Find first and last position  
3. Koko eating bananas
