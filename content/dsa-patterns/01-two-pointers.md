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

## Further study

- [NeetCode](https://neetcode.io/) — pattern-oriented practice roadmap.
- [LeetCode Two Pointers tag](https://leetcode.com/tag/two-pointers/) — drill set.
- [Big-O cheat sheet](https://www.bigocheatsheet.com/) — complexity while practicing.

## Practice prompts

1. 3Sum (sort + two pointers)  
2. Container with most water  
3. Remove duplicates from sorted array
