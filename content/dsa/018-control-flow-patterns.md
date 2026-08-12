---
id: dsa-control-flow-patterns
title: Control Flow and Pattern Printing
track: dsa
module: "01 Foundations"
order: 18
languages: [java, csharp]
summary: Nested loops, pattern printing, and the control-flow fluency OJ basics assume.
---

## Why this matters

Before DSA patterns, you need automatic control of `if`/`for` nesting, bounds, and off-by-ones. Pattern printing is pure nested-loop practice.

## Definitions

- **Control flow:** Order of execution — branching (`if`/`switch`) and looping (`for`/`while`).
- **Nested loop:** Loop inside another; classic for 2D grids and patterns — often O(rows·cols).
- **Loop invariant:** A statement true before/after each iteration that proves correctness.
- **Pattern printing:** Emit stars/numbers by choosing, for each row, how many symbols and what padding.

## Branching templates

```java
if (x > 0) { /* … */ }
else if (x == 0) { /* … */ }
else { /* … */ }

switch (op) {
  case '+' -> sum += v;
  case '-' -> sum -= v;
  default -> throw new IllegalArgumentException();
}
```

```csharp
switch (op) {
  case '+': sum += v; break;
  case '-': sum -= v; break;
  default: throw new ArgumentException();
}
```

## Worked example — pyramid of stars

```java
void pyramid(int n) {
  for (int i = 1; i <= n; i++) {
    for (int s = 0; s < n - i; s++) System.out.print(' ');
    for (int k = 0; k < 2 * i - 1; k++) System.out.print('*');
    System.out.println();
  }
}
```

```csharp
void Pyramid(int n) {
  for (int i = 1; i <= n; i++) {
    Console.Write(new string(' ', n - i));
    Console.WriteLine(new string('*', 2 * i - 1));
  }
}
```

## Worked example — triangular number pattern

```java
void triNumbers(int n) {
  int x = 1;
  for (int i = 1; i <= n; i++) {
    for (int j = 0; j < i; j++) System.out.print((x++) + " ");
    System.out.println();
  }
}
```

## Complexity

Nested loops over `n` rows with up to `n` work → Θ(n²) output size — fine for small n in pattern problems; always check constraints.

## Interview Q&A

- **Q:** `for` vs `while`?  
  **A:** `for` when bounds known; `while` when exit depends on a dynamic condition.
- **Q:** Why patterns in DSA prep?  
  **A:** Forces correct nesting and index math — same skill as matrix traversal.

## Pitfalls

- Off-by-one in spaces vs stars  
- Using `println` inside inner loop  
- Infinite loop from wrong update (`i--` instead of `i++`)

## 60-second answer

“I treat pattern problems as nested loops with a clear formula per row for spaces and symbols. The same index discipline carries into matrices and simulation problems.”

## Further study

- Online Judge approach · Matrices · Java/C# control-flow fundamentals  

## Practice prompts

1. Print an inverted pyramid  
2. Print a hollow square  
3. FizzBuzz with clean branching
