---
id: dsa-xor-trie
title: XOR Trie (Bit Trie)
track: dsa
module: "04 Trees"
order: 34
languages: [java, csharp]
summary: Binary trie on bits for maximum XOR pair and related queries.
---

## Why this matters

“Maximum XOR of two numbers in an array” and offline XOR queries are awkward with sorting alone. A bit trie gives greedy O(32·n) solutions.

## Definitions

- **Bit trie:** A binary trie where each edge is bit `0` or `1`, paths from root to leaf encode numbers (MSB→LSB).
- **XOR greed:** To maximize `x ^ y`, at each bit prefer the opposite bit of `x` if that child exists.
- **Insert:** Walk/create 32 (or 31) bit nodes for a number.
- **Query max XOR:** From MSB, branch toward `1 - bit` when possible.
- **Offline delete / limit:** Variants keep counts on nodes or insert only values ≤ constraint.

## Worked example — max XOR pair

```java
class BitTrie {
  static class Node { Node[] c = new Node[2]; }
  Node root = new Node();
  void insert(int x) {
    Node cur = root;
    for (int b = 31; b >= 0; b--) {
      int bit = (x >> b) & 1;
      if (cur.c[bit] == null) cur.c[bit] = new Node();
      cur = cur.c[bit];
    }
  }
  int maxXor(int x) {
    Node cur = root; int ans = 0;
    for (int b = 31; b >= 0; b--) {
      int bit = (x >> b) & 1;
      int want = 1 - bit;
      if (cur.c[want] != null) { ans |= 1 << b; cur = cur.c[want]; }
      else cur = cur.c[bit];
    }
    return ans;
  }
}
int findMaximumXOR(int[] a) {
  BitTrie t = new BitTrie(); int best = 0;
  for (int x : a) { t.insert(x); best = Math.max(best, t.maxXor(x)); }
  return best;
}
```

```csharp
class BitTrie {
  class Node { public Node?[] C = new Node?[2]; }
  readonly Node root = new();
  public void Insert(int x) {
    var cur = root;
    for (int b = 31; b >= 0; b--) {
      int bit = (x >> b) & 1;
      cur.C[bit] ??= new Node();
      cur = cur.C[bit]!;
    }
  }
  public int MaxXor(int x) {
    var cur = root; int ans = 0;
    for (int b = 31; b >= 0; b--) {
      int bit = (x >> b) & 1, want = 1 - bit;
      if (cur.C[want] != null) { ans |= 1 << b; cur = cur.C[want]!; }
      else cur = cur.C[bit]!;
    }
    return ans;
  }
}
```

## Complexity

Insert/query: O(W) with W=32 (or 63 for long). Space O(n·W) worst case.

## Interview Q&A

- **Q:** Why MSB first?  
  **A:** Higher bits dominate the numeric XOR value — greedy works only from the top.
- **Q:** Signed ints?  
  **A:** Treat the bit pattern; for non-negative constraints, 31→0 is enough.
- **Q:** vs hashing pairs?  
  **A:** Hash tricks exist for max XOR; trie is the clearest interview structure.

## Pitfalls

- Starting from LSB  
- Null child without fallback to same bit  
- Inserting after query when the problem needs “pair of distinct prior elements” — check order

## 60-second answer

“I store numbers in a binary bit trie MSB-first. To maximize XOR with x I greedily take the opposite bit at each level. That’s O(32n) for the classic max-XOR-pair problem.”

## Further study

- Tries lesson  
- Bit manipulation lesson  

## Practice prompts

1. Maximum XOR of Two Numbers in an Array  
2. Maximum XOR With an Element From Array  
3. Count pairs with XOR in a range (bit-trie counts)
