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

## Pattern-wise problems (solve in order)

| # | Problem | Level |
|---|---------|-------|
| 1 | [Longest Substring Without Repeating Characters](https://leetcode.com/problems/longest-substring-without-repeating-characters/) | Medium |
| 2 | [Maximum Average Subarray I](https://leetcode.com/problems/maximum-average-subarray-i/) | Easy |
| 3 | [Longest Repeating Character Replacement](https://leetcode.com/problems/longest-repeating-character-replacement/) | Medium |
| 4 | [Minimum Window Substring](https://leetcode.com/problems/minimum-window-substring/) | Hard |
| 5 | [Sliding Window Maximum](https://leetcode.com/problems/sliding-window-maximum/) | Hard |

## YouTube (watch after attempting)

- [NeetCode Sliding Window playlist](https://www.youtube.com/playlist?list=PLot-Xpze53leOBgcVsJBEGrHPd_7x_koV) — **best starting point**  
- [Aditya Verma Sliding Window](https://www.youtube.com/playlist?list=PL_z_8CaSLPWeM8BDJmIYDaoQ5zuwyxnfj) — excellent pedagogy  
- [Striver A2Z — Sliding Window & Two Pointer](https://takeuforward.org/strivers-a2z-dsa-course/strivers-a2z-dsa-course-sheet-2/)  

## Further study

- Master list: **Pattern-Wise Problems + Best YouTube Playlists**  
- [LeetCode Sliding Window tag](https://leetcode.com/tag/sliding-window/)

## Practice prompts

1. Fixed-size max sum of k  
2. Variable window with HashMap counts  
3. Re-derive Minimum Window Substring after the video
