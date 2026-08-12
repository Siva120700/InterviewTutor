---
id: dsa-fenwick
title: Fenwick Trees (Binary Indexed Trees)
track: dsa
module: "08 Advanced Tools"
order: 72
languages: [java, csharp]
summary: Point updates and prefix sums in O(log n) with a compact binary indexed tree.
---

## Why this matters

Fenwick trees are the lightest structure for “update index, query prefix/range sum.” Frequency of appearance in contests and interviews is high once you’re past basics.

## Definitions

- **Fenwick tree (BIT):** An array `bit[1..n]` where each index responsible for a range ending at that index, sized by the lowest set bit.
- **Prefix sum:** `sum(k)` = total of `a[1]+…+a[k]` (1-based mentally).
- **Point update:** Add `delta` at index `i`, propagating through responsible BIT indices.
- **Range sum:** `sum(r) - sum(l-1)`.
- **Lowest set bit:** `i & -i` — stride used to walk the tree.

## Concept

Index `i` covers a segment of length `i & -i` ending at `i`.

```text
i:     1  2  3  4  5  6  7  8
cover: 1  2  1  4  1  2  1  8   (lengths)
```

## Worked example — BIT

```java
class Fenwick {
  final int[] bit; // 1-based
  Fenwick(int n) { bit = new int[n + 1]; }
  void add(int i, int delta) { // i is 1-based
    for (; i < bit.length; i += i & -i) bit[i] += delta;
  }
  int sum(int i) {
    int s = 0;
    for (; i > 0; i -= i & -i) s += bit[i];
    return s;
  }
  int range(int l, int r) { return sum(r) - sum(l - 1); }
}
```

```csharp
class Fenwick {
  readonly int[] bit;
  public Fenwick(int n) => bit = new int[n + 1];
  public void Add(int i, int delta) {
    for (; i < bit.Length; i += i & -i) bit[i] += delta;
  }
  public int Sum(int i) {
    int s = 0;
    for (; i > 0; i -= i & -i) s += bit[i];
    return s;
  }
  public int Range(int l, int r) => Sum(r) - Sum(l - 1);
}
```

Build from array: `add(i+1, a[i])` for each i (0-based → 1-based).

## vs Segment tree

| | Fenwick | Segment tree |
|---|---------|--------------|
| Code size | Tiny | Larger |
| Prefix/range sum | Excellent | Excellent |
| Range min / lazy | Awkward | Natural |
| Memory | ~n | ~4n |

## Interview Q&A

- **Q:** Why 1-based?  
  **A:** Bit tricks with `i & -i` are cleaner; index 0 is unused.
- **Q:** Inversion count?  
  **A:** Classic — Fenwick/segment tree over compressed ranks while scanning.
- **Q:** 2D Fenwick?  
  **A:** Nested loops on both indices — know it exists for matrix problems.

## Pitfalls

- Mixing 0-based problem indices with 1-based BIT  
- `i & -i` on 0 (infinite loop)  
- Using int when sums overflow — prefer `long`

## 60-second answer

“A Fenwick tree supports point add and prefix sum in O(log n) via lowest-set-bit jumps. Range sum is two prefixes. I pick it when the aggregate is invertible like sum; otherwise I reach for a segment tree.”

## Further study

- [Fenwick tree](https://en.wikipedia.org/wiki/Fenwick_tree)
- CP-Algorithms: Fenwick tree

## Practice prompts

1. Build BIT from `[1,2,3,4]` and query range `[2,4]`  
2. Count inversions with Fenwick  
3. Implement frequency of values in a stream with updates
