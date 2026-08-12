---
id: dsa-recursion-basics
title: Recursion Basics
track: dsa
module: "01 Foundations"
order: 7
languages: [java, csharp]
summary: Base cases, call stacks, recursion trees, and converting simple recursion to iteration.
---

## Why this matters

Trees, backtracking, and divide-and-conquer all start with recursion fluency — base case, recursive case, and stack cost.

## Definitions

- **Recursion:** A function solving a problem by calling itself on smaller inputs.
- **Base case:** Input that returns without further recursive calls.
- **Recursive case:** Work + one or more calls on reduced subproblems.
- **Call stack:** Frames pushed per invocation; depth too large → stack overflow.
- **Recursion tree:** Visual expansion of calls — helps derive time complexity.
- **Tail recursion:** Last action is the recursive call (languages may optimize; Java/C# generally don’t).

## Template

```text
f(x):
  if base(x): return answer
  // optional pre-work
  ans = combine(f(smaller(x)), ...)
  // optional post-work
  return ans
```

## Worked example 1 — Factorial / power

```java
long fact(int n) {
  if (n <= 1) return 1;
  return n * fact(n - 1);
}
long pow(long a, int n) {
  if (n == 0) return 1;
  long half = pow(a, n / 2);
  return (n % 2 == 0) ? half * half : half * half * a;
}
```

```csharp
long Fact(int n) => n <= 1 ? 1 : n * Fact(n - 1);
long Pow(long a, int n) {
  if (n == 0) return 1;
  long half = Pow(a, n / 2);
  return n % 2 == 0 ? half * half : half * half * a;
}
```

## Worked example 2 — Print 1..n then unwind

```java
void upDown(int n) {
  if (n == 0) return;
  upDown(n - 1);
  System.out.println(n); // after recursive return
}
```

Understand pre-order vs post-order work relative to the recursive call.

## Complexity intuition

| Pattern | Time | Stack space |
|---------|------|-------------|
| Linear chain `f(n)→f(n-1)` | O(n) | O(n) |
| Binary split balanced | often O(n) or O(n log n) | O(log n) |
| Branching without memo | exponential | O(n) depth |

## Interview Q&A

- **Q:** Recursion vs iteration?  
  **A:** Recursion matches inductive structure; iteration saves stack and is often faster.
- **Q:** How to avoid stack overflow?  
  **A:** Smaller depth, convert to explicit stack/queue, or memoized DP bottom-up.
- **Q:** Multiple recursive calls?  
  **A:** Think tree — total nodes ≈ work.

## Pitfalls

- Missing base case → infinite recursion  
- Not reducing problem size each call  
- Ignoring stack space in “O(1) space” claims

## 60-second answer

“I state base case and how the input shrinks. I sketch the recursion tree for complexity and remember the call stack counts toward space.”

## Further study

- Recurrence / Master Theorem lesson  
- Backtracking lesson for search-style recursion

## Practice prompts

1. Sum of array via recursion  
2. Reverse a string recursively  
3. Climbing stairs naive recursion then memoize
