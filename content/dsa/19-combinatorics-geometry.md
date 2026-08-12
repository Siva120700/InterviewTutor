---
id: dsa-combinatorics-geometry
title: Combinatorics and Computational Geometry Basics
track: dsa
module: "08 Advanced Tools"
order: 76
languages: [java, csharp]
summary: nCr, lattice paths, and interview geometry — lines, rectangles, circles, orientations.
---

## Why this matters

Grid paths, unique BSTs, and rectangle/overlap problems mix counting with light geometry. Interviews want formulas + careful overflow/mod handling.

## Definitions

- **nCr / binomial:** Ways to choose r from n — \(\binom{n}{r}=n!/(r!(n-r)!)\).
- **Pascal identity:** \(\binom{n}{r}=\binom{n-1}{r}+\binom{n-1}{r-1}\).
- **Lattice paths:** Paths on a grid using only right/up often equal a binomial coefficient.
- **Orientation:** Cross product sign tells left/right turn for points A→B→C.
- **Axis-aligned rectangle:** Defined by min/max x/y; overlap via interval overlap on both axes.
- **Circle:** Center + radius; distance compare with \(r^2\) to avoid floats.

## Worked example 1 — nCr mod prime

```java
long[] fact, invFact;
void precompute(int n, long mod) {
  fact = new long[n + 1]; invFact = new long[n + 1];
  fact[0] = 1;
  for (int i = 1; i <= n; i++) fact[i] = fact[i - 1] * i % mod;
  invFact[n] = modPow(fact[n], mod - 2, mod); // Fermat
  for (int i = n; i > 0; i--) invFact[i - 1] = invFact[i] * i % mod;
}
long nCr(int n, int r, long mod) {
  if (r < 0 || r > n) return 0;
  return fact[n] * invFact[r] % mod * invFact[n - r] % mod;
}
```

## Worked example 2 — Rectangle overlap

```java
boolean overlap(int[] a, int[] b) {
  // [x1,y1,x2,y2] bottom-left / top-right
  return a[0] < b[2] && b[0] < a[2] && a[1] < b[3] && b[1] < a[3];
}
```

```csharp
bool Overlap(int[] a, int[] b) =>
  a[0] < b[2] && b[0] < a[2] && a[1] < b[3] && b[1] < a[3];
```

## Worked example 3 — Cross product orientation

```java
long cross(int[] a, int[] b, int[] c) {
  return (long)(b[0] - a[0]) * (c[1] - a[1]) - (long)(b[1] - a[1]) * (c[0] - a[0]);
}
// >0 left turn, <0 right, 0 collinear (for a->b->c)
```

## Interview Q&A

- **Q:** Unique paths on grid with obstacles?  
  **A:** DP — combinatorics only on empty grids.
- **Q:** Floating geometry?  
  **A:** Prefer integers + squared distances; state epsilon only if required.

## Pitfalls

- Computing factorials without mod then reducing  
- Inclusive/exclusive edges on rectangles  
- Integer overflow in cross products — cast to `long`

## 60-second answer

“Counting problems → binomials / DP. Geometry → overlap tests and orientation via cross products, avoiding floats when possible.”

## Further study

- [Binomial coefficient](https://en.wikipedia.org/wiki/Binomial_coefficient)
- [Cross product](https://en.wikipedia.org/wiki/Cross_product)

## Practice prompts

1. Unique Paths  
2. Rectangle Area / overlap  
3. Valid square / boomerang
