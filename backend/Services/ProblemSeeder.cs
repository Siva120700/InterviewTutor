using InterviewTutor.Api.Data;
using InterviewTutor.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTutor.Api.Services;

public static class ProblemSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Problems.AnyAsync())
            return;

        db.Problems.AddRange(
            P("two-sum-sorted", "Two Sum in Sorted Array", "Easy", "dsa", 1,
                "Given a **sorted** array of integers and a target, return indices of two numbers that add up to target (1-based).",
                """
                public int[] twoSum(int[] nums, int target) {
                  int l = 0, r = nums.length - 1;
                  while (l < r) {
                    int s = nums[l] + nums[r];
                    if (s == target) return new int[]{l + 1, r + 1};
                    if (s < target) l++; else r--;
                  }
                  return new int[]{-1, -1};
                }
                """,
                """
                public int[] TwoSum(int[] nums, int target) {
                  int l = 0, r = nums.Length - 1;
                  while (l < r) {
                    int s = nums[l] + nums[r];
                    if (s == target) return new[] { l + 1, r + 1 };
                    if (s < target) l++; else r--;
                  }
                  return new[] { -1, -1 };
                }
                """,
                "O(n) time, O(1) space — two pointers on sorted input."),
            P("longest-unique-substring", "Longest Substring Without Repeating", "Medium", "dsa", 2,
                "Find the length of the longest substring without repeating characters.",
                """
                public int lengthOfLongestSubstring(String s) {
                  Map<Character, Integer> last = new HashMap<>();
                  int best = 0, left = 0;
                  for (int i = 0; i < s.length(); i++) {
                    char c = s.charAt(i);
                    if (last.containsKey(c)) left = Math.max(left, last.get(c) + 1);
                    last.put(c, i);
                    best = Math.max(best, i - left + 1);
                  }
                  return best;
                }
                """,
                """
                public int LengthOfLongestSubstring(string s) {
                  var last = new Dictionary<char, int>();
                  int best = 0, left = 0;
                  for (int i = 0; i < s.Length; i++) {
                    char c = s[i];
                    if (last.TryGetValue(c, out var prev)) left = Math.Max(left, prev + 1);
                    last[c] = i;
                    best = Math.Max(best, i - left + 1);
                  }
                  return best;
                }
                """,
                "O(n) sliding window with last-seen index map."),
            P("level-order", "Binary Tree Level Order", "Medium", "dsa", 3,
                "Return the level-order traversal of a binary tree as a list of levels.",
                """
                public List<List<Integer>> levelOrder(TreeNode root) {
                  List<List<Integer>> res = new ArrayList<>();
                  if (root == null) return res;
                  Queue<TreeNode> q = new ArrayDeque<>();
                  q.add(root);
                  while (!q.isEmpty()) {
                    int n = q.size();
                    List<Integer> level = new ArrayList<>();
                    for (int i = 0; i < n; i++) {
                      TreeNode cur = q.poll();
                      level.add(cur.val);
                      if (cur.left != null) q.add(cur.left);
                      if (cur.right != null) q.add(cur.right);
                    }
                    res.add(level);
                  }
                  return res;
                }
                """,
                """
                public IList<IList<int>> LevelOrder(TreeNode root) {
                  var res = new List<IList<int>>();
                  if (root is null) return res;
                  var q = new Queue<TreeNode>();
                  q.Enqueue(root);
                  while (q.Count > 0) {
                    int n = q.Count;
                    var level = new List<int>();
                    for (int i = 0; i < n; i++) {
                      var cur = q.Dequeue();
                      level.Add(cur.val);
                      if (cur.left is not null) q.Enqueue(cur.left);
                      if (cur.right is not null) q.Enqueue(cur.right);
                    }
                    res.Add(level);
                  }
                  return res;
                }
                """,
                "BFS O(n) time / O(w) space."),
            P("lru-cache", "LRU Cache Sketch", "Medium", "lld", 4,
                "Design an LRU cache with `get` and `put` in average O(1).",
                """
                // HashMap + doubly linked list (sketch)
                // map: key -> node; list: most-recent at head
                """,
                """
                // Dictionary + LinkedList (sketch)
                // Move node to front on get/put; evict from back when over capacity
                """,
                "O(1) average with hashmap + doubly linked list."),
            P("rate-limiter", "Token Bucket Rate Limiter", "Medium", "hld", 5,
                "Implement a token-bucket rate limiter: allow N requests per window for a key.",
                """
                class TokenBucket {
                  final long capacity, refillPerSec;
                  long tokens; long lastRefillNanos;
                  synchronized boolean allow() {
                    refill();
                    if (tokens == 0) return false;
                    tokens--; return true;
                  }
                  void refill() { /* add tokens based on elapsed time */ }
                }
                """,
                """
                class TokenBucket {
                  readonly long capacity, refillPerSec;
                  long tokens; long lastRefillTicks;
                  public bool Allow() { Refill(); if (tokens == 0) return false; tokens--; return true; }
                  void Refill() { /* add tokens based on elapsed time */ }
                }
                """,
                "Trade-offs vs fixed window / sliding window logs."),
            P("isolation-drill", "Transaction Isolation Drill", "Easy", "cs-databases", 6,
                "Describe a scenario where READ COMMITTED allows a non-repeatable read, and how REPEATABLE READ / snapshot isolation changes it.",
                "// Conceptual — no code required",
                "// Conceptual — no code required",
                "Focus on phenomena: dirty, non-repeatable, phantom."),
            P("async-deadlock", "Async Deadlock Trap", "Medium", "dotnet", 7,
                "Explain why `.Result` / `.Wait()` on async ASP.NET code can deadlock, and the fix.",
                "// Java analogy: blocking on CompletableFuture on a constrained pool",
                """
                // Bad: var x = GetAsync().Result;
                // Good: var x = await GetAsync();
                """,
                "Sync-over-async + single-threaded sync context."),
            P("spring-transaction", "Spring @Transactional Boundaries", "Medium", "java", 8,
                "When does `@Transactional` not start a transaction (self-invocation, private methods, unchecked vs checked)?",
                "// Proxy-based AOP — internal calls bypass the proxy",
                "// N/A",
                "Proxy boundaries and exception rollback rules."),
            P("tcp-vs-udp", "TCP vs UDP Trade-offs", "Easy", "cs-networking", 9,
                "When would you choose UDP for a product feature, and what reliability would you add at the app layer?",
                "// Conceptual",
                "// Conceptual",
                "Latency, head-of-line blocking, NAT, retransmission."),
            P("observability-sli", "Define SLIs for an API", "Easy", "senior-fs", 10,
                "Propose 3 SLIs and rough SLOs for a public REST API serving 5k RPS.",
                "// Conceptual",
                "// Conceptual",
                "Latency, availability, error rate — avoid vanity metrics."),
            P("min-heap-k", "K Largest Elements", "Easy", "dsa", 11,
                "Return the k largest elements from an unsorted array.",
                """
                public int[] kLargest(int[] a, int k) {
                  PriorityQueue<Integer> pq = new PriorityQueue<>();
                  for (int x : a) {
                    pq.offer(x);
                    if (pq.size() > k) pq.poll();
                  }
                  return pq.stream().mapToInt(i -> i).toArray();
                }
                """,
                """
                public int[] KLargest(int[] a, int k) {
                  var pq = new PriorityQueue<int>();
                  foreach (var x in a) {
                    pq.Enqueue(x, x);
                    if (pq.Count > k) pq.Dequeue();
                  }
                  return pq.UnorderedItems.Select(i => i.Element).ToArray();
                }
                """,
                "Min-heap of size k — O(n log k)."),
            P("cache-aside", "Cache-Aside Pattern", "Easy", "hld", 12,
                "Describe cache-aside read/write flows and how you handle stampedes.",
                "// Conceptual + optional pseudo",
                "// Conceptual + optional pseudo",
                "TTL, soft TTL, singleflight / locking.")
        );

        await db.SaveChangesAsync();
    }

    private static Problem P(
        string slug, string title, string difficulty, string track, int order,
        string prompt, string java, string csharp, string notes) =>
        new()
        {
            Slug = slug,
            Title = title,
            Difficulty = difficulty,
            TrackSlug = track,
            Order = order,
            PromptMarkdown = prompt,
            JavaSolution = java,
            CsharpSolution = csharp,
            ComplexityNotes = notes
        };
}
