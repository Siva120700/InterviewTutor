---
id: pattern-bitwise-xor
title: "Pattern: Bitwise XOR"
track: dsa-patterns
module: "06 DP Patterns"
order: 61
languages: [java, csharp]
summary: XOR tricks for single number, missing number, and bit counting patterns.
---

## Why this matters

XOR cancels pairs. Great for “find the unique” with O(1) space.

## Definitions

- **XOR (^):** Bitwise exclusive or; `a ^ a = 0`, `a ^ 0 = a`, commutative/associative.
- **Single number pattern:** XOR all values — duplicates cancel, unique remains.
- **Bit mask:** Use bits to represent subsets when n ≤ 20 (related advanced pattern).

## Recognition cues

- Single number (others appear twice)  
- Missing number with XOR 0..n  
- Find two unique numbers (bit partition variant)

## Template

```java
int single = 0;
for (int x : nums) single ^= x;
return single;
```

```csharp
int single = 0;
foreach (int x in nums) single ^= x;
return single;
```

## Further study

- [Bitwise operation](https://en.wikipedia.org/wiki/Bitwise_operation)
- [LeetCode Bit Manipulation tag](https://leetcode.com/tag/bit-manipulation/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Single number  
2. Missing number (XOR approach)  
3. Single number III
