---
id: dsa-linked-list-design
title: Linked List Design Patterns
track: dsa
module: "03 Linear Structures"
order: 23
languages: [java, csharp]
summary: Dummy nodes, two-pointer designs, and list+hash map patterns (LRU-style) for interviews.
---

## Why this matters

Part-2 linked list interviews are less “reverse a list” and more “design a structure” — O(1) updates with pointers + maps.

## Definitions

- **Dummy / sentinel head:** Fake node before the real head so insert/delete don’t special-case empty/head.
- **Tail pointer:** Extra reference for O(1) append (deque, queue).
- **Hash map + doubly list:** Key → node for O(1) lookup; list orders recency (LRU).
- **Move-to-front / splice:** Unlink a node and reattach at head/tail without scanning.
- **Invariant:** After every operation, `map` and list links agree — no dangling neighbors.

## Worked example — LRU cache skeleton

```java
class LRUCache {
  static class Node {
    int k, v; Node prev, next;
    Node(int k, int v) { this.k = k; this.v = v; }
  }
  final int cap;
  final Map<Integer, Node> map = new HashMap<>();
  final Node head = new Node(0, 0), tail = new Node(0, 0);
  LRUCache(int capacity) {
    cap = capacity; head.next = tail; tail.prev = head;
  }
  void remove(Node n) { n.prev.next = n.next; n.next.prev = n.prev; }
  void addFront(Node n) {
    n.next = head.next; n.prev = head;
    head.next.prev = n; head.next = n;
  }
  public int get(int key) {
    Node n = map.get(key); if (n == null) return -1;
    remove(n); addFront(n); return n.v;
  }
  public void put(int key, int value) {
    Node n = map.get(key);
    if (n != null) { n.v = value; remove(n); addFront(n); return; }
    if (map.size() == cap) {
      Node lru = tail.prev; remove(lru); map.remove(lru.k);
    }
    Node fresh = new Node(key, value);
    map.put(key, fresh); addFront(fresh);
  }
}
```

```csharp
// Same design: Dictionary<int, Node> + doubly linked list with sentinels
```

## Other design prompts

| Prompt | Structure |
|--------|-----------|
| Browser history | Two stacks or doubly list + current |
| Max stack / min stack | Parallel stack of extrema |
| Queue with stacks | In/out stacks |
| O(1) insert-delete-getRandom | Map + array swap-remove |

## Interview Q&A

- **Q:** Why doubly for LRU?  
  **A:** Need O(1) unlink from middle; singly forces scan for prev.
- **Q:** Thread safety?  
  **A:** Not unless asked — mention concurrent maps/locks only if senior follow-up.
- **Q:** Capacity 0?  
  **A:** Clarify; usually capacity ≥ 1 in prompts.

## Pitfalls

- Updating map but forgetting list links (or vice versa)  
- Removing LRU from list but not from map  
- Null `prev`/`next` when skipping sentinels

## 60-second answer

“List design problems combine pointer rewiring with a hash map for O(1) access. LRU is the poster child: doubly linked list for order, map for key→node, sentinels to simplify edge cases.”

## Further study

- Linked Lists basics · LLD LRU lesson  

## Practice prompts

1. LRU Cache  
2. LFU Cache (advanced)  
3. Design Circular Deque
