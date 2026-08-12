---
id: dsa-monotonic-stack
title: Monotonic Stack
track: dsa
module: "03 Linear Structures"
order: 22
languages: [java, csharp]
summary: Next greater/smaller element, histogram rectangle, and the monotonic stack template.
---

## Why this matters

Many “nearest greater/smaller to left/right” problems collapse to one stack pass — O(n) instead of O(n²) nested scans.

## Definitions

- **Monotonic stack:** A stack kept strictly increasing or decreasing by value (or by `a[index]`).
- **Next greater element (NGE):** For each index, nearest later index with a strictly larger value (−1 if none).
- **Previous smaller:** Nearest earlier index with a smaller value — same idea, scan direction/flip inequality.
- **Histogram largest rectangle:** For each bar, max width = distance between previous smaller and next smaller.
- **Pop invariant:** While the top violates monotonicity, pop — the new element is the “next …” for popped indices.

## Template — next greater to the right

```java
int[] nextGreater(int[] a) {
  int n = a.length; int[] ans = new int[n];
  Arrays.fill(ans, -1);
  Deque<Integer> st = new ArrayDeque<>(); // indices, decreasing values
  for (int i = 0; i < n; i++) {
    while (!st.isEmpty() && a[st.peek()] < a[i]) ans[st.pop()] = a[i];
    st.push(i);
  }
  return ans;
}
```

```csharp
int[] NextGreater(int[] a) {
  int n = a.Length; var ans = Enumerable.Repeat(-1, n).ToArray();
  var st = new Stack<int>();
  for (int i = 0; i < n; i++) {
    while (st.Count > 0 && a[st.Peek()] < a[i]) ans[st.Pop()] = a[i];
    st.Push(i);
  }
  return ans;
}
```

## Worked example — largest rectangle in histogram

```java
int largestRectangle(int[] h) {
  int n = h.length; int best = 0;
  Deque<Integer> st = new ArrayDeque<>();
  for (int i = 0; i <= n; i++) {
    int cur = i == n ? 0 : h[i];
    while (!st.isEmpty() && h[st.peek()] > cur) {
      int height = h[st.pop()];
      int left = st.isEmpty() ? -1 : st.peek();
      best = Math.max(best, height * (i - left - 1));
    }
    st.push(i);
  }
  return best;
}
```

Sentinel `0` at the end flushes the stack.

## When to reach for it

| Cue | Pattern |
|-----|---------|
| “Next greater/smaller” | Monotonic stack |
| Daily temperatures | NGE distances |
| Trapping rain water (stack form) | Bounds via previous/next greater |
| Sum of subarray minimums | Previous/next smaller counts |

## Interview Q&A

- **Q:** Stack of values or indices?  
  **A:** Prefer **indices** — you often need distance/width.
- **Q:** Strict vs non-strict?  
  **A:** Match the problem (`<` vs `≤`); duplicates change equal-element handling.
- **Q:** Circular array NGE?  
  **A:** Loop `2n` with `i % n`, stop pushing after first pass.

## Pitfalls

- Storing values then needing widths  
- Off-by-one when computing span `i - left - 1`  
- Forgetting to clear remaining stack with a sentinel

## 60-second answer

“I keep a mono stack of indices. When a new value breaks the order, every popped index just found its next greater/smaller. Histogram and NGE are the same template.”

## Further study

- Stacks & Queues lesson  
- DSA Patterns: Monotonic Stack  

## Practice prompts

1. Daily Temperatures  
2. Next Greater Element I/II  
3. Largest Rectangle in Histogram
