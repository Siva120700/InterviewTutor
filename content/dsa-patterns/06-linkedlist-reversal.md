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


## Pattern-wise problems (solve in order)

| # | Problem | Level |
|---|---------|-------|
| 1 | [Reverse Linked List](https://leetcode.com/problems/reverse-linked-list/) | Easy |
| 2 | [Reverse Linked List II](https://leetcode.com/problems/reverse-linked-list-ii/) | Medium |
| 3 | [Swap Nodes in Pairs](https://leetcode.com/problems/swap-nodes-in-pairs/) | Medium |
| 4 | [Reorder List](https://leetcode.com/problems/reorder-list/) | Medium |
| 5 | [Reverse Nodes in k-Group](https://leetcode.com/problems/reverse-nodes-in-k-group/) | Hard |

## YouTube (watch after attempting)

- [NeetCode](https://www.youtube.com/@NeetCode) — Reverse Linked List / k-Group
- [Striver Linked List playlist](https://www.youtube.com/@takeUforward/playlists)
- [Blind 75 playlist](https://www.youtube.com/playlist?list=PLot-Xpze53ldVwtstag2TL4HQhAnC8ATf)

Master index: **Pattern-Wise Problems + Best YouTube Playlists** (Start Here module).

## Further study

- [Linked list](https://en.wikipedia.org/wiki/Linked_list)
- [LeetCode Linked List tag](https://leetcode.com/tag/linked-list/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Reverse linked list  
2. Reverse linked list II  
3. Reverse nodes in k-group
