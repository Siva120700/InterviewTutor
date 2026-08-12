---
id: java-variables-types-operators
title: Variables, Types, Operators, and Input
track: java
module: "00 Language Fundamentals"
order: 2
languages: [java]
summary: Literals, primitives vs references, operators, casting, printing, and Scanner input.
---

## Why this matters

Every bug in early Java code is a type, cast, or operator-precedence mistake. Interviews still poke at overflow, `==` on strings, and integer division.

## Definitions

- **Literal:** A fixed value in source (`42`, `3.14`, `'a'`, `"hi"`, `true`).
- **Primitive types:** `byte short int long float double char boolean` — hold values directly.
- **Reference types:** Classes/arrays/interfaces — variables hold references to heap objects.
- **Variable:** Named storage with a type; local variables must be assigned before use.
- **Casting / conversion:** Widening (safe) vs narrowing (explicit cast, may truncate).
- **Operator precedence:** `*` before `+`; use parentheses when unclear.

## Primitives at a glance

| Type | Bits | Notes |
|------|------|-------|
| byte | 8 | rare in interviews |
| short | 16 | rare |
| int | 32 | default for integers |
| long | 64 | suffix `L` |
| float | 32 | suffix `f` — prefer `double` |
| double | 64 | default floating |
| char | 16 | UTF-16 code unit |
| boolean | — | `true`/`false` |

## Variables and print

```java
int count = 3;
long big = 1_000_000_000L;
double pi = 3.14159;
boolean ok = true;
String name = "Ada"; // reference

System.out.println("count=" + count);
System.out.printf("pi=%.2f%n", pi);
```

## Operators

```java
int a = 7, b = 2;
int sum = a + b;
int div = a / b;      // 3 — integer division
int mod = a % b;      // 1
boolean cmp = a > b;  // relational
boolean both = cmp && a != 0; // short-circuit AND
int x = 1;
x += 2;               // compound assignment
x++;                  // increment
```

**Bitwise:** `& | ^ ~ << >> >>>` — see DSA Bit Manipulation for patterns.

## Type conversion

```java
int i = 7;
long L = i;          // widening
int back = (int) L;  // narrowing cast
int fromDouble = (int) 3.9; // truncates toward 0 → 3

String s = String.valueOf(i);
int parsed = Integer.parseInt("42");
```

## User input

```java
import java.util.Scanner;

Scanner sc = new Scanner(System.in);
int n = sc.nextInt();
String line = sc.next();      // token
String rest = sc.nextLine();  // rest of line — watch leftover newline
```

In contests, `BufferedReader` + `StringTokenizer` is faster than `Scanner`.

## Interview Q&A

- **Q:** `==` vs `.equals` for String?  
  **A:** `==` compares references; `.equals` compares contents. Always `.equals` for value.
- **Q:** Why `1/2 == 0`?  
  **A:** Integer division truncates; use `1.0/2` or cast.
- **Q:** Overflow of `int`?  
  **A:** Wraps silently (two’s complement). Use `long` or check bounds.

## Pitfalls

- `nextInt()` then `nextLine()` eating an empty line  
- Comparing strings with `==`  
- Narrowing cast without thinking about truncation

## 60-second answer

“Primitives hold values; objects are references. I watch integer division, overflow, and string equality. Input via Scanner is fine for interviews; know widening vs narrowing casts.”

## Further study

- Control flow lesson next · Language Core (OOP)  

## Practice prompts

1. Swap two ints without a temp (xor or sum)  
2. Parse `"12 34"` and print the sum  
3. Explain what `(int)(0.1+0.2)` is not
