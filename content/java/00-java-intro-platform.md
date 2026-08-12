---
id: java-intro-platform
title: Java Intro — Platform, JDK, First Program
track: java
module: "00 Language Fundamentals"
order: 0
languages: [java]
summary: What Java is, JVM/JRE/JDK, key features, and your first runnable program.
---

## Why this matters

Interviews assume you know *where* Java code runs (JVM) and how the toolchain splits (JDK vs JRE). Fuzzy answers here undermine everything else.

## Definitions

- **Java:** A statically typed, object-oriented language that compiles to bytecode for the JVM.
- **JVM (Java Virtual Machine):** Runtime that executes bytecode — enables “write once, run anywhere.”
- **JRE (Java Runtime Environment):** JVM + core libraries to *run* apps (no compilers/tools).
- **JDK (Java Development Kit):** JRE + compiler (`javac`), debugger, and tooling to *build* apps.
- **Bytecode:** Platform-neutral `.class` instructions produced by `javac`.
- **JIT:** HotSpot compiles hot bytecode methods to native code at runtime.

## Features (interview short list)

| Feature | Meaning |
|---------|---------|
| Object-oriented | Classes/interfaces, encapsulation |
| Portable | Bytecode + JVM per OS |
| Memory-managed | GC reclaims unreachable objects |
| Strong typing | Types checked at compile time |
| Multithreaded | `java.util.concurrent` / threads |
| Rich standard library | Collections, IO, concurrency |

## Java vs C++ (practical)

| | Java | C++ |
|---|------|-----|
| Memory | GC | Manual / RAII |
| Multiple inheritance | Interfaces (and default methods) | Classes |
| Pointers | References only | Raw/smart pointers |
| Compilation | To bytecode | To native |
| Unsigned ints | Limited (`int` signed) | Full unsigned set |

## First program

```java
public class Hello {
  public static void main(String[] args) {
    System.out.println("Hello, InterviewTutor");
  }
}
```

```bash
javac Hello.java
java Hello
```

- `public class` name must match file name `Hello.java`  
- `main` signature is the process entry point  
- `String[] args` are CLI arguments  

## Comments and statements

```java
// line comment
/* block comment */
/** javadoc — used on public APIs */

int x = 1; // statement ends with ;
```

## Interview Q&A

- **Q:** JDK vs JRE on a prod server?  
  **A:** Runtime needs JRE (or modern JDK distribution); build agents need JDK.
- **Q:** Is Java interpreted?  
  **A:** Bytecode interpreted/JITed — not source-interpreted like classic scripting.
- **Q:** Why `public static void main`?  
  **A:** JVM looks for that exact entry signature to start the process.

## Pitfalls

- Installing JRE only then wondering why `javac` is missing  
- Wrong class name vs file name  
- Calling `java Hello.class` instead of `java Hello`

## 60-second answer

“Java compiles to bytecode run by the JVM. I develop with the JDK, run with a JRE/JVM. Portability and GC are the headline features versus C++.”

## Further study

- JVM & GC lesson (deeper)  
- Variables / types / operators next  

## Practice prompts

1. Explain JDK vs JRE vs JVM to a beginner  
2. Compile and run a class with a package declaration  
3. Print CLI args one per line
