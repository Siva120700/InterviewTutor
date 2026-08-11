---
id: pattern-merge-intervals
title: "Pattern: Merge Intervals"
track: dsa-patterns
module: "01 Linear Patterns"
order: 13
languages: [java, csharp]
summary: Sort by start, merge overlaps — meetings, ranges, and coverage.
---

## Why this matters

Interval problems collapse once you sort. Pattern covers merge, insert, and meeting rooms.

## Definitions

- **Interval:** Range `[start, end]` (clarify inclusive/exclusive).
- **Overlap:** `a.start <= b.end && b.start <= a.end` (after sorting usually `curr.start <= last.end`).
- **Merge:** Combine overlapping intervals into one spanning range.
- **Sweep line (related):** Process starts/ends as events for counting overlaps.

## Recognition cues

- List of intervals / meetings / ranges  
- Merge overlapping  
- Minimum rooms / max concurrent  
- Insert interval into sorted list

## Template

```java
Arrays.sort(intervals, Comparator.comparingInt(a -> a[0]));
List<int[]> res = new ArrayList<>();
int[] cur = intervals[0];
for (int i = 1; i < intervals.length; i++) {
  if (intervals[i][0] <= cur[1]) cur[1] = Math.max(cur[1], intervals[i][1]);
  else { res.add(cur); cur = intervals[i]; }
}
res.add(cur);
```

```csharp
Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));
var res = new List<int[]>();
var cur = intervals[0];
for (int i = 1; i < intervals.Length; i++) {
  if (intervals[i][0] <= cur[1]) cur[1] = Math.Max(cur[1], intervals[i][1]);
  else { res.Add(cur); cur = intervals[i]; }
}
res.Add(cur);
```

## Further study

- [Interval graph (overview)](https://en.wikipedia.org/wiki/Interval_graph) — theory background.
- [LeetCode Array tag — interval problems](https://leetcode.com/tag/array/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Merge intervals  
2. Insert interval  
3. Meeting rooms II (min heaps of end times)
