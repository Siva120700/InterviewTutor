---
id: dsa-greedy
title: Greedy Algorithms
track: dsa
module: "08 Advanced Tools"
order: 69
languages: [java, csharp]
summary: Exchange argument mindset, interval scheduling, and classic greedy interview patterns.
---

## Why this matters

Greedy wins when a local choice extends to a global optimum. Interviews test whether you can *justify* the choice — not just sort and hope.

## Definitions

- **Greedy choice:** Pick the locally best option at each step.
- **Optimal substructure:** Optimal solution contains optimal solutions to subproblems.
- **Exchange argument:** Show any optimal solution can be transformed into the greedy one without worsening cost.
- **Stay-ahead:** Prove greedy is always at least as far ahead as any other algorithm.
- **Counterexample first:** If unsure, try to break the greedy idea before coding.

## Classic patterns

| Pattern | Greedy idea |
|---------|-------------|
| Interval scheduling | Earliest finish time |
| Interval merging | Sort by start, extend end |
| Jump game | Track farthest reachable |
| Huffman / file merge | Always merge two smallest |
| Activity selection | Same as scheduling |
| Fractional knapsack | Value/weight density |

## Worked example 1 — Non-overlapping intervals removed

```java
int eraseOverlap(int[][] intervals) {
  Arrays.sort(intervals, Comparator.comparingInt(a -> a[1]));
  int kept = 0, end = Integer.MIN_VALUE;
  for (int[] iv : intervals) {
    if (iv[0] >= end) { kept++; end = iv[1]; }
  }
  return intervals.length - kept;
}
```

```csharp
int EraseOverlap(int[][] intervals) {
  Array.Sort(intervals, (a, b) => a[1].CompareTo(b[1]));
  int kept = 0, end = int.MinValue;
  foreach (var iv in intervals) {
    if (iv[0] >= end) { kept++; end = iv[1]; }
  }
  return intervals.Length - kept;
}
```

## Worked example 2 — Jump game

```java
boolean canJump(int[] a) {
  int far = 0;
  for (int i = 0; i < a.length; i++) {
    if (i > far) return false;
    far = Math.max(far, i + a[i]);
  }
  return true;
}
```

## Interview Q&A

- **Q:** Greedy vs DP?  
  **A:** If local choice needs future trade-offs (0/1 knapsack), use DP; if exchange proof works, greedy.
- **Q:** How to prove?  
  **A:** Sketch exchange or stay-ahead in 30 seconds — interviewers care you know proof exists.

## Pitfalls

- Sorting by the wrong key  
- Assuming greedy works because samples pass  
- Mutating while iterating unsorted input

## 60-second answer

“I state the greedy choice and why no better solution is lost (exchange). Then I sort by the critical key and do a linear scan. If I can’t justify it, I switch to DP.”

## Further study

- Union-Find lesson (Kruskal is greedy + DSU)  
- [Greedy algorithm](https://en.wikipedia.org/wiki/Greedy_algorithm)

## Practice prompts

1. Minimum number of arrows to burst balloons  
2. Gas station  
3. Task scheduler
