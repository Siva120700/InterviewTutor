---
id: pattern-cyclic-sort
title: "Pattern: Cyclic Sort"
track: dsa-patterns
module: "01 Linear Patterns"
order: 14
languages: [java, csharp]
summary: Place numbers at index = value — missing/duplicate in 1..n arrays.
---

## Why this matters

When numbers are in a small range like `1..n`, cyclic sort finds missing/duplicates in O(n) time and O(1) space.

## Definitions

- **Cyclic sort:** Repeatedly swap `nums[i]` into index `nums[i] - 1` until every value sits in its “home” index.
- **Home index:** For value `v` in `1..n`, index `v - 1`.
- **Missing number:** After placement, index `i` without `i+1` is missing.

## Recognition cues

- Array of size n with values in `1..n` or `0..n`  
- Find missing / duplicate / first missing positive (variant)  
- In-place, O(1) extra space requested

## Template

```java
int i = 0;
while (i < n) {
  int correct = nums[i] - 1;
  if (nums[i] >= 1 && nums[i] <= n && nums[i] != nums[correct])
    swap(nums, i, correct);
  else i++;
}
```

```csharp
int i = 0;
while (i < n) {
  int correct = nums[i] - 1;
  if (nums[i] >= 1 && nums[i] <= n && nums[i] != nums[correct])
    (nums[i], nums[correct]) = (nums[correct], nums[i]);
  else i++;
}
```


## Pattern-wise problems (solve in order)

| # | Problem | Level |
|---|---------|-------|
| 1 | [Missing Number](https://leetcode.com/problems/missing-number/) | Easy |
| 2 | [Find All Numbers Disappeared in an Array](https://leetcode.com/problems/find-all-numbers-disappeared-in-an-array/) | Easy |
| 3 | [Find the Duplicate Number](https://leetcode.com/problems/find-the-duplicate-number/) | Medium |
| 4 | [Set Mismatch](https://leetcode.com/problems/set-mismatch/) | Easy |
| 5 | [First Missing Positive](https://leetcode.com/problems/first-missing-positive/) | Hard |

## YouTube (watch after attempting)

- [NeetCode](https://www.youtube.com/@NeetCode) — Missing Number / First Missing Positive
- Search “cyclic sort pattern” on YouTube after attempting
- [Striver Arrays](https://www.youtube.com/@takeUforward)

Master index: **Pattern-Wise Problems + Best YouTube Playlists** (Start Here module).

## Further study

- [Cycle sort](https://en.wikipedia.org/wiki/Cycle_sort) — related sorting algorithm.
- [LeetCode](https://leetcode.com/) — “missing number”, “find all duplicates” family.
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Missing number  
2. Find all duplicates in array  
3. First missing positive
