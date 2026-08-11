InterviewTutor (Separate Repo)

Repo location





Path: D:/Project/InterviewTutor



Name: InterviewTutor



Relation to StockYouNeed: none — fully separate project (folder already exists, currently empty / not a git repo yet)



Workspace for implementation: open/use D:/Project/InterviewTutor, not the StockYouNeed workspace

Verdict

Yes — buildable as a new standalone product. Default MVP: curated learning tracks + AI tutor + a small practice set (not a full LeetCode clone on day one).

Product goal

Help senior full-stack engineers prepare for interviews with:

Core interview tracks





DSA — patterns, complexity, worked examples (Java + C# side-by-side where useful)



LLD — OOP, design patterns, class design with concrete examples



HLD — system design with diagrams, trade-offs, capacity thinking



Senior FS — APIs, caching, concurrency, observability, behavioral / leadership

Computer science fundamentals





Databases — relational vs NoSQL, indexing, transactions/ACID, isolation levels, query plans, normalization, sharding/replication (interview depth)



Networking — OSI/TCP-IP, HTTP/HTTPS, DNS, TLS, load balancers, CDN, websockets, common debugging



Operating systems — processes/threads, scheduling, memory, virtual memory, locks/deadlocks, I/O, containers at a practical level



Other CS — concurrency basics, CAP theorem, consistency models, security basics (authn/authz, OWASP top risks)

Language / platform tracks





Java — language core → collections → concurrency → JVM → Spring Boot interview topics, with runnable-style examples in lessons



.NET / C# — language core → BCL → async/await → GC → ASP.NET Core / EF Core interview topics, with examples

Assumed MVP

Thin but useful MVP:





Topic library covering all tracks above (seed depth varies; see curriculum)



Lesson pages with clear explanations + examples (Java and/or C# code blocks)



Doubt-clearing AI chat on every lesson — ask free-form questions, get full step-by-step explanations (plus Explain / Quiz / Mock modes)



Progress tracking (completed lessons, weak areas)



Small practice set (10–20 problems) with Java + C# solution walkthroughs — no judge sandbox in v1



Chat history saved per lesson so you can continue a doubt thread later



Add-from-chat: if you say “include this topic in lessons” (or similar), the app drafts a lesson, you confirm, and it appears in the catalog

Defer to later: full online judge, company-tagged banks, video, multiplayer mocks, deep certification-style courses.

Recommended stack (new repo)

Reuse skills you already have, keep shipping fast:





Frontend: React + Vite + TypeScript



Backend: .NET 8 Web API + PostgreSQL



Auth: Google OAuth for v1



AI: OpenAI-compatible API with structured prompts + lesson context



Content: Seed lessons as Markdown in repo; user-requested lessons stored in Postgres (so chat can add them at runtime)



Code display: syntax highlighting (e.g. Shiki or Prism) in the lesson reader

flowchart LR
  User[User] --> Web[React_Vite_App]
  Web --> Api[DotNet_API]
  Api --> Db[(Postgres)]
  Api --> Ai[LLM_Provider]
  Api --> Content[Markdown_Seed_Lessons]
  Db --> Dynamic[User_Added_Lessons]

Information architecture





/ — landing (value prop + CTA)



/tracks — grouped catalog:





Interview: DSA, LLD, HLD, Senior FS



CS Basics: Databases, Networking, OS, Security & Distributed basics



Languages: Java, .NET



/tracks/:slug/lessons/:id — lesson + examples + AI doubt chat side panel



/practice — curated problems + Java/C# solutions (+ chat for problem doubts)



/mock — timed AI mock (HLD, CS fundamentals, or behavioral)



/chats — recent doubt threads across lessons (optional v1.1 if time)



/progress — completion + suggested next topics / study path

Content model (v1)

Hybrid catalog:





Seed lessons — Markdown under content/ (shipped with the app)



Dynamic lessons — rows in Postgres created when chat requests “add this topic”



Unified API merges both into Track → Module → Lesson for the UI

Entities:





Track → Module → Lesson (source: seed | user_requested, markdown body, examples, takeaways, pitfalls, languages, createdByUserId for dynamic)



LessonDraft — pending AI-generated lesson awaiting user confirm



Problem / Progress / ChatThread / ChatMessage as before

AI uses the current lesson/problem text as context so answers stay on-topic and interview-oriented.

Curriculum seed (MVP depth)

Seed enough to feel complete, not an encyclopedia:







Track



MVP modules (examples)





DSA



Arrays/Two pointers, Sliding window, Hashing, Trees/Graphs, Heaps, DP intro





LLD



OOP refresh, SOLID, common patterns, Parking lot / URL shortener design





HLD



URL shortener, Chat, News feed, Rate limiter, Cache design





Databases



Indexes, Transactions, Isolation, Query tuning basics, Redis use-cases





Networking



TCP vs UDP, HTTP lifecycle, DNS/TLS, LB/CDN





OS



Process vs thread, Sync primitives, Memory, Deadlocks





Java



Syntax/OOP, Collections, Streams, Concurrency, JVM GC, Spring Boot core





.NET



C# essentials, LINQ, async/await, GC, ASP.NET Core, EF Core





Senior FS



Auth, API design, caching, observability, behavioral

Each lesson: short concept → worked example → interview Q&A → pitfalls.

Doubt chat + AI features (v1)

Yes — chat is a core feature, not a side add-on.

Doubt-clearing chat (primary)





Persistent chat panel on each lesson/problem



User asks any doubt in natural language (“why do we need isolation levels?”, “trace this code”, “explain like I’m new”)



AI replies with full explanations: concept → why it matters → example → interview angle → short check question



Modes / shortcuts:





Explain simply / Explain deeply / Interview answer



Walk through this code (Java or C#)



Give another example



Quiz me after the doubt is cleared



Multi-turn: follow-ups stay in the same thread (“still confused about dirty reads”)



Context: current lesson markdown + recent chat messages + preferred language (Java / C#)



Persist threads in Postgres (ChatThread, ChatMessage) keyed by user + lesson

Other chat modes





Quiz me — 3–5 questions from the lesson, score + feedback



Compare Java vs .NET — dual-sample lessons



Mock interviewer — HLD / CS drill / behavioral; follow-ups + rubric

Response quality rules





Prefer structured, complete answers over one-liners



Use the open lesson as source of truth; say “not covered here” and teach carefully if the question goes beyond



Offer code examples in the user’s preferred language



Guardrails: no invented company insider facts; ask clarifying questions when the doubt is ambiguous

Add topic from chat (“include this in lessons”)

When the user types intents like:





“include this topic in your lessons”



“add a lesson on Redis pub/sub”



“create a module for JVM memory model”

the app will:





Detect intent (LLM tool/function call or classifier) — extract topic title, suggested track (DB / Java / HLD / …), and optional notes from the conversation



Check duplicates — if a similar lesson already exists, offer that instead of creating another



Generate a LessonDraft via LLM using the same lesson template: concept → example (Java/C# as relevant) → interview Q&A → pitfalls → Mermaid if useful



Show preview card in chat — title, track, short summary, “Add to my lessons” / “Edit draft” / “Cancel”



On confirm — persist as a published Lesson in Postgres under the chosen track/module; appear immediately in /tracks and openable like seed lessons



Doubt chat works on new lessons too — same AI panel, now grounded in the newly added markdown

Default scope for v1: lessons are added to the requesting user’s library (personal). Promoting a great user lesson into the shared seed catalog stays a later/manual step (export to content/ or admin approve).

flowchart TD
  Msg[User_chat_message] --> Detect{Add_topic_intent}
  Detect -->|no| Tutor[Normal_doubt_reply]
  Detect -->|yes| Extract[Extract_topic_and_track]
  Extract --> Dup{Similar_lesson_exists}
  Dup -->|yes| Suggest[Suggest_existing_lesson]
  Dup -->|no| Gen[Generate_LessonDraft]
  Gen --> Preview[Preview_in_chat]
  Preview --> Confirm{User_confirms}
  Confirm -->|yes| Save[Save_Lesson_in_Postgres]
  Confirm -->|no| Edit[Revise_or_cancel]
  Save --> Catalog[Shows_in_tracks_UI]

Where content / data comes from

We are not scraping LeetCode, InterviewBit, Educative, or paid course sites. Content is built as first-party markdown in the repo, using public knowledge + official docs + AI-assisted drafting that we review.

1. Curriculum outline (topic lists)

Public, non-copyrightable topic structures used as a syllabus only:





Classic DSA pattern lists (two pointers, sliding window, BFS/DFS, etc.)



Standard CS course outlines (DB / networking / OS interview checklists)



Common LLD/HLD problem names (URL shortener, rate limiter, etc.) — we write our own explanations

2. Authoritative references (link out, don’t copy)

Lessons cite and link to official sources; we rewrite in our own words:





.NET / C#: Microsoft Learn, ASP.NET Core docs, EF Core docs



Java: Oracle/OpenJDK docs, Spring Framework reference



Databases: PostgreSQL docs, Redis docs



Networking / HTTP: MDN, RFCs (high-level), Cloudflare/AWS architecture blogs (concepts)



OS / concurrency: well-known public concepts (processes, threads, locks) explained originally

3. Lesson bodies (what users actually read)

Two sources, one UI:





Seed: content/**/*.md in git — drafted/reviewed offline



User-requested: generated from chat → preview → Postgres after confirm



Same structure: concept → example → interview Q&A → pitfalls



Code samples in Java and/or C#; Mermaid where useful

4. Practice problems





Original or public-domain-style problem statements we write



Inspired by common patterns, not copied editorials from LeetCode/GeeksforGeeks premium



Solutions authored in Java + C# with complexity notes

5. AI at runtime (not a content dump)

LLM is used for:





Explain / simplify / quiz / mock interview on top of the open lesson



Optional: help author new lessons offline (draft → review → commit)

AI is not the database of truth. User progress / auth / bookmarks live in Postgres.

6. What we explicitly will not do





Scrape or mirror paid interview platforms



Copy GFG/LeetCode/Educative article text



Ship unreviewed AI-generated walls of text as “lessons”

Content pipeline (MVP)

flowchart TD
  Outline[Topic_outline] --> Draft[AI_or_human_draft]
  Docs[Official_docs_links] --> Draft
  Draft --> Review[Human_edit_examples]
  Review --> Md[content_markdown_in_git]
  Md --> App[Lesson_reader]
  App --> Tutor[AI_tutor_grounded_in_lesson]
  App --> Pg[(Postgres_progress_only)]

Suggested repo layout

D:/Project/InterviewTutor/
  frontend/          React + Vite
  backend/           .NET 8 API
  content/
    dsa/
    lld/
    hld/
    cs-databases/
    cs-networking/
    cs-os/
    java/
    dotnet/
    senior-fs/
  docs/              product + full curriculum outline
  README.md

Delivery phases





Skeleton — auth, grouped tracks list, lesson renderer (MD + Mermaid + code highlight), progress



Curriculum seed — modules above with at least 6–10 lessons per major track (Java/.NET and CS basics included)



AI tutor — doubt chat with lesson grounding; language preference (Java / C#)



Add-from-chat — intent detection, draft preview, confirm → personal lesson in catalog



Practice + mock — small dual-language problem bank + mock modes



Polish — search, bookmarks, spaced repetition, personalized study plan

Success criteria for MVP





User can study CS basics, Java, and .NET in the same app as DSA/LLD/HLD



Every lesson has at least one concrete example



AI can explain and quiz in interview style, grounded in the lesson



Progress persists; suggested path can mix CS + language + design tracks

Out of scope for v1





Full code execution / judge



Complete university-level OS/networking courses (interview-relevant depth only)



Mobile native apps



Paid marketplace / multi-tenant CMS



Scraping third-party paid interview content

Immediate next step after plan approval

In D:/Project/InterviewTutor: git init, scaffold frontend/backend/content/, then implement Track → Lesson browsing (including CS + Java + .NET) before wiring AI. Work happens in that repo, not in StockYouNeed.