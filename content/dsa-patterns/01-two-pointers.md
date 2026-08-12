---
id: pattern-two-pointers
title: "Pattern: Two Pointers"
track: dsa-patterns
module: "01 Linear Patterns"
order: 10
languages: [java, csharp]
summary: Opposite-ends and same-direction pointers — cues, template, and classic problems.
---

## Why this matters

One of the highest-frequency interview patterns. Turns many O(n²) scans into O(n) on sorted or partitionable data.

## Definitions

- **Two pointers:** Maintain two indices that move according to a rule to explore pairs or partitions in linear time.
- **Opposite-ends:** `left` at start, `right` at end; move based on comparison/sum.
- **Same-direction (slow/fast write):** Fast scans; slow writes the next valid position (dedupe, remove element).
- **Monotonic discard:** Each move permanently eliminates options because of sorted order or invariant.

## Recognition cues

- Array/string is **sorted** (or can be sorted)  
- Find pair/triplet with target sum  
- Remove duplicates **in-place**  
- Container / trapping water style width problems  
- Palindrome check from both ends

## Template

```java
int l = 0, r = n - 1;
while (l < r) {
  // decide based on a[l], a[r]
  if (needBigger) l++;
  else if (needSmaller) r--;
  else { /* found */ l++; r--; }
}
```

```csharp
int l = 0, r = n - 1;
while (l < r) {
  if (needBigger) l++;
  else if (needSmaller) r--;
  else { l++; r--; }
}
```

## Worked example — two sum sorted

```java
public int[] twoSum(int[] a, int t) {
  int l = 0, r = a.length - 1;
  while (l < r) {
    int s = a[l] + a[r];
    if (s == t) return new int[]{l, r};
    if (s < t) l++; else r--;
  }
  return new int[]{-1, -1};
}
```

```csharp
public int[] TwoSum(int[] a, int t) {
  int l = 0, r = a.Length - 1;
  while (l < r) {
    int s = a[l] + a[r];
    if (s == t) return new[] { l, r };
    if (s < t) l++; else r--;
  }
  return new[] { -1, -1 };
}
```

## When NOT to use

- Unsorted input where sorting changes the answer (indices) and hashing is better  
- Need all subsequences (not contiguous / not ends)

## Pattern-wise problems (solve in order)

| # | Problem | Level |
|---|---------|-------|
| 1 | [Valid Palindrome](https://leetcode.com/problems/valid-palindrome/) | Easy |
| 2 | [Two Sum II](https://leetcode.com/problems/two-sum-ii-input-array-is-sorted/) | Medium |
| 3 | [3Sum](https://leetcode.com/problems/3sum/) | Medium |
| 4 | [Container With Most Water](https://leetcode.com/problems/container-with-most-water/) | Medium |
| 5 | [Trapping Rain Water](https://leetcode.com/problems/trapping-rain-water/) | Hard |

## YouTube (watch after attempting)

- [NeetCode](https://www.youtube.com/@NeetCode) — search each problem title  
- [Blind 75 playlist](https://www.youtube.com/playlist?list=PLot-Xpze53ldVwtstag2TL4HQhAnC8ATf)  
- [Striver / takeUforward](https://www.youtube.com/@takeUforward) — Two Pointer / A2Z sheet videos  

## Further study

- Master list: **Pattern-Wise Problems + Best YouTube Playlists** in this track  
- [LeetCode Two Pointers tag](https://leetcode.com/tag/two-pointers/)

## Practice prompts

1. Code opposite-ends template from memory  
2. Solve Two Sum II + 3Sum same day  
3. Explain when hashing beats two pointers
