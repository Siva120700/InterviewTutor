---
id: dsa-hashing
title: Hashing Patterns
track: dsa
module: "02 Patterns"
order: 12
languages: [java, csharp]
summary: Frequency maps, two-sum, anagrams, prefix tricks — when hashing beats sorting.
---

## Why this matters

Hash maps turn “have I seen X?” into expected O(1). Many Easy/Medium array problems are frequency or complement lookups.

## Definitions

- **Hash map:** A key→value store with expected O(1) get/put that trades space for fast lookups (count, index, or list per key).
- **Hash set:** A membership structure for “have I seen this key?” without storing an associated value.
- **Frequency map:** A map from value → occurrence count, used for anagrams, majority, and duplicate detection.
- **Complement lookup:** Finding `target - x` (or another derived key) in a map while scanning once — the two-sum pattern.
- **Canonical key:** A normalized representation (sorted string or count signature) so equivalents (anagrams) share one bucket.
- **Prefix-sum + map:** Storing prefix sums as keys so subarray-sum problems become O(n) complement lookups.
- **Collision / worst case:** Multiple keys hashing to one bucket; average O(1) can degrade unless the table handles it (treeified bins, etc.).

## Concept

Store **key → count/index/list**. Trade space for time. Average O(1) get/put; worst case degrades (mention briefly, don’t obsess).

## Worked example 1 — Two sum (unsorted)

```java
public int[] twoSum(int[] nums, int target) {
  Map<Integer, Integer> idx = new HashMap<>();
  for (int i = 0; i < nums.length; i++) {
    Integer j = idx.get(target - nums[i]);
    if (j != null) return new int[]{j, i};
    idx.put(nums[i], i);
  }
  return new int[]{-1, -1};
}
```

```csharp
public int[] TwoSum(int[] nums, int target) {
  var idx = new Dictionary<int, int>();
  for (int i = 0; i < nums.Length; i++) {
    int need = target - nums[i];
    if (idx.TryGetValue(need, out int j)) return new[] { j, i };
    idx[nums[i]] = i;
  }
  return new[] { -1, -1 };
}
```

## Worked example 2 — Group anagrams

```java
public List<List<String>> groupAnagrams(String[] strs) {
  Map<String, List<String>> g = new HashMap<>();
  for (String s : strs) {
    char[] a = s.toCharArray();
    Arrays.sort(a);
    String key = new String(a);
    g.computeIfAbsent(key, k -> new ArrayList<>()).add(s);
  }
  return new ArrayList<>(g.values());
}
```

```csharp
public IList<IList<string>> GroupAnagrams(string[] strs) {
  var g = new Dictionary<string, IList<string>>();
  foreach (var s in strs) {
    char[] a = s.ToCharArray();
    Array.Sort(a);
    string key = new string(a);
    if (!g.ContainsKey(key)) g[key] = new List<string>();
    g[key].Add(s);
  }
  return g.Values.ToList();
}
```

**Alt key:** count array of 26 letters serialized — O(n·L) without sorting each string.

## Worked example 3 — Subarray sum equals K (prefix + map)

```java
public int subarraySum(int[] nums, int k) {
  Map<Integer, Integer> freq = new HashMap<>();
  freq.put(0, 1);
  int sum = 0, ans = 0;
  for (int x : nums) {
    sum += x;
    ans += freq.getOrDefault(sum - k, 0);
    freq.merge(sum, 1, Integer::sum);
  }
  return ans;
}
```

```csharp
public int SubarraySum(int[] nums, int k) {
  var freq = new Dictionary<int, int> { [0] = 1 };
  int sum = 0, ans = 0;
  foreach (int x in nums) {
    sum += x;
    if (freq.TryGetValue(sum - k, out int c)) ans += c;
    freq[sum] = freq.GetValueOrDefault(sum) + 1;
  }
  return ans;
}
```

## Interview Q&A

- **Q:** Hashing vs sort for uniqueness?
  **A:** Need any order / existence → hash. Need ordered output → TreeMap/sort.
- **Q:** Mutable keys?
  **A:** Never; hashCode must be stable while in the map.
- **Q:** Collision / adversarial input?
  **A:** Java HashMap treeifies bins; still design for average case unless asked.

## Pitfalls

- Using `==` instead of value equality for boxed types in some languages  
- Forgetting `freq.put(0,1)` on prefix problems  
- Storing only last index when problem needs counts

## 60-second answer

“I use a hash map for complements, frequencies, and prefix sums. Average O(1) updates give O(n) solutions where nested loops would be O(n²). I call out the O(n) space trade-off.”

## Further study

- [Hash table (Wikipedia)](https://en.wikipedia.org/wiki/Hash_table) — expected O(1) lookups underlying interview hashing
- [Hash function (Wikipedia)](https://en.wikipedia.org/wiki/Hash_function) — keys, collisions, and bucket behavior
- [HashMap (Java SE)](https://docs.oracle.com/en/java/javase/17/docs/api/java.base/java/util/HashMap.html) — Java map semantics for interviews
- [Dictionary (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2) — C# hash map API

## Practice prompts

1. Top K frequent elements  
2. Longest consecutive sequence (set)  
3. Copy list with random pointer (map old→new)
