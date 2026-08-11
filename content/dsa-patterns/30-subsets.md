---
id: pattern-subsets
title: "Pattern: Subsets"
track: dsa-patterns
module: "04 Recursion Patterns"
order: 40
languages: [java, csharp]
summary: BFS/cascade or backtracking to generate power set and combinations.
---

## Why this matters

Subset generation is the base for combinations and many “enumerate options” problems.

## Definitions

- **Subsets pattern:** Build all subsets by including/excluding each element (or cascading copy+append).
- **Power set:** All 2^n subsets of n elements.
- **Cascade BFS style:** Start `[[]]`; for each num, append copies with num added.

## Recognition cues

- Subsets / combinations  
- Letter case permutation  
- Generate unique subsets with duplicates (sort + skip)

## Template — cascade

```java
List<List<Integer>> subsets(int[] nums) {
  List<List<Integer>> res = new ArrayList<>();
  res.add(new ArrayList<>());
  for (int x : nums) {
    int n = res.size();
    for (int i = 0; i < n; i++) {
      List<Integer> next = new ArrayList<>(res.get(i));
      next.add(x);
      res.add(next);
    }
  }
  return res;
}
```

```csharp
IList<IList<int>> Subsets(int[] nums) {
  var res = new List<IList<int>> { new List<int>() };
  foreach (int x in nums) {
    int n = res.Count;
    for (int i = 0; i < n; i++) {
      var next = res[i].ToList();
      next.Add(x);
      res.Add(next);
    }
  }
  return res;
}
```

## Further study

- [Power set](https://en.wikipedia.org/wiki/Power_set)
- [LeetCode Backtracking tag](https://leetcode.com/tag/backtracking/)
- [NeetCode](https://neetcode.io/)

## Practice prompts

1. Subsets  
2. Subsets II (duplicates)  
3. Letter case permutation
