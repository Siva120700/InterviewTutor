---
id: java-arrays-methods
title: Arrays and Methods
track: java
module: "00 Language Fundamentals"
order: 4
languages: [java]
summary: 1D/2D arrays, iteration, and methods — arguments, return types, and local scope.
---

## Why this matters

Arrays are the default interview storage. Methods keep solutions readable — pass arguments, return results, keep locals tight.

## Definitions

- **Array:** Fixed-length contiguous object holding elements of one type; `length` is a field.
- **1D / 2D / jagged:** `int[]`, `int[][]` rectangular or rows of different lengths.
- **Method:** Named block with signature `(modifiers) returnType name(params)`.
- **Argument / parameter:** Value passed in vs name in the signature.
- **Pass-by-value:** Java always passes values — for objects, the *reference value* is copied.
- **Local variable:** Scoped to the method/block; not visible outside.
- **Standard library methods:** `Math`, `Arrays`, `String` helpers you should know cold.

## Arrays

```java
int[] a = new int[5];          // zeros
int[] b = {1, 2, 3};
int n = b.length;

int[][] g = new int[3][4];     // 3x4
int[][] jagged = { {1,2}, {3} };

for (int i = 0; i < b.length; i++) System.out.println(i + ": " + b[i]);
for (int x : b) System.out.println(x);
```

**Copy:** `Arrays.copyOf`, `System.arraycopy`, or `b.clone()` (shallow for objects).

## Methods

```java
static int sum(int[] a) {
  int s = 0;
  for (int x : a) s += x;
  return s;
}

static void swap(int[] a, int i, int j) {
  int t = a[i]; a[i] = a[j]; a[j] = t; // mutates array object
}

static int max(int a, int b) { return a >= b ? a : b; }
```

```java
public class Demo {
  public static void main(String[] args) {
    int[] xs = {3, 1, 2};
    System.out.println(sum(xs));
    swap(xs, 0, 2);
  }
}
```

## Why arguments and return types?

- **Inputs** as parameters keep methods reusable  
- **Return** communicates the result without globals  
- **Side effects** (mutating arrays) should be obvious from the name (`sortInPlace`)

## Locals and scope

```java
static int f(int x) {
  int y = x + 1; // local
  {
    int z = y * 2; // block scope
    return z;
  }
  // z not visible here
}
```

## Handy library calls

```java
Math.max(a, b);
Math.min(a, b);
Math.abs(x);
Arrays.sort(a);
Arrays.fill(a, 0);
Arrays.toString(a);
Arrays.binarySearch(a, key); // sorted array
String.valueOf(42);
```

## Practice patterns (array drills)

1. Print each element with index  
2. Find min/max in one pass  
3. Reverse in place  
4. Frequency count with `int[256]` or `HashMap`  
5. Two-sum with nested loop then hash

## Interview Q&A

- **Q:** Does `swap(int a, int b)` swap caller ints?  
  **A:** No — primitives passed by value. Swap via array/indices or return a pair.
- **Q:** `int[]` default values?  
  **A:** `0` / `false` / `null` for reference component types.
- **Q:** Varargs?  
  **A:** `void f(int... xs)` is syntactic sugar for `int[]`.

## Pitfalls

- `ArrayIndexOutOfBoundsException`  
- Confusing `length` (arrays) with `length()` (String) / `size()` (List)  
- Returning reference to internal mutable array (encapsulation leak)

## 60-second answer

“Arrays are fixed-length objects; I iterate with index or enhanced for. Methods take parameters and return results; Java is pass-by-value, so mutating an array’s elements is visible to the caller.”

## Further study

- Language Core (OOP, equals/hashCode) · Collections · DSA Arrays  

## Practice prompts

1. Method `average(int[] a)` returning `double`  
2. Rotate array right by k (in place)  
3. Check if two arrays are equal ignoring order (sort or frequency)
