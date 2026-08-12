---
id: dsa-linked-lists
title: Linked Lists
track: dsa
module: "03 Linear Structures"
order: 20
languages: [java, csharp]
summary: Singly/doubly lists, reverse, merge, cycle detection, and dummy-node technique.
---

## Why this matters

Lists teach pointer manipulation — easy to bug, common in interviews.

## Definitions

- **Linked list:** A sequence of nodes where each node stores a value and a pointer/reference to the next (and optionally previous) node.
- **Singly linked list:** Each node has only a `next` pointer; traversal is one-directional.
- **Doubly linked list:** Each node has `next` and `prev`, enabling O(1) removal given the node.
- **Dummy (sentinel) node:** A fake head before the real list that simplifies empty-list and head-update edge cases.
- **In-place reverse:** Reversing link directions by walking prev/cur/next pointers without allocating a new list.
- **Cycle:** A loop where following `next` never reaches null; detected with Floyd’s slow/fast pointer meeting.
- **Floyd’s algorithm:** Slow (1 step) and fast (2 steps) meet iff a cycle exists; resetting one to head finds the entrance.
- **Circular linked list:** Last node’s `next` points back to the first (ring); there is no null terminator — traversal needs a start marker or visit count.

## Concept

Node → next (→ prev). O(1) insert at head; O(n) access by index. Prefer arrays when random access matters.

| Variant | Links | End condition | Typical use |
|---------|-------|---------------|-------------|
| Singly | `next` | `null` | Stacks, simple lists |
| Doubly | `next` + `prev` | `null` | LRU, O(1) removal |
| Circular | `next` (ring) | back to head | Round-robin, buffers |

## Worked example 1 — Reverse list

```java
ListNode reverse(ListNode head) {
  ListNode prev = null, cur = head;
  while (cur != null) {
    ListNode next = cur.next;
    cur.next = prev;
    prev = cur;
    cur = next;
  }
  return prev;
}
```

```csharp
ListNode Reverse(ListNode head) {
  ListNode? prev = null, cur = head;
  while (cur is not null) {
    var next = cur.next;
    cur.next = prev;
    prev = cur;
    cur = next;
  }
  return prev!;
}
```

## Worked example 2 — Merge two sorted lists

```java
ListNode merge(ListNode a, ListNode b) {
  ListNode dummy = new ListNode(0), t = dummy;
  while (a != null && b != null) {
    if (a.val <= b.val) { t.next = a; a = a.next; }
    else { t.next = b; b = b.next; }
    t = t.next;
  }
  t.next = a != null ? a : b;
  return dummy.next;
}
```

```csharp
ListNode Merge(ListNode a, ListNode b) {
  var dummy = new ListNode(0); var t = dummy;
  while (a is not null && b is not null) {
    if (a.val <= b.val) { t.next = a; a = a.next!; }
    else { t.next = b; b = b.next!; }
    t = t.next;
  }
  t.next = a ?? b;
  return dummy.next!;
}
```

## Worked example 3 — Cycle (Floyd)

Covered lightly in two-pointers; interview staple: slow/fast meet ⇒ cycle; reset one to head to find entrance.

## Circular list — insert after head

```java
ListNode insertCircular(ListNode head, int val) {
  ListNode node = new ListNode(val);
  if (head == null) {
    node.next = node;
    return node;
  }
  node.next = head.next;
  head.next = node;
  return head;
}
```

```csharp
ListNode InsertCircular(ListNode? head, int val) {
  var node = new ListNode(val);
  if (head is null) {
    node.next = node;
    return node;
  }
  node.next = head.next;
  head.next = node;
  return head;
}
```

Traverse with `do { ...; cur = cur.next; } while (cur != head);` — never wait for `null`.

## Dummy node

Always consider a dummy head to simplify edge cases (delete head, merge).

## Interview Q&A

- **Q:** Array vs list for queue?
  **A:** Deque/circular buffer often better than singly list for cache locality.
- **Q:** Recursion reverse?
  **A:** Elegant but O(n) stack; iterative preferred in production interviews.

## Pitfalls

- Losing `next` before rewiring  
- Off-by-one on k-th node problems

## 60-second answer

“I draw pointer updates carefully, use a dummy for edge cases, and reverse/merge with iterative rewires. Fast/slow finds cycles in O(1) space.”

## Further study

- [Linked list (Wikipedia)](https://en.wikipedia.org/wiki/Linked_list) — singly/doubly linked structures
- [Doubly linked list (Wikipedia)](https://en.wikipedia.org/wiki/Doubly_linked_list) — O(1) removal given the node
- [Cycle detection (Wikipedia)](https://en.wikipedia.org/wiki/Cycle_detection) — Floyd’s algorithm for list cycles
- [Pointer (Wikipedia)](https://en.wikipedia.org/wiki/Pointer_(computer_programming)) — prev/cur/next rewiring mental model

## Practice prompts

1. Remove Nth from end  
2. Reorder list  
3. LRU Cache (list + hashmap) — see LLD
