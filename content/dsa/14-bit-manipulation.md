---
id: dsa-bit-manipulation
title: Bit Manipulation
track: dsa
module: "02 Patterns"
order: 14
languages: [java, csharp]
summary: AND/OR/XOR tricks, masks, and classic bit interview patterns.
---

## Why this matters

Bits unlock constant-space tricks (single number, subsets masks, DP on masks) and low-level interview favorites.

## Definitions

- **Bit:** 0/1 digit in binary representation.
- **Mask:** An integer whose bits mark a subset or flags.
- **AND `&`:** Bit is 1 only if both are 1 — clear/check bits.
- **OR `|`:** Bit is 1 if either is 1 — set bits.
- **XOR `^`:** Bit is 1 if different — toggle; `x^x=0`, `x^0=x`.
- **Shift:** `<<` multiply-ish by 2; `>>` arithmetic shift (sign-extend in Java/C# for signed ints).
- **Lowest set bit:** `x & -x` isolates the rightmost 1-bit.

## Core identities

```text
x & (x-1)     // clear lowest set bit
x & -x        // isolate lowest set bit
x | (1<<k)    // set bit k
x & ~(1<<k)   // clear bit k
x ^ (1<<k)    // toggle bit k
(x >> k) & 1  // test bit k
```

## Worked example 1 — Single number (XOR fold)

```java
int singleNumber(int[] a) {
  int x = 0;
  for (int v : a) x ^= v;
  return x;
}
```

```csharp
int SingleNumber(int[] a) {
  int x = 0;
  foreach (var v in a) x ^= v;
  return x;
}
```

## Worked example 2 — Count bits / Hamming weight

```java
int hammingWeight(int n) {
  int c = 0;
  while (n != 0) { n &= n - 1; c++; }
  return c;
}
```

```csharp
int HammingWeight(uint n) {
  int c = 0;
  while (n != 0) { n &= n - 1; c++; }
  return c;
}
```

## Worked example 3 — Subsets via masks

```java
List<List<Integer>> subsets(int[] nums) {
  int n = nums.length;
  List<List<Integer>> res = new ArrayList<>();
  for (int mask = 0; mask < (1 << n); mask++) {
    List<Integer> cur = new ArrayList<>();
    for (int i = 0; i < n; i++) if ((mask & (1 << i)) != 0) cur.add(nums[i]);
    res.add(cur);
  }
  return res;
}
```

## Interview Q&A

- **Q:** Signed right shift vs unsigned?  
  **A:** Java `>>>` / C# `>>>` (or cast to uint) for logical shift.
- **Q:** Two’s complement?  
  **A:** `-x == ~x + 1` — explains `x & -x`.
- **Q:** When DP bitmask?  
  **A:** State spaces with n ≤ ~20 subsets of items/cities (TSP-style).

## Pitfalls

- Assuming `1 << 31` is positive in signed 32-bit  
- Using `>>` when you meant logical shift  
- Off-by-one in mask loop bounds `1<<n`

## 60-second answer

“I use XOR for parity/unique elements, masks for subsets, and `x&(x-1)` to iterate set bits. Always state signedness and word size.”

## Further study

- [Bitwise operation](https://en.wikipedia.org/wiki/Bitwise_operation)
- [Two’s complement](https://en.wikipedia.org/wiki/Two%27s_complement)

## Practice prompts

1. Missing number via XOR  
2. Reverse bits  
3. Maximum XOR of two numbers in an array (trie follow-up)
