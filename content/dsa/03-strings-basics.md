---
id: dsa-strings-basics
title: Strings Essentials
track: dsa
module: "01 Foundations"
order: 3
languages: [java, csharp]
summary: Immutability, builders, palindromes, anagram checks, and frequency arrays.
---

## Why this matters

String problems test indexing discipline and language details (immutability, Unicode awareness at interview depth).

## Definitions

- **String:** An immutable sequence of characters in Java and C#; each concatenation creates a new object.
- **Immutability:** Contents cannot change after creation, so repeated `+` in a loop is O(n²) from copying.
- **StringBuilder:** A mutable character buffer used to build strings in amortized O(n) instead of quadratic concatenation.
- **Palindrome:** A string that reads the same forward and backward (often checked after ignoring non-alphanumeric chars and case).
- **Anagram:** Two strings with the same characters at the same frequencies, typically verified via counting or sorting.
- **Frequency array:** A fixed-size count table (e.g., length 26 for lowercase letters) used for O(n) character tallies.
- **Two-pointer string scan:** Moving indices from both ends (or one pass with skips) for palindrome and partition-style checks.

## Concept

In Java/`string` in C#, strings are immutable — repeated `+` in a loop is O(n²). Use `StringBuilder` / `StringBuilder`.

## Worked example 1 — Valid palindrome (alphanumeric)

```java
boolean isPalindrome(String s) {
  int l = 0, r = s.length() - 1;
  while (l < r) {
    while (l < r && !Character.isLetterOrDigit(s.charAt(l))) l++;
    while (l < r && !Character.isLetterOrDigit(s.charAt(r))) r--;
    if (Character.toLowerCase(s.charAt(l)) != Character.toLowerCase(s.charAt(r))) return false;
    l++; r--;
  }
  return true;
}
```

```csharp
bool IsPalindrome(string s) {
  int l = 0, r = s.Length - 1;
  while (l < r) {
    while (l < r && !char.IsLetterOrDigit(s[l])) l++;
    while (l < r && !char.IsLetterOrDigit(s[r])) r--;
    if (char.ToLowerInvariant(s[l]) != char.ToLowerInvariant(s[r])) return false;
    l++; r--;
  }
  return true;
}
```

## Worked example 2 — Anagram via counts

```java
boolean isAnagram(String a, String b) {
  if (a.length() != b.length()) return false;
  int[] c = new int[26];
  for (int i = 0; i < a.length(); i++) {
    c[a.charAt(i) - 'a']++;
    c[b.charAt(i) - 'a']--;
  }
  for (int x : c) if (x != 0) return false;
  return true;
}
```

```csharp
bool IsAnagram(string a, string b) {
  if (a.Length != b.Length) return false;
  int[] c = new int[26];
  for (int i = 0; i < a.Length; i++) {
    c[a[i] - 'a']++;
    c[b[i] - 'a']--;
  }
  return c.All(x => x == 0);
}
```

## Interview Q&A

- **Q:** Why StringBuilder?
  **A:** Avoid quadratic concatenation from immutable strings.
- **Q:** Unicode?
  **A:** Mention code units vs graphemes; interviews usually stick to ASCII/lowercase letters.

## Pitfalls

- Assuming lowercase without normalizing  
- Modifying string like a char array in Java (can’t)

## 60-second answer

“I treat strings as immutable sequences, use builders for construction, and frequency arrays or two pointers for palindrome/anagram patterns.”

## Further study

- [String (Wikipedia)](https://en.wikipedia.org/wiki/String_(computer_science)) — sequences and immutability trade-offs
- [StringBuilder (Java SE)](https://docs.oracle.com/en/java/javase/17/docs/api/java.base/java/lang/StringBuilder.html) — mutable string building in Java
- [StringBuilder (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder) — C# mutable string buffer
- [Anagram (Wikipedia)](https://en.wikipedia.org/wiki/Anagram) — same multiset of characters; frequency counting

## Practice prompts

1. Longest common prefix  
2. String to integer (atoi)  
3. Encode/decode strings
