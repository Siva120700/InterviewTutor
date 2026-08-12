---
id: dsa-hash-tables-internals
title: Hash Tables — Functions, Chaining, Open Addressing
track: dsa
module: "02 Hashing & Indexing"
order: 8
languages: [java, csharp]
summary: How hash functions map keys to indices, and how separate chaining vs open addressing resolve collisions.
---

## Why this matters

Hash maps/sets feel like magic O(1) until collisions and load factor show up. Interviews probe *why* average case is fast and when it degrades.

## Definitions

- **Hash function:** Maps a key to an integer; table index is usually `hash(key) mod capacity` (after bit mixing).
- **Hash map:** Key→value table with expected O(1) get/put/remove.
- **Hash set:** Membership-only variant (no associated value).
- **Collision:** Two keys mapping to the same bucket index.
- **Separate chaining:** Each bucket holds a linked list (or tree) of entries that hashed there.
- **Open addressing:** All entries live in the table array; on collision, probe another slot (linear, quadratic, double hashing).
- **Load factor \(\alpha\):** \(n / capacity\); triggers resize when too high (Java `HashMap` ~0.75).

## Concept

```text
key ──hash──► index ──► bucket
                         ├─ chaining: linked list / tree of entries
                         └─ open address: probe sequence until empty/match
```

| Strategy | Pros | Cons |
|----------|------|------|
| Separate chaining | Simple deletes; clusters less painful | Extra pointer memory |
| Linear probing | Cache-friendly | Primary clustering |
| Quadratic probing | Less clustering | May miss slots if poorly sized |
| Double hashing | Better probe spread | Two hashes per step |

## Worked example 1 — Chaining sketch

```java
class ChainedMap<K,V> {
  static class Node<K,V> { K k; V v; Node<K,V> next; Node(K k,V v){this.k=k;this.v=v;} }
  Node<K,V>[] tab;
  @SuppressWarnings("unchecked")
  ChainedMap(int cap) { tab = (Node<K,V>[]) new Node[cap]; }

  void put(K key, V val) {
    int i = Math.floorMod(key.hashCode(), tab.length);
    for (Node<K,V> n = tab[i]; n != null; n = n.next)
      if (n.k.equals(key)) { n.v = val; return; }
    tab[i] = new Node<>(key, val) {{ next = tab[i]; }}; // push front — fix carefully in real code
  }
}
```

Prefer a clear loop:

```java
void putClear(K key, V val) {
  int i = Math.floorMod(key.hashCode(), tab.length);
  for (Node<K,V> n = tab[i]; n != null; n = n.next)
    if (Objects.equals(n.k, key)) { n.v = val; return; }
  Node<K,V> fresh = new Node<>(key, val);
  fresh.next = tab[i];
  tab[i] = fresh;
}
```

```csharp
void Put(Dictionary<string,int> map, string key, int val) => map[key] = val;
// BCL Dictionary uses its own hashing + collision strategy — treat as black box + know load factor
```

## Worked example 2 — Linear probing insert

```java
boolean insert(int[] keys, boolean[] used, int key) {
  int n = keys.length;
  int i = Math.floorMod(key, n);
  for (int step = 0; step < n; step++) {
    int j = (i + step) % n;
    if (!used[j]) { keys[j] = key; used[j] = true; return true; }
    if (keys[j] == key) return false; // already present
  }
  return false; // table full
}
```

```csharp
bool Insert(int[] keys, bool[] used, int key) {
  int n = keys.Length;
  int i = Math.Abs(key % n);
  for (int step = 0; step < n; step++) {
    int j = (i + step) % n;
    if (!used[j]) { keys[j] = key; used[j] = true; return true; }
    if (keys[j] == key) return false;
  }
  return false;
}
```

## Complexity

| Operation | Average | Worst (bad hash / full table) |
|-----------|---------|--------------------------------|
| get/put | \(O(1)\) | \(O(n)\) |
| resize | amortized into puts | — |

## Interview Q&A

- **Q:** Why resize at load factor 0.75?  
  **A:** Keeps probe/chain lengths short; doubles capacity and rehashes.
- **Q:** HashSet vs TreeSet?  
  **A:** HashSet expected O(1) unordered; TreeSet O(log n) ordered.
- **Q:** Mutable keys?  
  **A:** Never mutate fields that affect `hashCode`/`Equals` while in the table.

## Pitfalls

- Using poor hash (identity only) → huge chains  
- Forgetting wrap-around in open addressing  
- Assuming worst-case O(1) in adversarial settings

## 60-second answer

“A hash function turns a key into a bucket index. Collisions use chaining (lists per bucket) or open addressing (probe). With a good hash and bounded load factor, get/put are expected O(1); interviews still want you to mention worst-case O(n).”

## Further study

- [Hash table (Wikipedia)](https://en.wikipedia.org/wiki/Hash_table)
- [Open addressing](https://en.wikipedia.org/wiki/Open_addressing)
- Java `HashMap` treeification notes (bins → red-black at high collision)

## Practice prompts

1. Implement a tiny string→int map with chaining  
2. Count operations until first collision for a given hash  
3. Explain why `HashMap` iteration order is unreliable
