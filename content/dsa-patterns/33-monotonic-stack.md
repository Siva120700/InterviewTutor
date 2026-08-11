---
id: pattern-monotonic-stack
title: "Pattern: Monotonic Stack"
track: dsa-patterns
module: "05 Search Patterns"
order: 51
languages: [java, csharp]
summary: Next greater/smaller element in O(n) with an increasing/decreasing stack.
---

## Why this matters

Daily temperatures, largest rectangle, and next greater all share this pattern.

## Definitions

- **Monotonic stack:** Stack kept strictly increasing or decreasing by values/indices.
- **Next greater element:** Nearest right index with a larger value.
- **Histogram trick:** For each bar, find previous/next smaller to compute area.

## Recognition cues

- Next greater / previous smaller  
- Daily temperatures  
- Largest rectangle in histogram  
- Trapping rain water (stack variant)  
- Remove k digits (related)

## Template — next greater

```java
int[] nextGreater(int[] a) {
  int n = a.length; int[] ans = new int[n];
  Arrays.fill(ans, -1);
  Deque<Integer> st = new ArrayDeque<>();
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

## Further study

- [Stack (abstract data type)](https://en.wikipedia.org/wiki/Stack_(abstract_data_type))
- [LeetCode Stack tag](https://leetcode.com/tag/stack/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Daily temperatures  
2. Next greater element I/II  
3. Largest rectangle in histogram
