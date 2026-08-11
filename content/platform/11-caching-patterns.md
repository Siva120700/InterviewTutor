---
id: platform-caching-patterns
title: Caching Patterns and Invalidation
track: platform
module: "02 Caching"
order: 11
languages: [java, csharp]
summary: Cache-aside, stampede control, invalidation, and consistency patterns for interviews.
---

## Why this matters

Interviews dig into invalidation and stampedes. Patterns show senior judgment.

## Definitions

- **Cache-aside (lazy loading):** App reads the cache first; on miss it loads the DB, then populates the cache.
- **Write-through / write-behind:** Writes update the cache on the path to storage (sync) or asynchronously (behind)—different durability trade-offs.
- **Invalidation:** Deleting or updating cache entries when the source data changes so readers don’t keep serving wrong data.
- **Soft TTL / stale-while-revalidate:** Serve slightly stale data while refreshing in the background to hide origin latency.
- **Singleflight (request coalescing):** One miss rebuilds a key while concurrent waiters share the result—stops stampedes.
- **Negative caching:** Briefly cache “not found” / empty results so repeated misses don’t hammer the origin.
- **TTL jitter:** Randomize expiry slightly so many keys don’t expire in the same second.


## Cache-aside

```java
String getUser(String id) {
  String key = "user:" + id;
  String cached = redis.get(key);
  if (cached != null) return cached;
  String val = db.find(id);
  if (val != null) redis.setEx(key, 300, val);
  return val;
}

void updateUser(String id, String val) {
  db.save(id, val);
  redis.del("user:" + id); // invalidate
}
```

```csharp
async Task<string?> GetUserAsync(string id) {
  var key = $"user:{id}";
  var cached = await cache.GetStringAsync(key);
  if (cached is not null) return cached;
  var val = await db.FindAsync(id);
  if (val is not null) await cache.SetStringAsync(key, val, TimeSpan.FromMinutes(5));
  return val;
}
```

## Stampede controls

- Lock around rebuild (`SET key:lock NX EX`)  
- Probabilistic early expire  
- Soft TTL + background refresh

## Consistency models (say out loud)

- **Eventual:** stale windows OK  
- **Read-your-writes:** invalidate or bypass cache after write  
- **Strong:** often skip cache or use transactional messaging carefully

## Interview Q&A

- **Q:** Delete vs update cache on write?
  **A:** Delete (invalidate) is safer; next read rebuilds correct value.
- **Q:** Hot key?
  **A:** Local near-cache, split keys, or edge cache.
- **Q:** Multi-key invalidation?
  **A:** Version prefixes (`user:v12:…`) or tag-based purge.

## Pitfalls

- Fire-and-forget invalidation without monitoring  
- Caching authenticated HTML at CDN  
- Unbounded key growth

## 60-second answer

“Cache-aside + delete-on-write is my default. I add TTL as a backstop, coalesce misses for hot keys, and choose consistency based on how wrong stale data can be.”

## Further study

- [Redis: Client-side caching](https://redis.io/docs/latest/develop/reference/client-side-caching/) — tracking and invalidation beyond simple TTLs.
- [Redis: Key eviction](https://redis.io/docs/latest/develop/reference/eviction/) — what happens when memory is full.
- [MDN: Cache-Control](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Cache-Control) — HTTP freshness directives related to soft TTL.
- [AWS Caching Best Practices](https://aws.amazon.com/caching/best-practices/) — invalidation and consistency guidance.

## Practice prompts

1. Invalidate related keys after profile update  
2. Design soft TTL for news feed  
3. Prevent stampede on flash-sale SKU
