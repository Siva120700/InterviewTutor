---
id: pattern-sliding-window
title: "Pattern: Sliding Window"
track: dsa-patterns
module: "01 Linear Patterns"
order: 11
languages: [java, csharp]
summary: Fixed and variable windows for contiguous subarray/substring constraints.
---

## Why this matters

Default pattern for “longest/shortest contiguous …” problems. O(n) if each index enters/leaves once.

## Definitions

- **Sliding window:** Contiguous range `[left, right]` expanded/shrunk while maintaining a constraint.
- **Fixed window:** Constant size `k`; slide by one.
- **Variable window:** Grow right; shrink left until valid.
- **Window state:** Sum, counts, set — updated in O(1)/O(k) per move.

## Recognition cues

- **Contiguous** subarray/substring  
- “Longest / shortest / at most K / exactly K”  
- Character frequency constraints  
- Max sum of size k

## Template (variable)

```java
int left = 0;
for (int right = 0; right < n; right++) {
  add(a[right]);
  while (!valid()) { remove(a[left]); left++; }
  updateAnswer(left, right);
}
```

```csharp
int left = 0;
for (int right = 0; right < n; right++) {
  Add(a[right]);
  while (!Valid()) { Remove(a[left]); left++; }
  UpdateAnswer(left, right);
}
```

## Worked example — longest unique substring

```java
public int lengthOfLongestSubstring(String s) {
  Map<Character, Integer> last = new HashMap<>();
  int best = 0, left = 0;
  for (int r = 0; r < s.length(); r++) {
    char c = s.charAt(r);
    if (last.containsKey(c)) left = Math.max(left, last.get(c) + 1);
    last.put(c, r);
    best = Math.max(best, r - left + 1);
  }
  return best;
}
```

```csharp
public int LengthOfLongestSubstring(string s) {
  var last = new Dictionary<char, int>();
  int best = 0, left = 0;
  for (int r = 0; r < s.Length; r++) {
    if (last.TryGetValue(s[r], out int prev)) left = Math.Max(left, prev + 1);
    last[s[r]] = r;
    best = Math.Max(best, r - left + 1);
  }
  return best;
}
```

## When NOT to use

- Non-contiguous subsequences  
- Negative numbers with some “max sum of size ≤ k” variants (may need other tools)

## Further study

- [Sliding window (CS concept)](https://en.wikipedia.org/wiki/Sliding_window_protocol) — related idea; interview use is subarray windows.
- [LeetCode Sliding Window tag](https://leetcode.com/tag/sliding-window/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Max sum subarray of size k  
2. Minimum window substring  
3. Longest substring with at most K distinct
