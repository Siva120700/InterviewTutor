---
id: dsa-string-matching
title: String Matching Algorithms
track: dsa
module: "08 Advanced Tools"
order: 74
languages: [java, csharp]
summary: Naive search, KMP, Rabin–Karp rolling hash — find patterns in text efficiently.
---

## Why this matters

“Find pattern P in text T” shows up in search, DNA-style problems, and as a building block. KMP and rolling hash beat naive \(O(n m)\) on average/worst cases that matter.

## Definitions

- **Pattern matching:** Locate occurrences of P inside T.
- **Naive search:** Try every alignment — \(O((n-m+1)m)\).
- **LPS / prefix function (KMP):** For pattern P, `lps[i]` = longest proper prefix that is also suffix of `P[0..i]`.
- **KMP:** Uses LPS to skip alignments without rechecking known matches — \(O(n+m)\).
- **Rolling hash (Rabin–Karp):** Fingerprint windows of length m; expected O(n+m) with good hash, verify on hit.
- **Collision:** Different strings, same hash — always verify for correctness unless using double hash carefully.

## Worked example 1 — KMP search

```java
int[] lps(String p) {
  int m = p.length(); int[] lps = new int[m];
  for (int i = 1, len = 0; i < m; ) {
    if (p.charAt(i) == p.charAt(len)) lps[i++] = ++len;
    else if (len > 0) len = lps[len - 1];
    else lps[i++] = 0;
  }
  return lps;
}
List<Integer> kmp(String t, String p) {
  int[] lps = lps(p);
  List<Integer> hits = new ArrayList<>();
  for (int i = 0, j = 0; i < t.length(); ) {
    if (t.charAt(i) == p.charAt(j)) { i++; j++; if (j == p.length()) { hits.add(i - j); j = lps[j - 1]; } }
    else if (j > 0) j = lps[j - 1];
    else i++;
  }
  return hits;
}
```

```csharp
int[] Lps(string p) {
  int m = p.Length; var lps = new int[m];
  for (int i = 1, len = 0; i < m; ) {
    if (p[i] == p[len]) lps[i++] = ++len;
    else if (len > 0) len = lps[len - 1];
    else lps[i++] = 0;
  }
  return lps;
}
```

## Worked example 2 — Rolling hash sketch

```java
// base/mod chosen primes; hash window of length m, slide by
// h = (h * base + add - remove * base^m) % mod
```

Use two mods to reduce collision risk in interviews when stating probabilistic algorithms.

## Interview Q&A

- **Q:** KMP vs `indexOf`?  
  **A:** Library is fine in product code; interviews want LPS intuition.
- **Q:** Z-algorithm?  
  **A:** Related linear matcher — know it exists alongside KMP.
- **Q:** Many patterns?  
  **A:** Aho–Corasick / suffix structures — advanced follow-up.

## Pitfalls

- Building LPS incorrectly (off-by-one on mismatch fallback)  
- Trusting single hash without verification  
- Confusing prefix function with Z-array

## 60-second answer

“Naive is O(nm). KMP precomputes the pattern’s LPS to search in O(n+m). Rabin–Karp slides a rolling hash and verifies hits. I implement KMP LPS carefully on paper first.”

## Further study

- [Knuth–Morris–Pratt algorithm](https://en.wikipedia.org/wiki/Knuth%E2%80%93Morris%E2%80%93Pratt_algorithm)
- [Rabin–Karp algorithm](https://en.wikipedia.org/wiki/Rabin%E2%80%93Karp_algorithm)
- Suffix trees/arrays lesson

## Practice prompts

1. Compute LPS for `aabaaab`  
2. Find all occurrences of pattern in text  
3. Shortest palindrome (KMP trick on `s + # + reverse(s)`)
