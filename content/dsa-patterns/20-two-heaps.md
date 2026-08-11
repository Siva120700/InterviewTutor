---
id: pattern-two-heaps
title: "Pattern: Two Heaps"
track: dsa-patterns
module: "03 Heap Patterns"
order: 30
languages: [java, csharp]
summary: Max-heap + min-heap for median and scheduling balance problems.
---

## Why this matters

Running median and “balance two groups” problems use two heaps as the pattern.

## Definitions

- **Two heaps pattern:** Max-heap for lower half, min-heap for upper half.
- **Rebalance:** Keep sizes differ by at most 1 so tops form the median boundary.
- **Priority queue:** Heap abstraction for extract-min/max.

## Recognition cues

- Find median from data stream  
- Sliding window median  
- Schedule tasks / CPU (sometimes)  
- Maximize capital (IPO) style mixes

## Template — median

```java
PriorityQueue<Integer> low = new PriorityQueue<>(Collections.reverseOrder()); // max
PriorityQueue<Integer> high = new PriorityQueue<>(); // min
void add(int x) {
  low.offer(x);
  high.offer(low.poll());
  if (high.size() > low.size()) low.offer(high.poll());
}
double median() {
  if (low.size() > high.size()) return low.peek();
  return (low.peek() + high.peek()) / 2.0;
}
```

```csharp
// use PriorityQueue with inverted priorities for max-heap lower half
```

## Further study

- [Binary heap](https://en.wikipedia.org/wiki/Binary_heap)
- [LeetCode Heap tag](https://leetcode.com/tag/heap-priority-queue/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Find median from data stream  
2. Sliding window median  
3. IPO (maximize capital)
