# Single-app deploy (API + React UI together)

One Docker image serves both the React UI and the .NET API. You only host **one** web service (+ free Neon Postgres).

## What changed

- Frontend calls `/api/...` on the **same origin**
- Backend serves `wwwroot` (built React app) + API
- Local Vite still works via `/api` proxy to port 5080

## Free hosting steps (Render + Neon)

### 1. Push repo to GitHub

Commit and push `InterviewTutor` including `Dockerfile`.

### 2. Create free Postgres (Neon)

1. [neon.tech](https://neon.tech) → Create project  
2. Copy the connection string. Either format works:

**URI (Neon default — OK):**
```text
postgresql://USER:PASSWORD@ep-xxxx.aws.neon.tech/neondb?sslmode=require
```

**Or Npgsql key=value:**
```text
Host=ep-xxxx.aws.neon.tech;Database=neondb;Username=USER;Password=PASSWORD;SSL Mode=Require;Trust Server Certificate=true
```

Important: if you use URI, keep `sslmode=require` complete (not just `sslmode`).

### 3. Deploy one Web Service (Render)

1. [render.com](https://render.com) → **New → Web Service** → your repo  
2. Settings:
   - **Language / Runtime:** Docker  
   - **Dockerfile path:** `Dockerfile` (repo root)  
   - **Instance type:** Free  
3. Environment:

| Key | Value |
|-----|--------|
| `ConnectionStrings__Default` | Neon string from step 2 |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `CONTENT_ROOT` | `/app/content` |
| `OPENAI_API_KEY` | optional |

4. Deploy → open the Render URL (e.g. `https://interviewtutor.onrender.com`)

That’s the whole app — UI + API — one URL.

### 4. Smoke test

- `/` → home page  
- `/api/health` → `{ "status": "ok" }`  
- Tracks / lessons / progress  

**Note:** Free Render apps sleep when idle; first load after sleep can be slow.

## Local single-app check

```bash
docker compose up -d
docker build -t interviewtutor .
docker run --rm -p 8080:8080 ^
  -e ConnectionStrings__Default="Host=host.docker.internal;Port=5433;Database=interviewtutor;Username=interviewtutor;Password=interviewtutor" ^
  -e CONTENT_ROOT=/app/content ^
  interviewtutor
```

Open http://localhost:8080

## Local dual process (dev)

Still supported:

```bash
docker compose up -d
cd backend && dotnet run
cd frontend && npm run dev
```

Vite proxies `/api` → `http://127.0.0.1:5080`.
