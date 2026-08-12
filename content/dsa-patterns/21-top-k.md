---
id: pattern-top-k
title: "Pattern: Top K Elements"
track: dsa-patterns
module: "03 Heap Patterns"
order: 31
languages: [java, csharp]
summary: Size-K heap for K largest/smallest/frequent — O(n log k).
---

## Why this matters

Faster than full sort when you only need K extremes.

## Definitions

- **Top K pattern:** Keep a heap of size K while scanning the stream/array.
- **Min-heap of size K:** For K largest — root is the smallest of the large ones.
- **Bucket / quickselect alternatives:** Frequency buckets or average O(n) select.

## Recognition cues

- Kth largest / K closest  
- Top K frequent  
- K smallest pairs  
- Sort characters by frequency (related)

## Template — K largest

```java
PriorityQueue<Integer> pq = new PriorityQueue<>(); // min-heap
for (int x : a) {
  pq.offer(x);
  if (pq.size() > k) pq.poll();
}
```

```csharp
var pq = new PriorityQueue<int, int>();
foreach (int x in a) {
  pq.Enqueue(x, x);
  if (pq.Count > k) pq.Dequeue();
}
```


## Pattern-wise problems (solve in order)

| # | Problem | Level |
|---|---------|-------|
| 1 | [Kth Largest Element in an Array](https://leetcode.com/problems/kth-largest-element-in-an-array/) | Medium |
| 2 | [Top K Frequent Elements](https://leetcode.com/problems/top-k-frequent-elements/) | Medium |
| 3 | [K Closest Points to Origin](https://leetcode.com/problems/k-closest-points-to-origin/) | Medium |
| 4 | [Ugly Number II](https://leetcode.com/problems/ugly-number-ii/) | Medium |

## YouTube (watch after attempting)

- [NeetCode](https://www.youtube.com/@NeetCode) — Top K / Heap
- [Striver Heap](https://www.youtube.com/@takeUforward/playlists)

Master index: **Pattern-Wise Problems + Best YouTube Playlists** (Start Here module).

## Further study

- [Selection algorithm](https://en.wikipedia.org/wiki/Selection_algorithm)
- [LeetCode Heap tag](https://leetcode.com/tag/heap-priority-queue/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Kth largest element  
2. Top K frequent elements  
3. K closest points to origin
