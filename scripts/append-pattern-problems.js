const fs = require("fs");
const path = require("path");

const dir = path.join(__dirname, "..", "content", "dsa-patterns");

const blocks = {
  "03-fast-slow-pointers.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Linked List Cycle](https://leetcode.com/problems/linked-list-cycle/) | Easy |
| 2 | [Middle of the Linked List](https://leetcode.com/problems/middle-of-the-linked-list/) | Easy |
| 3 | [Happy Number](https://leetcode.com/problems/happy-number/) | Easy |
| 4 | [Find the Duplicate Number](https://leetcode.com/problems/find-the-duplicate-number/) | Medium |
| 5 | [Palindrome Linked List](https://leetcode.com/problems/palindrome-linked-list/) | Easy |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Linked List Cycle / Duplicate Number
- [Blind 75 playlist](https://www.youtube.com/playlist?list=PLot-Xpze53ldVwtstag2TL4HQhAnC8ATf)
- [Striver Linked List](https://www.youtube.com/@takeUforward/playlists)`,
  },
  "04-merge-intervals.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Merge Intervals](https://leetcode.com/problems/merge-intervals/) | Medium |
| 2 | [Insert Interval](https://leetcode.com/problems/insert-interval/) | Medium |
| 3 | [Non-overlapping Intervals](https://leetcode.com/problems/non-overlapping-intervals/) | Medium |
| 4 | [Minimum Number of Arrows to Burst Balloons](https://leetcode.com/problems/minimum-number-of-arrows-to-burst-balloons/) | Medium |
| 5 | [Meeting Rooms II](https://leetcode.com/problems/meeting-rooms-ii/) *(Premium)* / practice Mentions | Medium |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Merge / Insert Interval
- [Blind 75 playlist](https://www.youtube.com/playlist?list=PLot-Xpze53ldVwtstag2TL4HQhAnC8ATf)
- [Striver / takeUforward](https://www.youtube.com/@takeUforward)`,
  },
  "05-cyclic-sort.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Missing Number](https://leetcode.com/problems/missing-number/) | Easy |
| 2 | [Find All Numbers Disappeared in an Array](https://leetcode.com/problems/find-all-numbers-disappeared-in-an-array/) | Easy |
| 3 | [Find the Duplicate Number](https://leetcode.com/problems/find-the-duplicate-number/) | Medium |
| 4 | [Set Mismatch](https://leetcode.com/problems/set-mismatch/) | Easy |
| 5 | [First Missing Positive](https://leetcode.com/problems/first-missing-positive/) | Hard |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Missing Number / First Missing Positive
- Search “cyclic sort pattern” on YouTube after attempting
- [Striver Arrays](https://www.youtube.com/@takeUforward)`,
  },
  "06-linkedlist-reversal.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Reverse Linked List](https://leetcode.com/problems/reverse-linked-list/) | Easy |
| 2 | [Reverse Linked List II](https://leetcode.com/problems/reverse-linked-list-ii/) | Medium |
| 3 | [Swap Nodes in Pairs](https://leetcode.com/problems/swap-nodes-in-pairs/) | Medium |
| 4 | [Reorder List](https://leetcode.com/problems/reorder-list/) | Medium |
| 5 | [Reverse Nodes in k-Group](https://leetcode.com/problems/reverse-nodes-in-k-group/) | Hard |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Reverse Linked List / k-Group
- [Striver Linked List playlist](https://www.youtube.com/@takeUforward/playlists)
- [Blind 75 playlist](https://www.youtube.com/playlist?list=PLot-Xpze53ldVwtstag2TL4HQhAnC8ATf)`,
  },
  "10-tree-bfs.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Binary Tree Level Order Traversal](https://leetcode.com/problems/binary-tree-level-order-traversal/) | Medium |
| 2 | [Binary Tree Zigzag Level Order Traversal](https://leetcode.com/problems/binary-tree-zigzag-level-order-traversal/) | Medium |
| 3 | [Binary Tree Right Side View](https://leetcode.com/problems/binary-tree-right-side-view/) | Medium |
| 4 | [Minimum Depth of Binary Tree](https://leetcode.com/problems/minimum-depth-of-binary-tree/) | Easy |
| 5 | [Word Ladder](https://leetcode.com/problems/word-ladder/) | Hard |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Tree BFS / Level Order
- [Striver Binary Trees](https://www.youtube.com/@takeUforward/playlists)
- [NeetCode Graphs](https://www.youtube.com/playlist?list=PLot-Xpze53ldBT_7QA8NVot219jFNr_GI) (for Word Ladder)`,
  },
  "11-tree-dfs.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Maximum Depth of Binary Tree](https://leetcode.com/problems/maximum-depth-of-binary-tree/) | Easy |
| 2 | [Path Sum](https://leetcode.com/problems/path-sum/) | Easy |
| 3 | [Diameter of Binary Tree](https://leetcode.com/problems/diameter-of-binary-tree/) | Easy |
| 4 | [Validate Binary Search Tree](https://leetcode.com/problems/validate-binary-search-tree/) | Medium |
| 5 | [Lowest Common Ancestor of a Binary Tree](https://leetcode.com/problems/lowest-common-ancestor-of-a-binary-tree/) | Medium |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Trees / DFS
- [Striver Binary Trees + BST](https://www.youtube.com/@takeUforward/playlists)
- [Blind 75 playlist](https://www.youtube.com/playlist?list=PLot-Xpze53ldVwtstag2TL4HQhAnC8ATf)`,
  },
  "12-island-matrix.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Number of Islands](https://leetcode.com/problems/number-of-islands/) | Medium |
| 2 | [Max Area of Island](https://leetcode.com/problems/max-area-of-island/) | Medium |
| 3 | [Flood Fill](https://leetcode.com/problems/flood-fill/) | Easy |
| 4 | [Rotting Oranges](https://leetcode.com/problems/rotting-oranges/) | Medium |
| 5 | [Surrounded Regions](https://leetcode.com/problems/surrounded-regions/) | Medium |`,
    yt: `- [NeetCode Graphs playlist](https://www.youtube.com/playlist?list=PLot-Xpze53ldBT_7QA8NVot219jFNr_GI)
- [Striver Graph series](https://www.youtube.com/@takeUforward/playlists)
- Search “Number of Islands NeetCode”`,
  },
  "13-topological-sort.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Course Schedule](https://leetcode.com/problems/course-schedule/) | Medium |
| 2 | [Course Schedule II](https://leetcode.com/problems/course-schedule-ii/) | Medium |
| 3 | [Find Eventual Safe States](https://leetcode.com/problems/find-eventual-safe-states/) | Medium |
| 4 | [Alien Dictionary](https://leetcode.com/problems/alien-dictionary/) *(Premium)* | Hard |`,
    yt: `- [NeetCode Graphs](https://www.youtube.com/playlist?list=PLot-Xpze53ldBT_7QA8NVot219jFNr_GI) — Course Schedule
- [Striver Graph — Topo Sort](https://www.youtube.com/@takeUforward/playlists)`,
  },
  "14-union-find.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Number of Provinces](https://leetcode.com/problems/number-of-provinces/) | Medium |
| 2 | [Redundant Connection](https://leetcode.com/problems/redundant-connection/) | Medium |
| 3 | [Graph Valid Tree](https://leetcode.com/problems/graph-valid-tree/) *(Premium)* | Medium |
| 4 | [Accounts Merge](https://leetcode.com/problems/accounts-merge/) | Medium |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Union Find / Redundant Connection
- [Striver DSU](https://www.youtube.com/@takeUforward)`,
  },
  "20-two-heaps.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Find Median from Data Stream](https://leetcode.com/problems/find-median-from-data-stream/) | Hard |
| 2 | [Sliding Window Median](https://leetcode.com/problems/sliding-window-median/) | Hard |
| 3 | [IPO](https://leetcode.com/problems/ipo/) | Hard |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Find Median from Data Stream
- Search “two heaps pattern” after attempting`,
  },
  "21-top-k.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Kth Largest Element in an Array](https://leetcode.com/problems/kth-largest-element-in-an-array/) | Medium |
| 2 | [Top K Frequent Elements](https://leetcode.com/problems/top-k-frequent-elements/) | Medium |
| 3 | [K Closest Points to Origin](https://leetcode.com/problems/k-closest-points-to-origin/) | Medium |
| 4 | [Ugly Number II](https://leetcode.com/problems/ugly-number-ii/) | Medium |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Top K / Heap
- [Striver Heap](https://www.youtube.com/@takeUforward/playlists)`,
  },
  "22-k-way-merge.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Merge Two Sorted Lists](https://leetcode.com/problems/merge-two-sorted-lists/) | Easy |
| 2 | [Merge k Sorted Lists](https://leetcode.com/problems/merge-k-sorted-lists/) | Hard |
| 3 | [Find K Pairs with Smallest Sums](https://leetcode.com/problems/find-k-pairs-with-smallest-sums/) | Medium |
| 4 | [Smallest Range Covering Elements from K Lists](https://leetcode.com/problems/smallest-range-covering-elements-from-k-lists/) | Hard |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Merge k Sorted Lists
- [Striver](https://www.youtube.com/@takeUforward) — Heap / Linked List`,
  },
  "30-subsets.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Subsets](https://leetcode.com/problems/subsets/) | Medium |
| 2 | [Subsets II](https://leetcode.com/problems/subsets-ii/) | Medium |
| 3 | [Permutations](https://leetcode.com/problems/permutations/) | Medium |
| 4 | [Combinations](https://leetcode.com/problems/combinations/) | Medium |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Subsets / Permutations
- [Striver Recursion playlist](https://www.youtube.com/@takeUforward/playlists)`,
  },
  "31-backtracking.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Combination Sum](https://leetcode.com/problems/combination-sum/) | Medium |
| 2 | [Generate Parentheses](https://leetcode.com/problems/generate-parentheses/) | Medium |
| 3 | [Word Search](https://leetcode.com/problems/word-search/) | Medium |
| 4 | [Palindrome Partitioning](https://leetcode.com/problems/palindrome-partitioning/) | Medium |
| 5 | [N-Queens](https://leetcode.com/problems/n-queens/) | Hard |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Backtracking playlist/search
- [Striver Recursion & Backtracking](https://www.youtube.com/@takeUforward/playlists)`,
  },
  "32-modified-binary-search.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Binary Search](https://leetcode.com/problems/binary-search/) | Easy |
| 2 | [Search in Rotated Sorted Array](https://leetcode.com/problems/search-in-rotated-sorted-array/) | Medium |
| 3 | [Find Peak Element](https://leetcode.com/problems/find-peak-element/) | Medium |
| 4 | [Koko Eating Bananas](https://leetcode.com/problems/koko-eating-bananas/) | Medium |
| 5 | [Capacity To Ship Packages Within D Days](https://leetcode.com/problems/capacity-to-ship-packages-within-d-days/) | Medium |`,
    yt: `- [NeetCode Binary Search playlist](https://www.youtube.com/playlist?list=PLot-Xpze53leNZQd0iINpD-MAhMOMzWvO) — **best**
- [Striver Binary Search](https://www.youtube.com/@takeUforward/playlists) — deep (1D / answer space)`,
  },
  "33-monotonic-stack.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Daily Temperatures](https://leetcode.com/problems/daily-temperatures/) | Medium |
| 2 | [Next Greater Element I](https://leetcode.com/problems/next-greater-element-i/) | Easy |
| 3 | [Largest Rectangle in Histogram](https://leetcode.com/problems/largest-rectangle-in-histogram/) | Hard |
| 4 | [Trapping Rain Water](https://leetcode.com/problems/trapping-rain-water/) | Hard |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Daily Temperatures / Histogram
- [Blind 75 playlist](https://www.youtube.com/playlist?list=PLot-Xpze53ldVwtstag2TL4HQhAnC8ATf)`,
  },
  "40-knapsack-dp.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Climbing Stairs](https://leetcode.com/problems/climbing-stairs/) | Easy |
| 2 | [House Robber](https://leetcode.com/problems/house-robber/) | Medium |
| 3 | [Coin Change](https://leetcode.com/problems/coin-change/) | Medium |
| 4 | [Partition Equal Subset Sum](https://leetcode.com/problems/partition-equal-subset-sum/) | Medium |
| 5 | [Target Sum](https://leetcode.com/problems/target-sum/) | Medium |`,
    yt: `- [Aditya Verma DP playlist](https://www.youtube.com/playlist?list=PL_z_8CaSLPWekqhdCPmFSrxB2olrIhfDt) — **best for knapsack intuition**
- [Striver DP](https://www.youtube.com/@takeUforward/playlists)
- [NeetCode](https://www.youtube.com/@NeetCode) — 1D/2D DP`,
  },
  "41-bitwise-xor.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Single Number](https://leetcode.com/problems/single-number/) | Easy |
| 2 | [Missing Number](https://leetcode.com/problems/missing-number/) | Easy |
| 3 | [Single Number II](https://leetcode.com/problems/single-number-ii/) | Medium |
| 4 | [Single Number III](https://leetcode.com/problems/single-number-iii/) | Medium |`,
    yt: `- [NeetCode](https://www.youtube.com/@NeetCode) — Single Number / bits
- [Striver Bit Manipulation](https://www.youtube.com/@takeUforward)`,
  },
  "42-prefix-sum-hashing.md": {
    problems: `| # | Problem | Level |
|---|---------|-------|
| 1 | [Subarray Sum Equals K](https://leetcode.com/problems/subarray-sum-equals-k/) | Medium |
| 2 | [Contiguous Array](https://leetcode.com/problems/contiguous-array/) | Medium |
| 3 | [Continuous Subarray Sum](https://leetcode.com/problems/continuous-subarray-sum/) | Medium |
| 4 | [Product of Array Except Self](https://leetcode.com/problems/product-of-array-except-self/) | Medium |`,
    yt: `- [NeetCode Arrays & Hashing](https://www.youtube.com/playlist?list=PLALUz6Z8Un2ew_yN3UAce8bOA25P5kaUl)
- [NeetCode](https://www.youtube.com/@NeetCode) — Subarray Sum Equals K
- [Striver](https://www.youtube.com/@takeUforward)`,
  },
};

function inject(file, block) {
  const p = path.join(dir, file);
  let text = fs.readFileSync(p, "utf8");
  if (text.includes("## Pattern-wise problems")) {
    console.log("skip", file);
    return;
  }
  const section = `
## Pattern-wise problems (solve in order)

${block.problems}

## YouTube (watch after attempting)

${block.yt}

Master index: **Pattern-Wise Problems + Best YouTube Playlists** (Start Here module).
`;
  if (text.includes("## Further study")) {
    text = text.replace("## Further study", section + "\n## Further study");
  } else if (text.includes("## Practice prompts")) {
    text = text.replace("## Practice prompts", section + "\n## Practice prompts");
  } else {
    text = text.trimEnd() + "\n" + section + "\n";
  }
  fs.writeFileSync(p, text);
  console.log("updated", file);
}

for (const [file, block] of Object.entries(blocks)) inject(file, block);
