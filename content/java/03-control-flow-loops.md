---
id: java-control-flow-loops
title: Control Flow — If, Switch, Loops
track: java
module: "00 Language Fundamentals"
order: 3
languages: [java]
summary: if/else, switch, for/while/do-while, break/continue — Java control flow for DSA.
---

## Why this matters

DSA code is mostly loops and branches. Clean control flow prevents off-by-ones and infinite loops under interview pressure.

## Definitions

- **if / else if / else:** Conditional branching on a boolean expression.
- **switch:** Multi-way branch on integrals/strings/enums (and modern switch expressions).
- **for:** Loop with init / condition / update — default for counted iteration.
- **while / do-while:** Condition-first vs run-at-least-once loops.
- **break / continue:** Exit loop vs skip to next iteration (`break` can target labeled loops).
- **Enhanced for:** `for (T x : collection)` — read-only iteration over arrays/iterables.

## if / else

```java
if (n % 2 == 0) {
  System.out.println("even");
} else if (n % 3 == 0) {
  System.out.println("divisible by 3");
} else {
  System.out.println("other");
}
```

## switch

```java
String grade = switch (score / 10) {
  case 10, 9 -> "A";
  case 8 -> "B";
  case 7 -> "C";
  default -> "D";
};
```

Classic `switch` with `break` still appears in older code — falling through without `break` is a common bug.

## Loops

```java
for (int i = 0; i < n; i++) { /* 0..n-1 */ }

int i = 0;
while (i < n) { i++; }

do {
  // runs once even if condition false afterward
} while (false);

for (int x : arr) {
  if (x < 0) continue;
  if (x == target) break;
}
```

## Labeled break (rare but useful)

```java
outer:
for (int r = 0; r < m; r++) {
  for (int c = 0; c < n; c++) {
    if (grid[r][c] == 0) break outer;
  }
}
```

## Standard micro-patterns

```java
// factorial
long fact = 1;
for (int i = 2; i <= n; i++) fact *= i;

// fizzbuzz
for (int i = 1; i <= n; i++) {
  if (i % 15 == 0) System.out.println("FizzBuzz");
  else if (i % 3 == 0) System.out.println("Fizz");
  else if (i % 5 == 0) System.out.println("Buzz");
  else System.out.println(i);
}
```

## Interview Q&A

- **Q:** When `do-while`?  
  **A:** Menu loops / “ask at least once” — uncommon in pure DSA.
- **Q:** Modify collection in enhanced for?  
  **A:** Don’t — use iterator/`removeIf` or index loop.
- **Q:** `for(;;)` forever?  
  **A:** Valid infinite loop; prefer `while (true)` for clarity.

## Pitfalls

- `=` instead of `==` in conditions (compile error on boolean, logic bug if allowed)  
- Off-by-one: `i <= n` vs `i < n`  
- Switch fall-through forgetting `break`

## 60-second answer

“I default to `for` for counted loops and `while` for condition-driven ones. Branches stay flat when possible. `break`/`continue` handle early exit without deep nesting.”

## Further study

- Arrays & methods · DSA Control Flow patterns  

## Practice prompts

1. Print all primes ≤ n with nested loops  
2. Rewrite a nested if chain as switch where sensible  
3. Sum of digits with `while (n > 0)`
