---
id: dsa-suffix-trees
title: Suffix Trees and Suffix Arrays
track: dsa
module: "08 Advanced Tools"
order: 73
languages: [java, csharp]
summary: Substring indexing via suffix trees/arrays for fast pattern search and string analytics.
---

## Why this matters

Suffix structures answer “does pattern P appear in text T?” after preprocessing T — plus LCP, repeated substrings, and string matching variants. Full Ukkonen builds are rare in interviews; **concepts + suffix array basics** are what get asked.

## Definitions

- **Suffix:** `T[i..]` — the substring starting at index `i` through the end.
- **Suffix tree:** A compressed trie of all suffixes of T (often with a terminal `$`), edges labeled by substrings.
- **Suffix array:** Sorted list of starting indices of all suffixes — compact alternative to an explicit tree.
- **LCP array:** Longest common prefixes between consecutive suffixes in the suffix array.
- **Pattern search:** Walk the tree (or binary-search the suffix array) for P in \(O(|P|)\) or \(O(|P|\log |T|)\).
- **Trie contrast:** Ordinary tries store a *dictionary of words*; suffix trees index *all suffixes of one text*.

## Concept

```text
T = banana$

Suffixes:          Suffix array order (lex):
0 banana$          6 $
1 anana$           5 a$
2 nana$            3 ana$
3 ana$             1 anana$
4 na$              0 banana$
5 a$               4 na$
6 $                2 nana$
```

A suffix tree compresses shared prefixes of these suffixes into edges.

## Worked example 1 — Naive suffix array

```java
int[] suffixArray(String s) {
  int n = s.length();
  Integer[] sa = new Integer[n];
  for (int i = 0; i < n; i++) sa[i] = i;
  Arrays.sort(sa, (i, j) -> s.substring(i).compareTo(s.substring(j)));
  return Arrays.stream(sa).mapToInt(x -> x).toArray();
}
// O(n² log n) — fine to explain; production uses O(n log n) / O(n)
```

```csharp
int[] SuffixArray(string s) {
  int n = s.Length;
  var sa = Enumerable.Range(0, n).ToArray();
  Array.Sort(sa, (i, j) => string.CompareOrdinal(s[i..], s[j..]));
  return sa;
}
```

## Worked example 2 — Pattern check via suffix array

```java
boolean contains(String text, int[] sa, String pat) {
  int lo = 0, hi = sa.length - 1;
  while (lo <= hi) {
    int mid = (lo + hi) >>> 1;
    String suf = text.substring(sa[mid]);
    int cmp = comparePrefix(suf, pat);
    if (cmp == 0) return true;
    if (cmp < 0) lo = mid + 1; else hi = mid - 1;
  }
  return false;
}
int comparePrefix(String suf, String pat) {
  int n = Math.min(suf.length(), pat.length());
  for (int i = 0; i < n; i++) {
    int d = Character.compare(suf.charAt(i), pat.charAt(i));
    if (d != 0) return d;
  }
  return suf.length() >= pat.length() ? 0 : -1;
}
```

```csharp
bool Contains(string text, int[] sa, string pat) {
  int lo = 0, hi = sa.Length - 1;
  while (lo <= hi) {
    int mid = (lo + hi) / 2;
    int cmp = ComparePrefix(text[sa[mid]..], pat);
    if (cmp == 0) return true;
    if (cmp < 0) lo = mid + 1; else hi = mid - 1;
  }
  return false;
}
```

## Interview depth expectations

| Topic | Expectation |
|-------|-------------|
| What suffixes / SA / LCP are | Must know |
| Binary search SA for pattern | Should code sketch |
| Ukkonen O(n) construction | Name-drop only |
| Applications (LRS, unique substrings) | Discuss with LCP |

## Interview Q&A

- **Q:** Suffix tree vs trie of words?  
  **A:** Suffix tree indexes one text’s suffixes; dictionary trie indexes many independent strings.
- **Q:** Space?  
  **A:** Explicit suffix tree is heavy; suffix arrays are \(O(n)\) integers + text.
- **Q:** Z-algorithm / KMP instead?  
  **A:** Better for single pattern without heavy preprocess of huge T — pick based on query load.

## Pitfalls

- Forgetting terminal `$` uniqueness when discussing tree leaves  
- Claiming you can code Ukkonen under 20 minutes  
- Confusing prefix trees (tries) with suffix trees by name alone

## 60-second answer

“A suffix tree is a compressed trie of all suffixes; a suffix array is the sorted list of suffix starts. After building, I can locate patterns via tree walk or binary search. In interviews I implement suffix arrays naively and explain LCP applications.”

## Further study

- [Suffix tree](https://en.wikipedia.org/wiki/Suffix_tree)
- [Suffix array](https://en.wikipedia.org/wiki/Suffix_array)
- [Longest common prefix array](https://en.wikipedia.org/wiki/LCP_array)

## Practice prompts

1. Build the suffix array of `mississippi` by hand  
2. Find the longest repeated substring using SA + LCP (sketch)  
3. Contrast KMP vs suffix array for 1 pattern vs many patterns
