---
id: dsa-sorting
title: Sorting Algorithms
track: dsa
module: "01 Foundations"
order: 9
languages: [java, csharp]
summary: Bubble through radix/bucket/cyclic sort — when to use each and interview complexities.
---

## Why this matters

Sorting underpins binary search, two pointers, and many greedy proofs. Interviews expect complexities, stability, and when library sort is enough vs implementing a specific algorithm.

## Definitions

- **Comparison sort:** Relies on pairwise compares — lower bound \(\Omega(n \log n)\) in the algebraic decision-tree model.
- **Stable sort:** Equal keys keep relative order (merge sort yes; quicksort typical no).
- **In-place:** Uses O(1) (or tiny) extra memory beyond the input.
- **Partition:** Quicksort step that places a pivot and splits lesser/greater sides.
- **Counting / radix / bucket:** Non-comparison sorts when keys have limited range or digit structure.
- **Cyclic sort:** Place each value at index `value - 1` when numbers are a permutation of `1..n`.

## Cheat sheet

| Algorithm | Best / Avg / Worst | Extra space | Stable | Notes |
|-----------|--------------------|-------------|--------|-------|
| Bubble | n / n² / n² | O(1) | Yes | Teaching only |
| Insertion | n / n² / n² | O(1) | Yes | Great on nearly sorted |
| Selection | n² / n² / n² | O(1) | No | Few swaps |
| Merge | n log n | O(n) | Yes | Predictable |
| Quick | n log n / n log n / n² | O(log n) | No* | Library default often dual-pivot |
| Heap sort | n log n | O(1) | No | Guaranteed |
| Counting | n + k | O(k) | Yes | Small integer range k |
| Radix | d(n+k) | O(n+k) | Yes | Digit passes |
| Bucket | n + k typical | O(n+k) | Yes* | Uniform distribution |
| Cyclic | n | O(1) | — | Permutation / missing number |

## Worked example 1 — Merge sort merge

```java
void merge(int[] a, int lo, int mid, int hi, int[] tmp) {
  int i = lo, j = mid, k = lo;
  while (i < mid && j < hi)
    tmp[k++] = a[i] <= a[j] ? a[i++] : a[j++];
  while (i < mid) tmp[k++] = a[i++];
  while (j < hi) tmp[k++] = a[j++];
  for (k = lo; k < hi; k++) a[k] = tmp[k];
}
```

```csharp
void Merge(int[] a, int lo, int mid, int hi, int[] tmp) {
  int i = lo, j = mid, k = lo;
  while (i < mid && j < hi)
    tmp[k++] = a[i] <= a[j] ? a[i++] : a[j++];
  while (i < mid) tmp[k++] = a[i++];
  while (j < hi) tmp[k++] = a[j++];
  for (k = lo; k < hi; k++) a[k] = tmp[k];
}
```

## Worked example 2 — Cyclic sort placement

```java
void cyclicSort(int[] a) {
  int i = 0;
  while (i < a.length) {
    int j = a[i] - 1;
    if (a[i] != a[j]) { int t = a[i]; a[i] = a[j]; a[j] = t; }
    else i++;
  }
}
```

## Interview Q&A

- **Q:** Why not always quicksort?  
  **A:** Worst-case \(O(n^2)\) without care; merge is stable and predictable; counting wins on tiny alphabets.
- **Q:** Is `Arrays.sort` / `Array.Sort` enough?  
  **A:** Usually yes in interviews unless they ask you to implement or need a custom comparator with proven complexity.
- **Q:** Custom sort string by order?  
  **A:** Count frequency then emit in custom order — often O(n).

## Pitfalls

- Confusing average with worst case for quicksort  
- Using counting sort with huge `k` (memory blowup)  
- Forgetting stability when sorting by secondary key

## 60-second answer

“I default to library sort. If asked to implement: merge for stable predictable n log n, quick for average speed, counting/radix when keys are small integers, cyclic when values are a 1..n permutation.”

## Further study

- [Sorting algorithm](https://en.wikipedia.org/wiki/Sorting_algorithm)
- Practice sheet: Sorting subgroups (bubble → custom)

## Practice prompts

1. Implement merge sort and prove \(\Theta(n \log n)\)  
2. Sort colors / Dutch national flag in one pass  
3. Find missing number with cyclic sort
