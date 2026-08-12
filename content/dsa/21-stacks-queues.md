---
id: dsa-stacks-queues
title: Stacks and Queues
track: dsa
module: "03 Linear Structures"
order: 21
languages: [java, csharp]
summary: Monotonic stack, valid parentheses, BFS queue, and deque patterns.
---

## Why this matters

Stacks encode “most recent unresolved”; queues drive BFS. Monotonic stacks unlock next-greater-element class problems.

## Definitions

- **Stack:** A LIFO (last-in, first-out) structure supporting push/pop/peek — used for nested structure and “most recent unresolved” state.
- **Queue:** A FIFO (first-in, first-out) structure supporting enqueue/dequeue — the standard driver for BFS level-order traversal.
- **Deque:** A double-ended queue allowing push/pop from both ends; implements stacks, queues, and sliding-window extremes.
- **Monotonic stack:** A stack kept strictly increasing or decreasing so pops reveal the next greater/smaller element in O(n).
- **Next greater element:** For each index, the nearest later value strictly larger — solved with a monotonic decreasing stack of indices.
- **Valid parentheses:** Matching every opening bracket with the correct closer in nested order, typically with a stack.
- **Enqueue / dequeue:** Queue insert at the back and remove from the front; confusing ends breaks BFS and scheduling logic.
- **Circular buffer queue:** Fixed array with front/rear indices modulo capacity — classic FIFO backing without shifting elements.
- **Priority queue:** Not FIFO — extracts by priority (usually a heap); see the Heaps lesson.

## Circular buffer (FIFO sketch)

```java
class RingQueue {
  final int[] a; int head, tail, size;
  RingQueue(int cap) { a = new int[cap]; }
  boolean offer(int x) {
    if (size == a.length) return false;
    a[tail] = x; tail = (tail + 1) % a.length; size++; return true;
  }
  Integer poll() {
    if (size == 0) return null;
    int x = a[head]; head = (head + 1) % a.length; size--; return x;
  }
}
```

## Worked example 1 — Valid parentheses

```java
boolean isValid(String s) {
  Deque<Character> st = new ArrayDeque<>();
  for (char c : s.toCharArray()) {
    if (c == '(' || c == '[' || c == '{') st.push(c);
    else {
      if (st.isEmpty()) return false;
      char o = st.pop();
      if ((c == ')' && o != '(') || (c == ']' && o != '[') || (c == '}' && o != '{')) return false;
    }
  }
  return st.isEmpty();
}
```

```csharp
bool IsValid(string s) {
  var st = new Stack<char>();
  foreach (char c in s) {
    if (c is '(' or '[' or '{') st.Push(c);
    else {
      if (st.Count == 0) return false;
      char o = st.Pop();
      if ((c == ')' && o != '(') || (c == ']' && o != '[') || (c == '}' && o != '{')) return false;
    }
  }
  return st.Count == 0;
}
```

## Worked example 2 — Next greater element (monotonic stack)

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

## Queue for BFS

Use `ArrayDeque` / `Queue<T>` — never restack with two stacks unless asked to implement queue.

## Interview Q&A

- **Q:** Stack with getMin O(1)?
  **A:** Aux stack of minima, or store pairs (value, minSoFar).
- **Q:** When monotonic?
  **A:** When you need nearest greater/smaller to left/right.

## Pitfalls

- Using stack for BFS (that’s DFS)  
- Forgetting empty checks on pop

## 60-second answer

“Stacks solve nested structure and nearest-greater problems; queues drive level-order/BFS. Monotonic stacks keep indices in decreasing/increasing order for O(n) next-greater.”

## Further study

- [Stack (Wikipedia)](https://en.wikipedia.org/wiki/Stack_(abstract_data_type)) — LIFO semantics and parentheses matching
- [Queue (Wikipedia)](https://en.wikipedia.org/wiki/Queue_(abstract_data_type)) — FIFO semantics for BFS
- [Double-ended queue (Wikipedia)](https://en.wikipedia.org/wiki/Double-ended_queue) — Deque for both ends and monotonic patterns
- [Deque (Java SE)](https://docs.oracle.com/en/java/javase/17/docs/api/java.base/java/util/Deque.html) — Java stack/queue implementations

## Practice prompts

1. Daily temperatures  
2. Largest rectangle in histogram  
3. Implement queue using stacks
