---
id: dsa-arrays-basics
title: Arrays from Scratch
track: dsa
module: "01 Foundations"
order: 2
languages: [java, csharp]
summary: Indexing, in-place updates, prefix sums, and rotation — array fundamentals before patterns.
---

## Why this matters

Arrays are the substrate for most Easy/Medium problems. Master scans, in-place writes, and prefix sums before patterns.

## Definitions

- **Array:** A contiguous block of elements with O(1) random access by index and O(n) insert/delete in the middle (elements must shift).
- **Dynamic array:** A resizable array (`ArrayList`/`List`) that amortizes append to O(1) via occasional capacity doubling.
- **In-place algorithm:** An algorithm that rearranges or updates the input using only O(1) (or small) extra memory.
- **Prefix sum:** A precomputed array where each entry stores the sum up to that index, enabling O(1) range-sum queries after O(n) preprocess.
- **Two-pointer reverse:** Swapping from both ends moving inward to reverse an array in O(n) time and O(1) space.
- **Rotation:** Cyclically shifting elements left or right by k positions (often via reverse tricks or modular indexing).
- **Index invariant:** A clear meaning for each index (inclusive vs exclusive bounds) that prevents off-by-one bugs in range math.

## Concept

Contiguous memory, O(1) index access, O(n) insert/delete in the middle.

### Static vs dynamic arrays

| | Static (`int[]`) | Dynamic (`ArrayList` / `List<T>`) |
|---|------------------|-----------------------------------|
| Size | Fixed at creation | Grows via capacity doubling |
| Index math | `base + i * stride` | Same under the hood |
| Append | N/A (or manual copy) | Amortized O(1); occasional O(n) resize |
| Random access | O(1) | O(1) |

**Resizing strategy:** when full, allocate ~2× capacity, copy elements, replace buffer — total cost over n appends is still \(O(n)\).

## Worked example 1 — Reverse in place

```java
void reverse(int[] a) {
  int l = 0, r = a.length - 1;
  while (l < r) {
    int tmp = a[l]; a[l] = a[r]; a[r] = tmp;
    l++; r--;
  }
}
```

```csharp
void Reverse(int[] a) {
  int l = 0, r = a.Length - 1;
  while (l < r) {
    (a[l], a[r]) = (a[r], a[l]);
    l++; r--;
  }
}
```

## Worked example 2 — Prefix sums

Range sum queries in O(1) after O(n) preprocess.

```java
int[] buildPrefix(int[] a) {
  int[] p = new int[a.length + 1];
  for (int i = 0; i < a.length; i++) p[i + 1] = p[i] + a[i];
  return p; // sum[l..r] = p[r+1] - p[l]
}
```

```csharp
int[] BuildPrefix(int[] a) {
  var p = new int[a.Length + 1];
  for (int i = 0; i < a.Length; i++) p[i + 1] = p[i] + a[i];
  return p;
}
```

## Worked example 3 — Rotate right by k

```java
void rotate(int[] a, int k) {
  int n = a.length; k %= n;
  reverse(a, 0, n - 1);
  reverse(a, 0, k - 1);
  reverse(a, k, n - 1);
}
void reverse(int[] a, int l, int r) {
  while (l < r) { int t = a[l]; a[l++] = a[r]; a[r--] = t; }
}
```

```csharp
void Rotate(int[] a, int k) {
  int n = a.Length; k %= n;
  Array.Reverse(a, 0, n);
  Array.Reverse(a, 0, k);
  Array.Reverse(a, k, n - k);
}
```

## Interview Q&A

- **Q:** Array vs ArrayList?
  **A:** Fixed vs growable; both O(1) index; inserts shift elements O(n).
- **Q:** When prefix sums?
  **A:** Many range-sum queries; also subarray-sum patterns with hashing.

## Pitfalls

- Integer overflow on sums — use `long`  
- Off-by-one on inclusive ranges  
- Mutating input when not allowed

## 60-second answer

“Arrays give O(1) access. I use in-place two-ended swaps, prefix sums for ranges, and careful index math. Dynamic lists trade occasional resize for flexible size.”

## Further study

- [Array (Wikipedia)](https://en.wikipedia.org/wiki/Array_(data_structure)) — contiguous storage and O(1) indexing
- [Dynamic array (Wikipedia)](https://en.wikipedia.org/wiki/Dynamic_array) — capacity doubling and amortized append
- [Prefix sum (Wikipedia)](https://en.wikipedia.org/wiki/Prefix_sum) — O(1) range sums after O(n) preprocess
- [List (Java SE)](https://docs.oracle.com/en/java/javase/17/docs/api/java.base/java/util/List.html) — Java list contract for interviews

## Practice prompts

1. Product of array except self  
2. Maximum subarray (Kadane)  
3. Merge two sorted arrays
