---
id: pattern-backtracking
title: "Pattern: Backtracking"
track: dsa-patterns
module: "04 Recursion Patterns"
order: 41
languages: [java, csharp]
summary: Choose → explore → unchoose for permutations, combinations, and constraint search.
---

## Why this matters

Permutations, N-Queens, word search — all share the same undo template.

## Definitions

- **Backtracking:** Incremental construction with undo when a branch fails.
- **State:** Current path / board / used bitmask.
- **Pruning:** Skip branches that cannot succeed (constraints early).

## Recognition cues

- Permutations / combinations  
- Sudoku / N-Queens  
- Word search  
- Palindrome partitioning  
- Generate parentheses

## Template

```java
void bt(State s) {
  if (done(s)) { record(s); return; }
  for (Choice c : choices(s)) {
    apply(s, c);
    bt(s);
    undo(s, c);
  }
}
```

```csharp
void Bt(State s) {
  if (Done(s)) { Record(s); return; }
  foreach (var c in Choices(s)) {
    Apply(s, c);
    Bt(s);
    Undo(s, c);
  }
}
```

## Further study

- [Backtracking](https://en.wikipedia.org/wiki/Backtracking)
- [LeetCode Backtracking tag](https://leetcode.com/tag/backtracking/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Permutations  
2. Combination sum  
3. N-Queens
