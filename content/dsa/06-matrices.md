---
id: dsa-matrices
title: Matrices and 2D Array Layouts
track: dsa
module: "01 Foundations"
order: 6
languages: [java, csharp]
summary: Row/column-major layouts, indexing math, sparse matrices, and common grid traversal patterns.
---

## Why this matters

Grids, images, DP tables, and adjacency matrices are 2D layouts. Off-by-one and layout confusion cause endless bugs; sparse representations matter for graphs and large empty grids.

## Definitions

- **Matrix / 2D array:** An \(m \times n\) collection addressable by `(r, c)`.
- **Row-major order:** Rows stored contiguously; index \(r \cdot n + c\) in a flat buffer (Java/C# nested arrays are row-of-rows).
- **Column-major order:** Columns contiguous (Fortran/MATLAB default); index \(c \cdot m + r\).
- **Contiguous block:** Cache-friendly access when iterating the major dimension first.
- **Sparse matrix:** Mostly zeros — store only nonzeros (COO, CSR) instead of full \(m \times n\).
- **Adjacency matrix:** \(n \times n\) boolean/weight grid for graphs — \(O(1)\) edge query, \(O(n^2)\) space.
- **In-bounds check:** Guard `0 ≤ r < m` and `0 ≤ c < n` before every neighbor step.

## Concept

```text
Row-major 3×4 logical view          Flat index = r*4 + c
r\c  0  1  2  3
0    a  b  c  d     →  [a b c d | e f g h | i j k l]
1    e  f  g  h
2    i  j  k  l
```

**Static vs dynamic:** fixed `int[m][n]` vs lists of lists. Prefer rectangular arrays when dimensions are known.

## Worked example 1 — Neighbor deltas

```java
int[][] DIRS = {{1,0},{-1,0},{0,1},{0,-1}};

boolean in(int r, int c, int m, int n) {
  return r >= 0 && r < m && c >= 0 && c < n;
}

void visitNeighbors(int[][] g, int r, int c) {
  int m = g.length, n = g[0].length;
  for (int[] d : DIRS) {
    int nr = r + d[0], nc = c + d[1];
    if (in(nr, nc, m, n)) { /* use g[nr][nc] */ }
  }
}
```

```csharp
int[][] Dirs = { new[]{1,0}, new[]{-1,0}, new[]{0,1}, new[]{0,-1} };

bool In(int r, int c, int m, int n) =>
  r >= 0 && r < m && c >= 0 && c < n;
```

## Worked example 2 — Sparse COO sum

```java
// Coordinate list: list of (r, c, value)
record Triple(int r, int c, int v) {}

int get(List<Triple> sparse, int r, int c) {
  for (var t : sparse) if (t.r() == r && t.c() == c) return t.v();
  return 0;
}
```

```csharp
record Triple(int R, int C, int V);

int Get(List<Triple> sparse, int r, int c) {
  foreach (var t in sparse)
    if (t.R == r && t.C == c) return t.V;
  return 0;
}
```

For many nonzeros, prefer hash map key `r * n + c` or CSR for row scans.

## Interview Q&A

- **Q:** When adjacency matrix vs list?  
  **A:** Matrix for dense graphs / fast edge checks; list for sparse \(E \ll V^2\).
- **Q:** Transpose complexity?  
  **A:** \(O(mn)\) time and usually new \(n \times m\) space (or in-place for square with care).
- **Q:** Why iterate columns slowly in row-major?  
  **A:** Poor cache locality — jumps by row stride.

## Pitfalls

- `g.length` vs `g[0].length` swapped  
- Jagged arrays with unequal row lengths  
- Treating diagonal DP as if off-diagonals exist without checks

## 60-second answer

“A matrix is an \(m \times n\) grid; Java/C# are row-major nested arrays. I always clamp neighbors and pick dense arrays vs sparse maps based on fill factor. Graph adjacency matrices are the same idea with \(O(1)\) edge tests.”

## Further study

- [Row- and column-major order](https://en.wikipedia.org/wiki/Row-_and_column-major_order)
- [Sparse matrix](https://en.wikipedia.org/wiki/Sparse_matrix)
- [Adjacency matrix](https://en.wikipedia.org/wiki/Adjacency_matrix)

## Practice prompts

1. Rotate an \(n \times n\) matrix 90° in place  
2. Convert adjacency list ↔ adjacency matrix  
3. Implement spiral order traversal
