---
id: dsa-game-theory
title: Game Theory Basics
track: dsa
module: "08 Advanced Tools"
order: 75
languages: [java, csharp]
summary: Impartial games, winning/losing positions, Nim and grundy-style interview patterns.
---

## Why this matters

Stone games, Nim variants, and “optimal play” problems reduce to classifying positions as winning or losing under perfect information.

## Definitions

- **Impartial game:** Both players have the same moves from a position (Nim-like).
- **Winning position:** The player to move can force a win.
- **Losing position:** Every move leaves a winning position for the opponent (or no moves — often lose).
- **Nim:** Heaps of stones; move removes any count from one heap; XOR of heap sizes decides winner.
- **Nimber / Grundy number:** For a graph game, mex of child grundies; XOR combines independent components.
- **DP on games:** `dp[i] = true` if some move to a losing position for opponent.

## Worked example 1 — Subtract-a-square / simple stones

```java
// stones n; can remove 1..m; player who takes last wins
boolean firstWins(int n, int m) {
  boolean[] win = new boolean[n + 1];
  for (int i = 1; i <= n; i++) {
    for (int k = 1; k <= m && k <= i; k++)
      if (!win[i - k]) { win[i] = true; break; }
  }
  return win[n];
}
```

```csharp
bool FirstWins(int n, int m) {
  var win = new bool[n + 1];
  for (int i = 1; i <= n; i++)
    for (int k = 1; k <= m && k <= i; k++)
      if (!win[i - k]) { win[i] = true; break; }
  return win[n];
}
```

## Worked example 2 — Nim XOR

```java
boolean nimWin(int[] heaps) {
  int x = 0;
  for (int h : heaps) x ^= h;
  return x != 0; // first player wins if XOR != 0
}
```

## Interview Q&A

- **Q:** Misère (last move loses)?  
  **A:** Different analysis near the end — mention separately.
- **Q:** Minimax?  
  **A:** For scored/partisan games; DSA interviews usually stick to win/lose DP or Nim.

## Pitfalls

- Swapping “last move wins” vs loses  
- Forgetting base case `dp[0]`  
- XORing when heaps aren’t independent impartial games

## 60-second answer

“I classify positions as win/lose by DP: a position is winning if I can move to a losing one. Multi-heap impartial games combine with Nim XOR / Grundy numbers.”

## Further study

- [Nim](https://en.wikipedia.org/wiki/Nim)
- [Sprague–Grundy theorem](https://en.wikipedia.org/wiki/Sprague%E2%80%93Grundy_theorem)

## Practice prompts

1. Nim Game (LeetCode)  
2. Divisor game  
3. Stone Game series intuition (DP vs math)
