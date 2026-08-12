---
id: dsa-recurrences-master-theorem
title: Recurrence Relations and Master Theorem
track: dsa
module: "01 Foundations"
order: 5
languages: [java, csharp]
summary: Translate recursive algorithms into recurrences and solve them with the Master Theorem and intuition.
---

## Why this matters

Divide-and-conquer (merge sort, binary search trees, many graph/DP recursions) is analyzed with recurrence relations. Interviewers expect Master Theorem fluency for classic forms.

## Definitions

- **Recurrence relation:** An equation defining \(T(n)\) in terms of smaller inputs, e.g. \(T(n) = 2T(n/2) + O(n)\).
- **Base case:** The constant-size stopping condition, e.g. \(T(1) = \Theta(1)\).
- **Divide-and-conquer cost:** Work outside recursive calls (split + combine), often \(f(n)\).
- **Master Theorem:** Closed-form cookbook for \(T(n) = aT(n/b) + f(n)\) under regularity conditions.
- **Work tree:** Expanding the recurrence level by level to sum costs across \(\log_b n\) levels.
- **Akra–Bazzi / substitution:** Alternatives when Master Theorem doesn’t apply (uneven splits, floors/ceils obsessively).

## Asymptotic bounds refresher

| Symbol | Meaning | Interview use |
|--------|---------|---------------|
| \(O\) (Big-O) | Upper bound | Default answer |
| \(\Omega\) (Omega) | Lower bound | “At least this slow” |
| \(\Theta\) (Theta) | Tight bound | When upper = lower |

## Master Theorem (standard form)

For \(T(n) = a\,T(n/b) + f(n)\) with \(a \ge 1\), \(b > 1\):

Compare \(f(n)\) to \(n^{\log_b a}\):

1. **Case 1:** If \(f(n) = O(n^{\log_b a - \varepsilon})\) for some \(\varepsilon > 0\) → \(T(n) = \Theta(n^{\log_b a})\)
2. **Case 2:** If \(f(n) = \Theta(n^{\log_b a} \log^k n)\) (often \(k=0\)) → \(T(n) = \Theta(n^{\log_b a} \log^{k+1} n)\)
3. **Case 3:** If \(f(n) = \Omega(n^{\log_b a + \varepsilon})\) and regularity → \(T(n) = \Theta(f(n))\)

## Classic mappings

| Algorithm | Recurrence | Result |
|-----------|------------|--------|
| Binary search | \(T(n)=T(n/2)+O(1)\) | \(\Theta(\log n)\) |
| Merge sort | \(T(n)=2T(n/2)+O(n)\) | \(\Theta(n \log n)\) |
| Naive recursive fib | \(T(n)=T(n-1)+T(n-2)+O(1)\) | \(\Theta(\phi^n)\) — *not* Master form |
| Karatsuba-ish | \(T(n)=3T(n/2)+O(n)\) | \(\Theta(n^{\log_2 3})\) |

## Worked example 1 — Merge sort

```java
void mergeSort(int[] a, int lo, int hi) {
  if (hi - lo <= 1) return;
  int mid = (lo + hi) >>> 1;
  mergeSort(a, lo, mid);      // T(n/2)
  mergeSort(a, mid, hi);      // T(n/2)
  merge(a, lo, mid, hi);      // Θ(n)
}
// T(n) = 2T(n/2) + Θ(n) → Θ(n log n)
```

```csharp
void MergeSort(int[] a, int lo, int hi) {
  if (hi - lo <= 1) return;
  int mid = (lo + hi) / 2;
  MergeSort(a, lo, mid);
  MergeSort(a, mid, hi);
  Merge(a, lo, mid, hi);
}
```

## Worked example 2 — Spot the case

\(a=2, b=2 \Rightarrow n^{\log_b a} = n\).

- \(f(n)=1\) → Case 1 → \(\Theta(n)\)
- \(f(n)=n\) → Case 2 → \(\Theta(n \log n)\)
- \(f(n)=n^2\) → Case 3 → \(\Theta(n^2)\)

## Interview Q&A

- **Q:** Why isn’t Fibonacci Master-applicable?  
  **A:** Subproblems shrink by 1 and 2, not \(n/b\) equal splits.
- **Q:** Do floors/ceils matter?  
  **A:** For interviews, ignore — they don’t change asymptotic class.
- **Q:** Recursion stack space?  
  **A:** Separate from \(T(n)\) time — depth is often \(O(\log n)\) or \(O(n)\).

## Pitfalls

- Forgetting the combine cost \(f(n)\)  
- Using Master Theorem on overlapping DP without memo (wrong model)  
- Saying “\(O(n \log n)\)” when you mean \(\Theta\) but that’s usually fine in interviews

## 60-second answer

“I write \(T(n)\) from the recursive structure, then apply Master Theorem: compare the non-recursive work \(f(n)\) to \(n^{\log_b a}\). Merge sort is the Case-2 poster child — \(\Theta(n \log n)\).”

## Further study

- [Master theorem (Wikipedia)](https://en.wikipedia.org/wiki/Master_theorem_(analysis_of_algorithms))
- [Recurrence relation (Wikipedia)](https://en.wikipedia.org/wiki/Recurrence_relation)
- CLRS Ch. 4 — divide-and-conquer recurrences

## Practice prompts

1. Solve \(T(n)=4T(n/2)+n\)  
2. Solve \(T(n)=T(n/2)+n\)  
3. Explain why quicksort average vs worst needs two different recurrences
