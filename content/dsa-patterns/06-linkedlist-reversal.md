---
id: pattern-ll-reversal
title: "Pattern: In-place Linked List Reversal"
track: dsa-patterns
module: "01 Linear Patterns"
order: 15
languages: [java, csharp]
summary: Reverse whole list, sublist, or k-group — pointer rewiring template.
---

## Why this matters

Reversal is the gateway to palindrome lists, rotate list, and reverse k-group.

## Definitions

- **In-place reversal:** Rewire `next` pointers without extra list allocation.
- **Dummy node:** Sentinel before head to simplify edge cases.
- **Sublist reverse:** Reverse between positions `m` and `n` or every k nodes.

## Recognition cues

- Reverse linked list / reverse between  
- Reverse nodes in k-group  
- Palindrome linked list  
- Reorder list

## Template

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

## Further study

- [Linked list](https://en.wikipedia.org/wiki/Linked_list)
- [LeetCode Linked List tag](https://leetcode.com/tag/linked-list/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Reverse linked list  
2. Reverse linked list II  
3. Reverse nodes in k-group
