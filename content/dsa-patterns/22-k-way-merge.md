---
id: pattern-k-way-merge
title: "Pattern: K-way Merge"
track: dsa-patterns
module: "03 Heap Patterns"
order: 32
languages: [java, csharp]
summary: Merge K sorted lists/arrays with a min-heap of heads.
---

## Why this matters

Merge K sorted lists is a classic heap pattern — O(N log K).

## Definitions

- **K-way merge:** Repeatedly take the smallest current head among K sorted streams.
- **Heap node:** Stores value + which list/index it came from.

## Recognition cues

- Merge K sorted lists  
- Smallest range covering elements from K lists  
- Find K pairs with smallest sums (related)

## Template

```java
PriorityQueue<Node> pq = new PriorityQueue<>(Comparator.comparingInt(n -> n.val));
for (each non-empty list) pq.offer(head);
while (!pq.isEmpty()) {
  Node cur = pq.poll();
  append(cur.val);
  if (cur.next != null) pq.offer(cur.next);
}
```

```csharp
// PriorityQueue of (value, listIndex, elementIndex)
```


## Pattern-wise problems (solve in order)

| # | Problem | Level |
|---|---------|-------|
| 1 | [Merge Two Sorted Lists](https://leetcode.com/problems/merge-two-sorted-lists/) | Easy |
| 2 | [Merge k Sorted Lists](https://leetcode.com/problems/merge-k-sorted-lists/) | Hard |
| 3 | [Find K Pairs with Smallest Sums](https://leetcode.com/problems/find-k-pairs-with-smallest-sums/) | Medium |
| 4 | [Smallest Range Covering Elements from K Lists](https://leetcode.com/problems/smallest-range-covering-elements-from-k-lists/) | Hard |

## YouTube (watch after attempting)

- [NeetCode](https://www.youtube.com/@NeetCode) — Merge k Sorted Lists
- [Striver](https://www.youtube.com/@takeUforward) — Heap / Linked List

Master index: **Pattern-Wise Problems + Best YouTube Playlists** (Start Here module).

## Further study

- [K-way merge algorithm](https://en.wikipedia.org/wiki/K-way_merge_algorithm)
- [LeetCode](https://leetcode.com/problems/merge-k-sorted-lists/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Merge K sorted lists  
2. Merge K sorted arrays  
3. Smallest range covering elements from K lists
