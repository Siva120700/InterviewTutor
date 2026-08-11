---
id: dsa-tries
title: Tries (Prefix Trees)
track: dsa
module: "04 Trees"
order: 32
languages: [java, csharp]
summary: Insert/search/startsWith, autocomplete, and word-break adjacent patterns.
---

## Why this matters

Tries excel at prefix queries and dictionaries — autocomplete, spellcheck, word search II.

## Definitions

- **Trie (prefix tree):** A tree where each path from the root spells a string prefix, and shared prefixes share nodes.
- **Trie node:** A node storing children (array[26] or map) and typically an `isWord`/`end` flag marking a complete word.
- **Insert:** Walking/creating child nodes for each character and marking the final node as a word end.
- **Search:** Following the character path and returning true only if the path exists and ends on a marked word.
- **StartsWith (prefix query):** Following the character path and returning true if the path exists, even if not a complete word.
- **Prefix sharing:** Multiple words reuse nodes for a common prefix, making tries efficient for dictionaries and autocomplete.
- **Alphabet branching:** Children keyed by character; array[26] is O(1) per step for lowercase, maps handle larger alphabets.

## Structure

Each node: children map/array[26] + `isWord` flag.

```java
class TrieNode {
  TrieNode[] next = new TrieNode[26];
  boolean end;
}
class Trie {
  TrieNode root = new TrieNode();
  void insert(String w) {
    TrieNode n = root;
    for (char c : w.toCharArray()) {
      int i = c - 'a';
      if (n.next[i] == null) n.next[i] = new TrieNode();
      n = n.next[i];
    }
    n.end = true;
  }
  boolean search(String w) {
    TrieNode n = find(w);
    return n != null && n.end;
  }
  boolean startsWith(String p) { return find(p) != null; }
  TrieNode find(String w) {
    TrieNode n = root;
    for (char c : w.toCharArray()) {
      int i = c - 'a';
      if (n.next[i] == null) return null;
      n = n.next[i];
    }
    return n;
  }
}
```

```csharp
class TrieNode {
  public TrieNode?[] Next = new TrieNode[26];
  public bool End;
}
class Trie {
  readonly TrieNode root = new();
  public void Insert(string w) {
    var n = root;
    foreach (char c in w) {
      int i = c - 'a';
      n.Next[i] ??= new TrieNode();
      n = n.Next[i]!;
    }
    n.End = true;
  }
  public bool StartsWith(string p) => Find(p) is not null;
  TrieNode? Find(string w) {
    var n = root;
    foreach (char c in w) {
      int i = c - 'a';
      if (n.Next[i] is null) return null;
      n = n.Next[i]!;
    }
    return n;
  }
}
```

## Complexity

Insert/search O(L) time, space O(total characters) with sharing.

## Interview Q&A

- **Q:** HashSet vs Trie?
  **A:** HashSet exact lookup; Trie prefix/share structure.
- **Q:** Map vs array children?
  **A:** Array for lowercase a–z; map for sparse/unicode.

## Pitfalls

- Forgetting `end` vs prefix-only node  
- Mutating shared nodes incorrectly

## 60-second answer

“A trie stores characters on edges for O(L) prefix ops. I use it for dictionaries and multi-pattern search over boards.”

## Further study

- [Trie (Wikipedia)](https://en.wikipedia.org/wiki/Trie) — prefix trees for dictionaries and autocomplete
- [Radix tree (Wikipedia)](https://en.wikipedia.org/wiki/Radix_tree) — compressed-prefix relatives of tries
- [Prefix (computer science) (Wikipedia)](https://en.wikipedia.org/wiki/Prefix_(computer_science)) — shared prefixes that tries exploit
- [String-searching algorithm (Wikipedia)](https://en.wikipedia.org/wiki/String-searching_algorithm) — broader context for prefix queries

## Practice prompts

1. Word search II  
2. Replace words with root stems  
3. Design autocomplete system
