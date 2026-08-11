---
id: dsa-heaps-dp-intro
title: Heaps and DP Intro
track: dsa
module: "05 Heaps"
order: 41
languages: [java, csharp]
summary: Priority queues for Top-K, plus a practical DP playbook (1D/2D) for interviews.
---

## Why this matters

Heaps shine for **Top-K / running median / merge K lists**. DP is how you systematically solve overlapping subproblems — interviewers care about state definition more than clever tricks.

## Definitions

- **Heap (priority queue):** A structure that always yields the current minimum or maximum in O(1) peek with O(log n) insert/delete.
- **Top-K pattern:** Keeping a size-K heap of the opposite polarity (min-heap for K largest) to get O(n log k) instead of full sorting.
- **Dynamic programming (DP):** Solving overlapping subproblems by defining a state, transition, base cases, and iteration order.
- **DP state:** A clear verbal meaning of `dp[i]` or `dp[i][j]` — the interviewer’s main correctness check.
- **Transition:** How a state is computed from smaller already-solved states.
- **Base case:** The simplest states with known answers that seed the recurrence.
- **Space optimization:** Reducing a DP table to rolling variables or one row when only recent states are needed.

## Heaps — concept

Binary heap: get min/max in O(1), insert/delete O(log n).

| Need | Structure |
|------|-----------|
| K largest | **Min-heap** of size K |
| K smallest | **Max-heap** of size K |
| Merge K sorted | Min-heap of current heads |

## Worked example — K largest

```java
public int[] kLargest(int[] a, int k) {
  PriorityQueue<Integer> pq = new PriorityQueue<>(); // min-heap
  for (int x : a) {
    pq.offer(x);
    if (pq.size() > k) pq.poll();
  }
  return pq.stream().mapToInt(i -> i).toArray();
}
```

```csharp
public int[] KLargest(int[] a, int k) {
  var pq = new PriorityQueue<int, int>(); // min by priority
  foreach (int x in a) {
    pq.Enqueue(x, x);
    if (pq.Count > k) pq.Dequeue();
  }
  return pq.UnorderedItems.Select(i => i.Element).ToArray();
}
```

**Complexity:** O(n log k) time, O(k) space — better than full sort when k ≪ n.

## DP — playbook

1. **Define state** — what does `dp[i]` / `dp[i][j]` mean?  
2. **Transition** — how to compute from smaller states  
3. **Base cases**  
4. **Order of iteration**  
5. **Answer location**  
6. Optional: space-optimize

## Worked example — Climbing stairs (1D)

```java
public int climbStairs(int n) {
  if (n <= 2) return n;
  int a = 1, b = 2;
  for (int i = 3; i <= n; i++) {
    int c = a + b; a = b; b = c;
  }
  return b;
}
```

```csharp
public int ClimbStairs(int n) {
  if (n <= 2) return n;
  int a = 1, b = 2;
  for (int i = 3; i <= n; i++) {
    int c = a + b; a = b; b = c;
  }
  return b;
}
```

## Worked example — 0/1 Knapsack sketch (2D → 1D)

```java
// dp[w] = max value with capacity w
for (Item it : items)
  for (int w = W; w >= it.weight; w--)
    dp[w] = Math.max(dp[w], dp[w - it.weight] + it.value);
```

```csharp
for (var it in items)
  for (int w = W; w >= it.Weight; w--)
    dp[w] = Math.Max(dp[w], dp[w - it.Weight] + it.Value);
```

Iterate capacity **downward** so each item is used once.

## Worked example — Longest common subsequence idea

`dp[i][j]` = LCS of `a[0..i)` and `b[0..j)`.  
If equal chars → `1 + dp[i-1][j-1]`, else `max(dp[i-1][j], dp[i][j-1])`.

## Interview Q&A

- **Q:** Heap vs sort for Top-K?
  **A:** Heap O(n log k); sort O(n log n). Prefer heap for streaming/large n.
- **Q:** How do you know it’s DP?
  **A:** Optimal substructure + overlapping subproblems; recursion with memo is the discovery tool.
- **Q:** Top-down vs bottom-up?
  **A:** Memoized recursion is easier to derive; bottom-up often faster/cleaner in interviews once transitions are clear.

## Pitfalls

- Using max-heap when you needed min-heap of size K (and vice versa)  
- Wrong loop direction in knapsack (reusing items accidentally)  
- Off-by-one in DP index meaning (prefix vs inclusive)

## 60-second answer

“For Top-K I keep a size-K heap. For DP I define state, transition, and base cases explicitly — climbing stairs and knapsack are the templates I start from, then I specialize.”

## Further study

- [Heap (Wikipedia)](https://en.wikipedia.org/wiki/Heap_(data_structure)) — binary heap structure and complexities
- [Priority queue (Wikipedia)](https://en.wikipedia.org/wiki/Priority_queue) — abstract interface heaps implement
- [Dynamic programming (Wikipedia)](https://en.wikipedia.org/wiki/Dynamic_programming) — overlapping subproblems and optimal substructure
- [Knapsack problem (Wikipedia)](https://en.wikipedia.org/wiki/Knapsack_problem) — classic DP template for capacity constraints

## Practice prompts

1. Merge K sorted lists  
2. House robber  
3. Coin change (unbounded knapsack)
