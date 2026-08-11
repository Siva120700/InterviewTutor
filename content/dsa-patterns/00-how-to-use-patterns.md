---
id: pattern-howto
title: How to Study With Patterns
track: dsa-patterns
module: "00 Start Here"
order: 1
languages: [java, csharp]
summary: Pattern-based interview prep — recognize the cue, apply a template, then vary the problem.
---

## Why this matters

Grinding random problems is slow. Pattern prep (popularized by resources like Grokking / NeetCode roadmaps) teaches you to **name the approach first**, then code a known template.

## Definitions

- **Coding pattern:** A reusable problem-solving template (e.g. sliding window) that covers many interview questions.
- **Recognition cue:** Phrases/constraints that hint which pattern fits (“contiguous subarray”, “sorted array”, “cycle in list”).
- **Template:** The skeleton loop/state you reuse, then customize.
- **Blind 75 / NeetCode 150:** Curated problem lists often grouped by pattern for focused practice.
- **Transfer:** Applying the same pattern to a problem you have never seen.

## Concept

For every new problem, force this order:

1. **Restate** constraints and ask clarifying questions  
2. **Name the pattern** out loud  
3. **Sketch the template** (indices, queue, heap, dp state)  
4. **Dry-run** a small example  
5. **Code** + complexity  
6. **Edge cases**

```text
if contiguous + window constraint     → Sliding Window
if sorted + pair/compare ends         → Two Pointers
if cycle / middle of list             → Fast & Slow
if intervals overlap                  → Merge Intervals
if level-by-level tree                → Tree BFS
if path/subtree properties            → Tree DFS
if top K / running median             → Heap / Two Heaps
if generate combinations              → Subsets / Backtracking
if dependencies / courses             → Topological Sort
```

## Study plan

1. Read the pattern lesson (template + cues)  
2. Code the template from memory  
3. Solve 3–5 related problems (easy → medium)  
4. Only then move to the next pattern

## Further study

- [NeetCode](https://neetcode.io/) — curated pattern-oriented practice roadmaps.
- [Blind 75 list](https://leetcode.com/discuss/general-discussion/460599/blind-75-leetcode-questions) — classic focused problem set.
- [Big-O cheat sheet](https://www.bigocheatsheet.com/) — complexity reference while practicing.

## Practice prompts

1. Pick 5 recent problems you solved and label each with a pattern  
2. Write the sliding-window and two-pointer templates from memory  
3. Build a personal cue → pattern cheat sheet on one page
