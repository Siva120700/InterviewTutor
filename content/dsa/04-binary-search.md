---
id: dsa-binary-search
title: Binary Search Mastery
track: dsa
module: "01 Foundations"
order: 4
languages: [java, csharp]
summary: Classic search, lower/upper bound, and binary search on answer — from basic to advanced.
---

## Why this matters

Binary search is O(log n) on sorted data **and** a template for “minimize the maximum” problems.

## Definitions

- **Binary search:** An O(log n) search that repeatedly halves a sorted search space by comparing the midpoint to the target.
- **Search space:** The continuous range of candidate indices or answer values that still might contain the solution.
- **Lower bound:** The first position where a value is ≥ the target in a sorted array (insertion point).
- **Upper bound:** The first position where a value is strictly greater than the target in a sorted array.
- **Binary search on answer:** Searching over a numeric answer range with a monotonic feasibility check (`can(mid)`) instead of over array indices.
- **Monotonic predicate:** A yes/no condition that flips from false→true (or true→false) at most once as the candidate grows — required for binary search on answer.
- **Overflow-safe mid:** Computing `mid` as `lo + (hi - lo) / 2` to avoid integer overflow from `(lo + hi) / 2`.

## Classic template

```java
int indexOf(int[] a, int t) {
  int lo = 0, hi = a.length - 1;
  while (lo <= hi) {
    int mid = lo + (hi - lo) / 2;
    if (a[mid] == t) return mid;
    if (a[mid] < t) lo = mid + 1;
    else hi = mid - 1;
  }
  return -1;
}
```

```csharp
int IndexOf(int[] a, int t) {
  int lo = 0, hi = a.Length - 1;
  while (lo <= hi) {
    int mid = lo + (hi - lo) / 2;
    if (a[mid] == t) return mid;
    if (a[mid] < t) lo = mid + 1;
    else hi = mid - 1;
  }
  return -1;
}
```

## Lower bound (first ≥ t)

```java
int lowerBound(int[] a, int t) {
  int lo = 0, hi = a.length; // hi exclusive
  while (lo < hi) {
    int mid = lo + (hi - lo) / 2;
    if (a[mid] < t) lo = mid + 1;
    else hi = mid;
  }
  return lo;
}
```

## Binary search on answer — split array largest sum

Find minimal largest-split-sum with ≤ k parts.

```java
boolean can(int[] a, int k, int mid) {
  int parts = 1, sum = 0;
  for (int x : a) {
    if (x > mid) return false;
    if (sum + x > mid) { parts++; sum = 0; }
    sum += x;
  }
  return parts <= k;
}
int splitArray(int[] a, int k) {
  int lo = 0, hi = 0;
  for (int x : a) { lo = Math.max(lo, x); hi += x; }
  while (lo < hi) {
    int mid = lo + (hi - lo) / 2;
    if (can(a, k, mid)) hi = mid; else lo = mid + 1;
  }
  return lo;
}
```

```csharp
bool Can(int[] a, int k, int mid) {
  int parts = 1, sum = 0;
  foreach (int x in a) {
    if (x > mid) return false;
    if (sum + x > mid) { parts++; sum = 0; }
    sum += x;
  }
  return parts <= k;
}
```

## Interview Q&A

- **Q:** Overflow on mid?
  **A:** Use `lo + (hi - lo) / 2`, not `(lo + hi) / 2` with large ints.
- **Q:** When on answer?
  **A:** Monotonic predicate: larger budget always “more feasible”.

## Pitfalls

- Infinite loops from wrong `lo/hi` updates  
- Off-by-one on inclusive vs exclusive bounds

## 60-second answer

“Binary search halves a monotonic search space. I use it on sorted arrays and on answer ranges with a feasibility check — classic for minimize-the-maximum problems.”

## Further study

- [Binary search algorithm (Wikipedia)](https://en.wikipedia.org/wiki/Binary_search_algorithm) — classic halving search on sorted data
- [Binary search (Wikipedia)](https://en.wikipedia.org/wiki/Binary_search) — related overview of the technique
- [Bisection method (Wikipedia)](https://en.wikipedia.org/wiki/Bisection_method) — continuous analog of binary search on answer
- [Arrays (Java SE)](https://docs.oracle.com/en/java/javase/17/docs/api/java.base/java/util/Arrays.html) — JDK binarySearch and sorted-array helpers

## Practice prompts

1. Search in rotated sorted array  
2. Koko eating bananas  
3. Median of two sorted arrays (advanced)
