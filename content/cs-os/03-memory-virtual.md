---
id: os-memory-virtual
title: Memory and Virtual Memory
track: cs-os
module: "02 Memory"
order: 3
languages: [java, csharp]
summary: Stack vs heap, paging, thrashing, and what it means for JVM/.NET services.
---

## Why this matters

OOM kills, GC pressure, and thrashing connect OS memory to app performance.

## Definitions

- **Stack:** Per-thread memory for call frames/locals; allocated/freed automatically as functions enter/return.
- **Heap:** Dynamically allocated object memory shared in a process; GC-managed in Java/.NET.
- **Virtual memory:** Illusion that each process has its own address space, backed by page mappings to RAM or disk.
- **Page:** Fixed-size block of virtual memory the OS maps to a physical frame or swap.
- **Page fault:** Trap when a needed page isn’t mapped in RAM — OS must load/allocate it.
- **Working set:** Pages a process actively uses over a window; if it exceeds RAM, latency collapses.
- **Thrashing:** System spends most time paging because working sets don’t fit in RAM.
- **OOM (out of memory):** Allocation cannot be satisfied; containers often kill the process (`OOMKilled`).

## Stack vs heap

- Stack: frames, automatic, per-thread  
- Heap: dynamic objects, GC-managed in Java/.NET

## Virtual memory

Processes see virtual addresses → pages mapped to physical RAM or disk (swap). Page fault on miss.

## Thrashing

Working set > RAM → constant paging → collapse. Fix: more RAM, smaller working set, fix memory leaks.

## Interview Q&A

- **Q:** Why per-thread stacks matter?
  **A:** Thousands of threads ⇒ GBs of stack reserve; prefer pooled async.
- **Q:** Container limits?
  **A:** cgroup memory caps; JVM needs flags aware of container (`-XX:+UseContainerSupport`).

## 60-second answer

“Virtual memory maps pages to RAM/disk. Apps allocate heap objects; stacks are per-thread. When working sets exceed RAM, thrashing destroys latency — size heaps and thread counts deliberately.”

## Further study

- [Virtual memory (Wikipedia)](https://en.wikipedia.org/wiki/Virtual_memory) — address spaces, paging, and the illusion of private memory
- [Paging (Wikipedia)](https://en.wikipedia.org/wiki/Paging) — pages, frames, and page faults
- [Java GC tuning guide](https://docs.oracle.com/en/java/javase/21/gctuning/) — how JVM heaps interact with OS memory pressure
- [.NET garbage collection](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/) — managed heap generations and LOH concepts

## Practice prompts

1. Diagnose pod OOMKilled  
2. Contrast GC heap vs native memory  
3. Explain copy-on-write fork at high level
