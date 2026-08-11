---
id: pattern-prefix-hash
title: "Pattern: Prefix Sum + Hashing"
track: dsa-patterns
module: "06 DP Patterns"
order: 62
languages: [java, csharp]
summary: Subarray sum equals K and related hash-map prefix tricks.
---

## Why this matters

Combines hashing with prefix sums for powerful O(n) subarray answers.

## Definitions

- **Prefix sum:** `p[i]` = sum of first i elements; range sum = `p[r]-p[l]`.
- **Prefix + map:** Store frequency of prefix values to count ranges with target sum.
- **Running XOR / mod:** Same idea for XOR or divisible-by-K problems.

## Recognition cues

- Subarray sum equals K  
- Contiguous array (equal 0/1)  
- Continuous subarray sum (multiple of K)  
- Path sum III (tree variant of prefix map)

## Template

```java
Map<Integer, Integer> freq = new HashMap<>();
freq.put(0, 1);
int sum = 0, ans = 0;
for (int x : nums) {
  sum += x;
  ans += freq.getOrDefault(sum - k, 0);
  freq.merge(sum, 1, Integer::sum);
}
```

```csharp
var freq = new Dictionary<int, int> { [0] = 1 };
int sum = 0, ans = 0;
foreach (int x in nums) {
  sum += x;
  if (freq.TryGetValue(sum - k, out int c)) ans += c;
  freq[sum] = freq.GetValueOrDefault(sum) + 1;
}
```

## Further study

- [Prefix sum](https://en.wikipedia.org/wiki/Prefix_sum)
- [LeetCode Hash Table / Prefix Sum tags](https://leetcode.com/tag/prefix-sum/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Subarray sum equals K  
2. Contiguous array  
3. Continuous subarray sum
