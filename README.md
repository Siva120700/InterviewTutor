# InterviewTutor

Personal interview prep app: curated tracks (DSA, LLD, HLD, CS, Java, .NET), lesson reader, progress, AI doubt chat, add-topic-from-chat, and a small practice/mock set.

No auth or billing — single-user local use.

## Stack

- **Frontend:** React + Vite + TypeScript
- **Backend:** .NET 8 Web API + EF Core
- **Database:** PostgreSQL (Docker)
- **AI:** OpenAI-compatible API (optional; falls back to local stub)

## Quick start

```bash
# 1. Start Postgres
docker compose up -d

# 2. Backend
cd backend
dotnet run

# 3. Frontend (another terminal)
cd frontend
npm install
npm run dev
```

Open http://localhost:5173 — API defaults to http://localhost:5080. Postgres is on host port **5433**.

Copy `.env.example` values into `backend/appsettings.Development.json` or environment variables. Set `OPENAI_API_KEY` for real AI replies.

## Curriculum

Basics → advanced study path: see [docs/curriculum.md](docs/curriculum.md) (DSA, **DSA Patterns**, LLD, HLD, CS, Java, .NET, React, Auth/Cache/Security, Senior FS).

## Content

Seed lessons live under `content/<track>/`. Each file uses YAML frontmatter:

```yaml
---
id: dsa-two-pointers
title: Two Pointers
track: dsa
module: Arrays and Patterns
order: 1
languages: [java, csharp]
summary: Classic two-pointer pattern for sorted arrays.
---
```

Chat-added lessons are stored in Postgres and merged into the same catalog.
