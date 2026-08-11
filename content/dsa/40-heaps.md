---
id: dsa-heaps
title: Heaps and Priority Queues
track: dsa
module: "05 Heaps"
order: 40
languages: [java, csharp]
summary: Min/max heaps, Top-K, merge K lists, and median maintenance.
---

## Why this matters

Heaps give efficient Top-K and “always grab the smallest current” scheduling.

## Definitions

- **Heap (priority queue):** A binary-tree-backed structure that exposes the current min or max in O(1) peek, with O(log n) insert/delete.
- **Min-heap / max-heap:** A min-heap keeps the smallest at the root; a max-heap keeps the largest.
- **Top-K:** Finding the K largest (or smallest) elements, often with a size-K opposite-direction heap in O(n log k).
- **Heapify:** Building a heap from an unsorted array in O(n) time rather than n successive inserts.
- **Merge K lists:** Repeatedly extracting the smallest current head from K sorted lists using a min-heap of size K — O(N log K).
- **Running median:** Maintaining the stream median with a max-heap (lower half) and min-heap (upper half), rebalancing sizes.
- **Comparator / ordering:** The rule that defines “priority”; wrong polarity is the most common heap interview bug.

## Concept

Binary heap: parent ≤ children (min). Insert/delete O(log n), peek O(1).

## Worked example 1 — Kth largest in stream

```java
class KthLargest {
  private final PriorityQueue<Integer> pq; // min-heap size k
  private final int k;
  KthLargest(int k, int[] nums) {
    this.k = k; pq = new PriorityQueue<>();
    for (int x : nums) add(x);
  }
  int add(int val) {
    pq.offer(val);
    if (pq.size() > k) pq.poll();
    return pq.peek();
  }
}
```

```csharp
class KthLargest {
  private readonly PriorityQueue<int, int> pq = new();
  private readonly int k;
  public KthLargest(int k, int[] nums) {
    this.k = k;
    foreach (var x in nums) Add(x);
  }
  public int Add(int val) {
    pq.Enqueue(val, val);
    if (pq.Count > k) pq.Dequeue();
    return pq.Peek();
  }
}
```

## Worked example 2 — Merge K sorted lists idea

Min-heap of current heads; pop smallest, push its next — O(N log K).

## Median of stream

Max-heap (lower half) + min-heap (upper half); rebalance sizes.

## Interview Q&A

- **Q:** Heap vs sorted set?
  **A:** Heap can’t arbitrary delete middle efficiently; TreeMap can.
- **Q:** Build heap?
  **A:** Heapify O(n).

## Pitfalls

- Wrong heap direction for Top-K  
- Comparing objects without proper comparator

## 60-second answer

“I use a size-K min-heap for K largest, and dual heaps for running median. Complexity O(n log k) beats full sorts when k is small.”

## Further study

- [Heap (Wikipedia)](https://en.wikipedia.org/wiki/Heap_(data_structure)) — binary heap shape and heap-order property
- [Binary heap (Wikipedia)](https://en.wikipedia.org/wiki/Binary_heap) — array representation and O(log n) ops
- [Priority queue (Wikipedia)](https://en.wikipedia.org/wiki/Priority_queue) — interview vocabulary for heaps
- [PriorityQueue (Java SE)](https://docs.oracle.com/en/java/javase/17/docs/api/java.base/java/util/PriorityQueue.html) — Java min-heap defaults and comparators

## Practice prompts

1. Top K frequent words  
2. Meeting rooms II (min-heap of end times)  
3. Find median from data stream
