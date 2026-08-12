---
id: dsa-online-judge
title: Online Judges and Problem Approach
track: dsa
module: "01 Foundations"
order: 17
languages: [java, csharp]
summary: How OJ platforms work, constraints-first thinking, and a repeatable way to tear down a problem.
---

## Why this matters

Theory alone doesn’t ship ACs. You need a habit: read constraints → pick complexity budget → choose pattern → code → dry-run edge cases.

## Definitions

- **Online judge (OJ):** Autograder that runs your program on hidden tests (LeetCode, Codeforces, AtCoder, etc.).
- **Verdict:** Result label — Accepted, Wrong Answer, TLE, MLE, Runtime Error, Compilation Error.
- **Constraints:** Bounds on `n`, value ranges, time/memory limits that dictate feasible complexity.
- **Time limit heuristic:** ~10⁸ simple ops/sec in interviews; if `n=10⁵`, aim ≤ O(n log n).
- **Sample vs hidden tests:** Samples teach the format; hidden cases catch edges you must invent.
- **Problem teardown:** Restate I/O, constraints, examples, then brute → optimize.

## Complexity budget cheat sheet

| Typical `n` | Feasible |
|-------------|----------|
| ≤ 20 | \(2^n\), \(n!\) with pruning |
| ≤ 100 | \(n^3\), \(n^2\log n\) |
| ≤ 10³ | \(n^2\) |
| ≤ 10⁵ | \(n\log n\), \(n\) |
| ≤ 10⁶+ | \(n\) or better; careful constants |

## Approach template

1. **Restate** — inputs, outputs, exact success condition  
2. **Constraints** — pick Big-O budget  
3. **Brute** — say the naive idea (even if too slow)  
4. **Pattern** — two pointers / hash / DP / graph / …  
5. **Edge cases** — empty, n=1, duplicates, negatives, overflow  
6. **Code** — clear names; one responsibility per helper  
7. **Dry-run** — walk one sample + one nasty case on paper  

## Worked example — teardown

**Problem:** Longest substring without repeating characters.  
**Constraints:** `n ≤ 5·10⁴`.  
**Budget:** O(n) or O(n log n).  
**Brute:** Check all substrings O(n²)·O(n).  
**Optimize:** Sliding window + last-seen index map → O(n).  
**Edges:** empty string, all unique, all same char.

## Platforms (what they’re for)

| Platform | Strength |
|----------|----------|
| LeetCode | Interview-style tagged problems |
| Codeforces | Speed + harder algos |
| AtCoder | Clean statements, strong samples |
| InterviewTutor Practice sheet | Topic roadmap + links |

## Interview Q&A

- **Q:** TLE but algorithm looks right?  
  **A:** Hidden \(O(n^2)\), slow language constants, or accidental extra log factor — re-check loops and data structures.
- **Q:** WA only on hidden tests?  
  **A:** Missed edges: overflow (`long`), off-by-one, unsorted assumptions, mutability.
- **Q:** Should I memorize solutions?  
  **A:** Memorize *patterns* and templates; reconstruct solutions from the pattern.

## Pitfalls

- Coding before reading constraints  
- Optimizing a wrong problem statement  
- Ignoring 64-bit overflow and 0-based vs 1-based indices

## 60-second answer

“I tear the problem down: constraints → complexity budget → brute → pattern → edges → code → dry-run. OJ verdicts tell me whether I failed correctness, speed, or memory.”

## Further study

- Complexity lesson (Big-O)  
- Practice → DSA Sheet in this app  

## Practice prompts

1. For `n=2·10⁵`, list algorithms that are too slow  
2. Write edge cases for “merge intervals”  
3. Explain WA vs TLE vs RE in one sentence each
