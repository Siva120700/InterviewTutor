---
id: dsa-complexity-big-o
title: Complexity and Big-O
track: dsa
module: "01 Foundations"
order: 1
languages: [java, csharp]
summary: Time/space complexity, common bounds, and how to analyze loops — the language of every DSA interview.
---

## Why this matters

Every solution discussion starts with complexity. You must compare O(n) vs O(n log n) vs O(n²) confidently and spot hidden costs (sorting, hashmap, recursion stack).

## Definitions

- **Big-O (O):** An asymptotic upper bound on how time or space grows as input size n → ∞, ignoring constants and lower-order terms.
- **Theta (Θ) / Omega (Ω):** Θ is a tight bound (both upper and lower); Ω is a lower bound — interviews usually accept Big-O unless they ask for Θ.
- **Time complexity:** How the number of dominant operations scales with input size (state worst case unless you explicitly mean average).
- **Space complexity:** Extra memory beyond the input, including auxiliary arrays and recursion call-stack depth.
- **Worst-case vs average-case:** Worst case is the maximum cost over inputs of size n; average case (e.g., hashing) needs a stated input model.
- **Amortized complexity:** Average cost per operation over a long sequence, even if occasional ops (array resize) are expensive.
- **Dominant term:** The fastest-growing part of a cost expression that determines Big-O after dropping constants.

## Concept

**Big-O** upper-bounds growth as input size → ∞ (ignore constants/lower terms).

| Bound | Name | Example |
|-------|------|---------|
| O(1) | Constant | Hash map get average |
| O(log n) | Logarithmic | Binary search |
| O(n) | Linear | Single scan |
| O(n log n) | Linearithmic | Efficient sort |
| O(n²) | Quadratic | Nested loops |
| O(2^n) | Exponential | Naive subsets |
| O(n!) | Factorial | Naive permutations |

**Space:** extra memory beyond input. Recursion depth counts.

## Worked examples

```java
// O(n) time, O(1) space
int sum(int[] a) {
  int s = 0;
  for (int x : a) s += x;
  return s;
}

// O(n²) time
boolean hasPairBrute(int[] a, int t) {
  for (int i = 0; i < a.length; i++)
    for (int j = i + 1; j < a.length; j++)
      if (a[i] + a[j] == t) return true;
  return false;
}
```

```csharp
int Sum(int[] a) {
  int s = 0;
  foreach (var x in a) s += x;
  return s;
}
```

**Amortized:** ArrayList/`List` append is amortized O(1) despite occasional resize.

## Interview Q&A

- **Q:** Best vs average vs worst?
  **A:** Interviews usually want worst-case unless you say average (hashing).
- **Q:** Is n² always bad?
  **A:** Fine for n ≤ 10³–10⁴; impossible for n = 10⁶.
- **Q:** Log base?
  **A:** Irrelevant in Big-O; usually log₂ from divide-and-conquer.

## Pitfalls

- Calling a O(n) method inside a loop without counting it  
- Forgetting sort cost before two-pointers  
- Ignoring output size (returning O(n²) pairs)

## 60-second answer

“I count dominant operations as n grows. Nested loops over n are n²; divide-and-conquer is often n log n. I always state time and extra space, including recursion stack.”

## Further study

- [Big O notation (Wikipedia)](https://en.wikipedia.org/wiki/Big_O_notation) — precise definition of asymptotic bounds used in interviews
- [Time complexity (Wikipedia)](https://en.wikipedia.org/wiki/Time_complexity) — common complexity classes and how they arise
- [Amortized analysis (Wikipedia)](https://en.wikipedia.org/wiki/Amortized_analysis) — why ArrayList append is amortized O(1)
- [Space complexity (Wikipedia)](https://en.wikipedia.org/wiki/Space_complexity) — auxiliary memory and recursion stack costs

## Practice prompts

1. Analyze merge sort  
2. Complexity of nested loop where inner runs `n-i` times  
3. When is O(n log n) better than O(n) with large constants?
