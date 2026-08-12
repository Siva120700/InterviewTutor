---
id: dsa-memory-pointers
title: Memory — Stack vs Heap and Pointers
track: dsa
module: "01 Foundations"
order: 0
languages: [java, csharp]
summary: Stack vs heap allocation, references vs addresses, and how pointer/reference mechanics underpin linked structures.
---

## Why this matters

Every linked list, tree, and graph bug is a misunderstanding of where data lives and what a reference points to. Interviews assume you can reason about stack frames vs heap objects.

## Definitions

- **Stack memory:** Per-thread storage for call frames — locals, parameters, and return addresses; allocated/freed automatically on call/return (LIFO).
- **Heap memory:** Dynamically allocated object storage (`new` / `new`) that lives until GC (or manual free in lower-level languages) reclaims it.
- **Reference:** In Java/C#, a value that *points to* a heap object (or is `null`); assigning a reference copies the pointer, not the object.
- **Address:** The identity of a memory location; you rarely see raw addresses in managed languages, but every reference *is* one under the hood.
- **Dereferencing:** Following a reference to read/write the object’s fields (e.g. `node.next`, `arr[i]`).
- **Null / null reference:** A reference that points to no object; dereferencing it throws (`NullPointerException` / `NullReferenceException`).
- **Aliasing:** Two references pointing to the same heap object — mutating through one is visible through the other.

## Concept

```text
Call stack (frames)          Heap
┌──────────────┐             ┌─────────────────┐
│ main()       │             │ Node { val: 1 } │◄── head
│  head ───────┼────────────►│ next ───────────┼──► Node { val: 2 }
│  x = 42      │             │                 │    next = null
└──────────────┘             └─────────────────┘
```

| | Stack | Heap |
|---|-------|------|
| Lifetime | Tied to method call | Until unreachable / GC |
| Speed | Very fast | Allocation + GC cost |
| Size | Limited (stack overflow risk) | Large |
| Typical contents | Primitives, references | Objects, arrays, nodes |

**Pointer architecture in interviews:** when you write `ListNode n = head.next`, you copy a *reference*. Changing `n.val` mutates the shared node; `n = n.next` only rebinds the local variable.

## Worked example 1 — Alias vs copy

```java
class Box { int v; Box(int v) { this.v = v; } }

void demo() {
  Box a = new Box(1);   // heap object
  Box b = a;            // same reference (alias)
  b.v = 99;
  // a.v is also 99
  int x = 5;            // stack primitive
  int y = x;            // copy of value
  y = 7;                // x still 5
}
```

```csharp
class Box { public int V; public Box(int v) => V = v; }

void Demo() {
  var a = new Box(1);
  var b = a;            // same reference
  b.V = 99;             // a.V is 99
  int x = 5;
  int y = x;            // value copy
  y = 7;                // x still 5
}
```

## Worked example 2 — Why reverse-list needs three locals

```java
// prev, cur, next are stack references into heap nodes
ListNode reverse(ListNode head) {
  ListNode prev = null, cur = head;
  while (cur != null) {
    ListNode next = cur.next; // save before rewiring
    cur.next = prev;          // mutate heap edge
    prev = cur;
    cur = next;
  }
  return prev;
}
```

Losing `next` before rewiring orphans the rest of the list — classic pointer mistake.

## Interview Q&A

- **Q:** Where is an `int` local stored?  
  **A:** On the stack (unless boxed). An `Integer`/`object` local stores a *reference* on the stack pointing to the heap.
- **Q:** Does `arr = otherArr` copy elements?  
  **A:** No — it aliases the same array object.
- **Q:** StackOverflowError cause?  
  **A:** Recursion (or deep call chain) exceeded stack frame limit — not “heap full” (that’s OOM).

## Pitfalls

- Assuming assignment deep-copies objects  
- Losing the only reference to a heap subgraph (memory leak until GC; logic bug forever)  
- Confusing “pass by value of the reference” with “pass by reference” semantics

## 60-second answer

“Primitives and locals live on the stack; objects live on the heap. Variables hold references. Linked structures are graphs of heap nodes connected by reference fields — interviews test whether you rewire those edges without losing nodes.”

## Further study

- [Stack-based memory allocation (Wikipedia)](https://en.wikipedia.org/wiki/Stack-based_memory_allocation)
- [Memory management (Wikipedia)](https://en.wikipedia.org/wiki/Memory_management)
- [Reference (computer science)](https://en.wikipedia.org/wiki/Reference_(computer_science))

## Practice prompts

1. Draw stack + heap for building a 3-node singly linked list  
2. Explain what `a = a.next` does to the heap  
3. Why can recursion blow the stack even when heap space is free?
