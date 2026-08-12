---
id: pattern-fast-slow
title: "Pattern: Fast and Slow Pointers"
track: dsa-patterns
module: "01 Linear Patterns"
order: 12
languages: [java, csharp]
summary: Floyd cycle detection, middle of list, and happy-number style tricks.
---

## Why this matters

Linked-list cycle and “find middle” appear constantly. O(1) space vs hashing nodes.

## Definitions

- **Fast & slow pointers:** Two iterators at different speeds on the same structure.
- **Floyd’s cycle detection:** Slow ×1, fast ×2; meeting implies a cycle.
- **Cycle entrance:** After meet, reset one pointer to head; advance both ×1 to find start.
- **Middle of list:** When fast hits end, slow is at middle.

## Recognition cues

- Detect cycle / happy number  
- Find middle of singly linked list  
- Palindrome linked list (find mid + reverse)  
- Remove nth from end (gap of n between pointers)

## Template — cycle

```java
boolean hasCycle(ListNode head) {
  ListNode slow = head, fast = head;
  while (fast != null && fast.next != null) {
    slow = slow.next;
    fast = fast.next.next;
    if (slow == fast) return true;
  }
  return false;
}
```

```csharp
bool HasCycle(ListNode head) {
  var slow = head; var fast = head;
  while (fast is not null && fast.next is not null) {
    slow = slow.next!;
    fast = fast.next.next!;
    if (ReferenceEquals(slow, fast)) return true;
  }
  return false;
}
```


## Pattern-wise problems (solve in order)

| # | Problem | Level |
|---|---------|-------|
| 1 | [Linked List Cycle](https://leetcode.com/problems/linked-list-cycle/) | Easy |
| 2 | [Middle of the Linked List](https://leetcode.com/problems/middle-of-the-linked-list/) | Easy |
| 3 | [Happy Number](https://leetcode.com/problems/happy-number/) | Easy |
| 4 | [Find the Duplicate Number](https://leetcode.com/problems/find-the-duplicate-number/) | Medium |
| 5 | [Palindrome Linked List](https://leetcode.com/problems/palindrome-linked-list/) | Easy |

## YouTube (watch after attempting)

- [NeetCode](https://www.youtube.com/@NeetCode) — Linked List Cycle / Duplicate Number
- [Blind 75 playlist](https://www.youtube.com/playlist?list=PLot-Xpze53ldVwtstag2TL4HQhAnC8ATf)
- [Striver Linked List](https://www.youtube.com/@takeUforward/playlists)

Master index: **Pattern-Wise Problems + Best YouTube Playlists** (Start Here module).

## Further study

- [Floyd’s tortoise and hare](https://en.wikipedia.org/wiki/Cycle_detection#Floyd's_tortoise_and_hare)
- [LeetCode Linked List tag](https://leetcode.com/tag/linked-list/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Linked list cycle II (entrance)  
2. Middle of the linked list  
3. Happy number
