using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using InterviewTutor.Api.Data;
using InterviewTutor.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTutor.Api.Services;

public class AiTutorService(
    AppDbContext db,
    CatalogService catalog,
    IHttpClientFactory httpClientFactory,
    IConfiguration config,
    ILogger<AiTutorService> logger)
{
    private static readonly Regex AddTopicRegex = new(
        @"\b(add|include|create)\b.+\b(lesson|topic|module)\b|\b(add this topic|include this topic|create a lesson)\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public async Task<SendChatResponse> ChatAsync(string lessonId, SendChatRequest request, CancellationToken ct = default)
    {
        var lesson = await catalog.GetLessonAsync(lessonId, ct)
            ?? throw new InvalidOperationException("Lesson not found");

        var thread = request.ThreadId is Guid tid
            ? await db.ChatThreads.Include(t => t.Messages).FirstOrDefaultAsync(t => t.Id == tid, ct)
            : await db.ChatThreads.Include(t => t.Messages)
                .Where(t => t.LessonId == lessonId && t.ProblemId == null)
                .OrderByDescending(t => t.UpdatedAt)
                .FirstOrDefaultAsync(ct);

        if (thread is null)
        {
            thread = new ChatThread
            {
                LessonId = lessonId,
                Title = $"Doubts: {lesson.Title}"
            };
            db.ChatThreads.Add(thread);
        }

        var userMsg = new ChatMessage
        {
            ThreadId = thread.Id,
            Role = "user",
            Content = request.Message
        };
        db.ChatMessages.Add(userMsg);
        thread.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        LessonDraftDto? draftDto = null;
        LessonDto? suggested = null;

        if (LooksLikeAddTopic(request.Message))
        {
            var topic = ExtractTopic(request.Message, lesson.Title);
            var track = GuessTrack(request.Message, lesson.TrackSlug);
            suggested = await catalog.FindSimilarAsync(topic, track, ct);
            if (suggested is null)
            {
                var markdown = await GenerateLessonMarkdownAsync(topic, track, request.PreferredLanguage, ct);
                var draft = new LessonDraft
                {
                    Title = topic,
                    TrackSlug = track,
                    Module = "Custom",
                    Summary = $"Interview-oriented lesson on {topic}",
                    MarkdownBody = markdown,
                    Status = "pending"
                };
                db.LessonDrafts.Add(draft);
                await db.SaveChangesAsync(ct);
                draftDto = new LessonDraftDto(draft.Id, draft.Title, draft.TrackSlug, draft.Module, draft.Summary, draft.MarkdownBody, draft.Status);
            }
        }

        var history = thread.Messages.OrderBy(m => m.CreatedAt).TakeLast(12).ToList();
        var reply = await GenerateTutorReplyAsync(lesson, history, request, suggested, draftDto, ct);

        var assistant = new ChatMessage
        {
            ThreadId = thread.Id,
            Role = "assistant",
            Content = reply,
            MetaJson = draftDto is null && suggested is null
                ? null
                : JsonSerializer.Serialize(new { draftId = draftDto?.Id, suggestedLessonId = suggested?.Id })
        };
        db.ChatMessages.Add(assistant);
        thread.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        await db.Entry(thread).Collection(t => t.Messages).LoadAsync(ct);
        var dto = ToThreadDto(thread);
        return new SendChatResponse(
            dto,
            new ChatMessageDto(assistant.Id, assistant.Role, assistant.Content, assistant.CreatedAt),
            draftDto,
            suggested);
    }

    public async Task<ChatThreadDto?> GetThreadAsync(string lessonId, CancellationToken ct = default)
    {
        var thread = await db.ChatThreads.Include(t => t.Messages)
            .Where(t => t.LessonId == lessonId && t.ProblemId == null)
            .OrderByDescending(t => t.UpdatedAt)
            .FirstOrDefaultAsync(ct);
        return thread is null ? null : ToThreadDto(thread);
    }

    public async Task<LessonDraftDto?> GetDraftAsync(Guid id, CancellationToken ct = default)
    {
        var d = await db.LessonDrafts.FindAsync([id], ct);
        return d is null ? null : new LessonDraftDto(d.Id, d.Title, d.TrackSlug, d.Module, d.Summary, d.MarkdownBody, d.Status);
    }

    public async Task<LessonDto?> ConfirmDraftAsync(Guid id, ConfirmDraftRequest req, CancellationToken ct = default)
    {
        var draft = await db.LessonDrafts.FindAsync([id], ct);
        if (draft is null || draft.Status != "pending")
            return null;

        if (!string.IsNullOrWhiteSpace(req.EditedTitle))
            draft.Title = req.EditedTitle.Trim();
        if (!string.IsNullOrWhiteSpace(req.EditedMarkdown))
            draft.MarkdownBody = req.EditedMarkdown;
        if (!string.IsNullOrWhiteSpace(req.TrackSlug))
            draft.TrackSlug = req.TrackSlug.Trim().ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(req.Module))
            draft.Module = req.Module.Trim();

        return await catalog.PublishDraftAsync(draft, ct);
    }

    public async Task CancelDraftAsync(Guid id, CancellationToken ct = default)
    {
        var draft = await db.LessonDrafts.FindAsync([id], ct);
        if (draft is null) return;
        draft.Status = "cancelled";
        await db.SaveChangesAsync(ct);
    }

    public async Task<MockSessionDto> StartMockAsync(StartMockRequest request, CancellationToken ct = default)
    {
        var opener = await GenerateMockOpenerAsync(request.Mode, ct);
        var transcript = new List<ChatMessageDto>
        {
            new(Guid.NewGuid(), "assistant", opener, DateTime.UtcNow)
        };

        var session = new MockSession
        {
            Mode = request.Mode.ToLowerInvariant(),
            DurationMinutes = request.DurationMinutes,
            TranscriptJson = JsonSerializer.Serialize(transcript)
        };
        db.MockSessions.Add(session);
        await db.SaveChangesAsync(ct);
        return ToMockDto(session, transcript);
    }

    public async Task<MockSessionDto?> MockTurnAsync(Guid id, MockTurnRequest request, CancellationToken ct = default)
    {
        var session = await db.MockSessions.FindAsync([id], ct);
        if (session is null) return null;

        var transcript = JsonSerializer.Deserialize<List<ChatMessageDto>>(session.TranscriptJson) ?? [];
        transcript.Add(new ChatMessageDto(Guid.NewGuid(), "user", request.Message, DateTime.UtcNow));

        var reply = await GenerateMockFollowUpAsync(session.Mode, transcript, ct);
        transcript.Add(new ChatMessageDto(Guid.NewGuid(), "assistant", reply, DateTime.UtcNow));

        session.TranscriptJson = JsonSerializer.Serialize(transcript);
        await db.SaveChangesAsync(ct);
        return ToMockDto(session, transcript);
    }

    public async Task<MockSessionDto?> EndMockAsync(Guid id, CancellationToken ct = default)
    {
        var session = await db.MockSessions.FindAsync([id], ct);
        if (session is null) return null;

        var transcript = JsonSerializer.Deserialize<List<ChatMessageDto>>(session.TranscriptJson) ?? [];
        var rubric = await GenerateMockRubricAsync(session.Mode, transcript, ct);
        session.EndedAt = DateTime.UtcNow;
        session.RubricJson = rubric;
        await db.SaveChangesAsync(ct);
        return ToMockDto(session, transcript, rubric);
    }

    private async Task<string> GenerateTutorReplyAsync(
        LessonDto lesson,
        List<ChatMessage> history,
        SendChatRequest request,
        LessonDto? suggested,
        LessonDraftDto? draft,
        CancellationToken ct)
    {
        if (suggested is not null)
        {
            return $"It looks like you want to add a topic. A similar lesson already exists: **{suggested.Title}** (`{suggested.Id}`) in track `{suggested.TrackSlug}`. Open that instead of creating a duplicate.";
        }

        if (draft is not null)
        {
            return $"I drafted a new lesson: **{draft.Title}** for track `{draft.TrackSlug}`. Review the preview card and confirm to add it to your catalog.";
        }

        var modeHint = request.Mode switch
        {
            "explain_simple" => "Explain simply, like for a smart beginner.",
            "explain_deep" => "Explain deeply with edge cases and internals.",
            "interview" => "Answer as a strong senior interview answer (structured, concise, confident).",
            "walkthrough" => $"Walk through code carefully in {request.PreferredLanguage}.",
            "example" => $"Give another concrete example in {request.PreferredLanguage}.",
            "quiz" => "Ask 3–5 short quiz questions based on the lesson, then wait for answers.",
            _ => "Clear the user's doubt thoroughly."
        };

        var system = $$"""
            You are InterviewTutor, a senior interview coach.
            Prefer structured, complete answers: concept → why it matters → example → interview angle → short check question.
            Preferred language for code: {{request.PreferredLanguage}}.
            Mode: {{modeHint}}
            Use the lesson as source of truth. If the question goes beyond the lesson, say so and teach carefully.
            Do not invent company insider facts.

            LESSON TITLE: {{lesson.Title}}
            LESSON TRACK: {{lesson.TrackSlug}}
            LESSON CONTENT:
            {{lesson.MarkdownBody ?? lesson.Summary}}
            """;

        return await CompleteChatAsync(system, history.Select(m => (m.Role, m.Content)).Append(("user", request.Message)), ct)
               ?? StubTutorReply(lesson, request);
    }

    private async Task<string> GenerateLessonMarkdownAsync(string topic, string track, string lang, CancellationToken ct)
    {
        var system = """
            Generate a markdown interview lesson with sections:
            ## Concept
            ## Worked example (Java and C# when relevant)
            ## Interview Q&A
            ## Pitfalls
            Optional Mermaid diagram if useful. No frontmatter. Be original; do not copy paid course text.
            """;
        var user = $"Topic: {topic}\nTrack: {track}\nPreferred language: {lang}";
        var result = await CompleteChatAsync(system, [("user", user)], ct);
        return result ?? $$"""
            ## Concept
            {{topic}} is a common interview topic in the {{track}} track. Focus on definitions, trade-offs, and when to use it.

            ## Worked example

            ```java
            // Java sketch for {{topic}}
            public class Example {
              public static void demo() {
                System.out.println("{{topic}}");
              }
            }
            ```

            ```csharp
            // C# sketch for {{topic}}
            public static class Example {
              public static void Demo() => Console.WriteLine("{{topic}}");
            }
            ```

            ## Interview Q&A
            - **Q:** What is {{topic}}?
              **A:** Explain the core idea, a concrete use case, and one trade-off.
            - **Q:** When would you not use it?
              **A:** Call out complexity, operational cost, or simpler alternatives.

            ## Pitfalls
            - Jumping to buzzwords without constraints
            - Skipping complexity or failure modes
            """;
    }

    private async Task<string> GenerateMockOpenerAsync(string mode, CancellationToken ct)
    {
        var prompt = mode.ToLowerInvariant() switch
        {
            "behavioral" => "Start a behavioral mock: ask about a hard technical leadership conflict.",
            "cs" => "Start a CS fundamentals mock: ask about database isolation levels.",
            _ => "Start an HLD mock: ask the candidate to design a URL shortener. Ask clarifying questions."
        };
        return await CompleteChatAsync(
                   "You are a strict but fair mock interviewer. Ask one question at a time.",
                   [("user", prompt)], ct)
               ?? mode.ToLowerInvariant() switch
               {
                   "behavioral" => "Tell me about a time you disagreed with a tech lead on architecture. What did you do?",
                   "cs" => "Explain READ COMMITTED vs REPEATABLE READ, and when each matters in production.",
                   _ => "Design a URL shortener. What are the functional and non-functional requirements?"
               };
    }

    private async Task<string> GenerateMockFollowUpAsync(string mode, List<ChatMessageDto> transcript, CancellationToken ct)
    {
        var system = $"You are continuing a {mode} mock interview. Ask a sharp follow-up or probe a gap. Keep it to 2–4 sentences.";
        var msgs = transcript.Select(t => (t.Role, t.Content));
        return await CompleteChatAsync(system, msgs, ct)
               ?? "Interesting. What are the bottlenecks at 10x traffic, and how would you measure them?";
    }

    private async Task<string> GenerateMockRubricAsync(string mode, List<ChatMessageDto> transcript, CancellationToken ct)
    {
        var system = "Produce a short markdown rubric: Strengths, Gaps, Score /10, Next study topics.";
        var msgs = transcript.Select(t => (t.Role, t.Content));
        return await CompleteChatAsync(system, msgs, ct)
               ?? """
               ## Rubric
               - **Strengths:** Covered core requirements
               - **Gaps:** Dig deeper into failure modes and metrics
               - **Score:** 6/10
               - **Next:** Caching, consistency, and capacity estimation drills
               """;
    }

    private async Task<string?> CompleteChatAsync(string system, IEnumerable<(string Role, string Content)> messages, CancellationToken ct)
    {
        var apiKey = config["OPENAI_API_KEY"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
            return null;

        var baseUrl = (config["OPENAI_BASE_URL"] ?? Environment.GetEnvironmentVariable("OPENAI_BASE_URL") ?? "https://api.openai.com/v1")
            .TrimEnd('/');
        var model = config["OPENAI_MODEL"] ?? Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

        try
        {
            var client = httpClientFactory.CreateClient("openai");
            using var req = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/chat/completions");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var payload = new
            {
                model,
                temperature = 0.4,
                messages = new object[] { new { role = "system", content = system } }
                    .Concat(messages.Select(m => new { role = m.Role, content = m.Content }))
                    .ToArray()
            };

            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            using var res = await client.SendAsync(req, ct);
            var body = await res.Content.ReadAsStringAsync(ct);
            if (!res.IsSuccessStatusCode)
            {
                logger.LogWarning("OpenAI error {Status}: {Body}", res.StatusCode, body);
                return null;
            }

            using var doc = JsonDocument.Parse(body);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "AI completion failed");
            return null;
        }
    }

    private static string StubTutorReply(LessonDto lesson, SendChatRequest request) =>
        $$"""
        *(Local stub — set OPENAI_API_KEY for live AI.)*

        ## On {{lesson.Title}}
        You asked: {{request.Message}}

        **Concept:** Stay grounded in the lesson summary: {{lesson.Summary}}

        **Why it matters in interviews:** Interviewers want trade-offs, complexity, and a clear example — not buzzwords.

        **Example angle ({{request.PreferredLanguage}}):** Restate the worked example from the lesson in your own words, then change one constraint.

        **Check question:** Can you explain this topic in 60 seconds with one trade-off?
        """;

    private static bool LooksLikeAddTopic(string message) => AddTopicRegex.IsMatch(message);

    private static string ExtractTopic(string message, string fallback)
    {
        var m = Regex.Match(message, @"(?:lesson|topic|module)\s+(?:on|for|about)\s+(.+)$", RegexOptions.IgnoreCase);
        if (m.Success) return TrimTopic(m.Groups[1].Value);
        m = Regex.Match(message, @"add\s+(.+?)(?:\s+to\s+(?:my\s+)?lessons)?$", RegexOptions.IgnoreCase);
        if (m.Success) return TrimTopic(m.Groups[1].Value);
        return fallback;
    }

    private static string TrimTopic(string s) =>
        s.Trim().TrimEnd('.', '!', '?').Trim('"', '\'');

    private static string GuessTrack(string message, string fallback)
    {
        var lower = message.ToLowerInvariant();
        foreach (var key in ContentCatalog.Tracks.Keys)
        {
            if (lower.Contains(key.Replace('-', ' ')) || lower.Contains(key))
                return key;
        }
        if (lower.Contains("redis") || lower.Contains("sql") || lower.Contains("index")) return "cs-databases";
        if (lower.Contains("http") || lower.Contains("tcp") || lower.Contains("dns")) return "cs-networking";
        if (lower.Contains("thread") || lower.Contains("process") || lower.Contains("deadlock")) return "cs-os";
        if (lower.Contains("spring") || lower.Contains("jvm")) return "java";
        if (lower.Contains("asp.net") || lower.Contains("ef core") || lower.Contains("csharp") || lower.Contains("c#")) return "dotnet";
        if (lower.Contains("system design") || lower.Contains("hld")) return "hld";
        if (lower.Contains("design pattern") || lower.Contains("lld")) return "lld";
        return fallback;
    }

    private static ChatThreadDto ToThreadDto(ChatThread thread) =>
        new(
            thread.Id,
            thread.LessonId,
            thread.ProblemId,
            thread.Title,
            thread.Messages.OrderBy(m => m.CreatedAt)
                .Select(m => new ChatMessageDto(m.Id, m.Role, m.Content, m.CreatedAt))
                .ToList());

    private static MockSessionDto ToMockDto(MockSession s, IReadOnlyList<ChatMessageDto> transcript, string? rubric = null) =>
        new(s.Id, s.Mode, s.DurationMinutes, s.StartedAt, s.EndedAt, transcript, rubric ?? s.RubricJson);
}
