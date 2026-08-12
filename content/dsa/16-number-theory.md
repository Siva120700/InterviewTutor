---
id: dsa-number-theory
title: Number Theory for Interviews
track: dsa
module: "01 Foundations"
order: 16
languages: [java, csharp]
summary: GCD/LCM, primes, modular arithmetic, and sieve — the math toolkit for DSA.
---

## Why this matters

Contest and OA problems love GCD, primes, modular inverse, and fast exponentiation. You need clean templates more than deep number theory.

## Definitions

- **GCD / LCM:** Greatest common divisor; `lcm(a,b)=a/gcd*b` (watch overflow).
- **Euclidean algorithm:** `gcd(a,b)=gcd(b,a%b)` — O(log min(a,b)).
- **Prime:** Only divisors 1 and itself; test via trial up to \(\sqrt{n}\).
- **Sieve of Eratosthenes:** Mark multiples to list primes ≤ n in ~O(n log log n).
- **Modular arithmetic:** Work in \(\mathbb{Z}/m\mathbb{Z}\); add/mul take `% m` carefully (non-neg remainder).
- **Fast pow (binary exponentiation):** Compute \(a^n \bmod m\) in O(log n).
- **Modular inverse:** \(a\cdot a^{-1}\equiv 1 \pmod m\) when gcd(a,m)=1; use Fermat if m prime (\(a^{m-2}\)).

## Worked example 1 — GCD / LCM

```java
int gcd(int a, int b) { return b == 0 ? Math.abs(a) : gcd(b, a % b); }
long lcm(int a, int b) { return Math.abs((long) a / gcd(a, b) * b); }
```

```csharp
int Gcd(int a, int b) => b == 0 ? Math.Abs(a) : Gcd(b, a % b);
long Lcm(int a, int b) => Math.Abs((long)a / Gcd(a, b) * b);
```

## Worked example 2 — Modpow

```java
long modPow(long a, long e, long mod) {
  long r = 1 % mod; a %= mod;
  while (e > 0) {
    if ((e & 1) == 1) r = r * a % mod;
    a = a * a % mod; e >>= 1;
  }
  return r;
}
```

```csharp
long ModPow(long a, long e, long mod) {
  long r = 1 % mod; a %= mod;
  while (e > 0) {
    if ((e & 1) == 1) r = r * a % mod;
    a = a * a % mod; e >>= 1;
  }
  return r;
}
```

## Worked example 3 — Sieve

```java
boolean[] sieve(int n) {
  boolean[] p = new boolean[n + 1];
  Arrays.fill(p, true);
  if (n >= 0) p[0] = false;
  if (n >= 1) p[1] = false;
  for (int i = 2; (long) i * i <= n; i++) if (p[i])
    for (int j = i * i; j <= n; j += i) p[j] = false;
  return p;
}
```

## Interview Q&A

- **Q:** Negative mods in Java?  
  **A:** `%` can be negative — normalize `((x%m)+m)%m`.
- **Q:** Factorize n?  
  **A:** Trial divide by primes ≤ √n after sieve or on the fly.
- **Q:** nCr mod p?  
  **A:** Precompute factorials + inverse factorials when p is prime.

## Pitfalls

- Overflow before mod  
- Off-by-one in sieve loops  
- Using floating `Math.pow` for integers

## 60-second answer

“I keep Euclid GCD, binary modpow, and Eratosthenes as templates. For combinatorics under a prime mod I precompute factorial inverses.”

## Further study

- [Euclidean algorithm](https://en.wikipedia.org/wiki/Euclidean_algorithm)
- [Sieve of Eratosthenes](https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes)

## Practice prompts

1. Count primes ≤ n  
2. Super Pow / modular exponent  
3. GCD of an array
