using System.Text.Json;
using InterviewTutor.Api.Data;
using InterviewTutor.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTutor.Api.Services;

public class CatalogService(MarkdownLessonLoader loader, AppDbContext db)
{
    public async Task<IReadOnlyList<TrackDto>> GetTracksAsync(CancellationToken ct = default)
    {
        var completed = await db.LessonProgress
            .Where(p => p.Completed)
            .Select(p => p.LessonId)
            .ToListAsync(ct);

        var completedSet = completed.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var lessons = await GetAllLessonsAsync(includeBody: false, ct);

        return ContentCatalog.Tracks
            .Select(kv =>
            {
                var trackLessons = lessons
                    .Where(l => string.Equals(l.TrackSlug, kv.Key, StringComparison.OrdinalIgnoreCase))
                    .Select(l => l with { Completed = completedSet.Contains(l.Id) })
                    .ToList();

                var modules = trackLessons
                    .GroupBy(l => l.Module)
                    .OrderBy(g => g.Min(x => x.Order))
                    .Select(g => new ModuleDto(
                        g.Key,
                        g.OrderBy(x => x.Order).ThenBy(x => x.Title).ToList()))
                    .ToList();

                return new TrackDto(kv.Key, kv.Value.Title, kv.Value.Group, kv.Value.Description, modules);
            })
            .Where(t => t.Modules.Count > 0)
            .OrderBy(t => t.Group)
            .ThenBy(t => t.Title)
            .ToList();
    }

    public async Task<LessonDto?> GetLessonAsync(string id, CancellationToken ct = default)
    {
        var seed = loader.LoadSeedLesson(id, includeBody: true);
        if (seed is not null)
        {
            var done = await db.LessonProgress.AnyAsync(p => p.LessonId == id && p.Completed, ct);
            return seed with { Completed = done };
        }

        var dyn = await db.DynamicLessons.FirstOrDefaultAsync(l => l.Slug == id || l.Id.ToString() == id, ct);
        if (dyn is null)
            return null;

        var completed = await db.LessonProgress.AnyAsync(p => p.LessonId == dyn.Slug && p.Completed, ct);
        return ToDto(dyn, includeBody: true, completed);
    }

    public async Task<IReadOnlyList<LessonDto>> GetAllLessonsAsync(bool includeBody, CancellationToken ct = default)
    {
        var seed = loader.LoadSeedLessons(includeBody);
        var dynamic = await db.DynamicLessons.AsNoTracking().ToListAsync(ct);
        var dynDtos = dynamic.Select(d => ToDto(d, includeBody, false));
        return seed.Concat(dynDtos).ToList();
    }

    public async Task<LessonDto?> FindSimilarAsync(string title, string? trackSlug, CancellationToken ct = default)
    {
        var all = await GetAllLessonsAsync(false, ct);
        var needle = Normalize(title);
        return all.FirstOrDefault(l =>
            (trackSlug is null || string.Equals(l.TrackSlug, trackSlug, StringComparison.OrdinalIgnoreCase)) &&
            (Normalize(l.Title).Contains(needle) || needle.Contains(Normalize(l.Title)) ||
             Normalize(l.Summary).Contains(needle)));
    }

    public async Task<LessonDto> PublishDraftAsync(LessonDraft draft, CancellationToken ct = default)
    {
        var slug = Slugify(draft.Title);
        var existing = await db.DynamicLessons.AnyAsync(l => l.Slug == slug, ct);
        if (existing)
            slug = $"{slug}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

        var entity = new DynamicLesson
        {
            Slug = slug,
            Title = draft.Title,
            TrackSlug = draft.TrackSlug,
            Module = draft.Module,
            Order = 900,
            Summary = draft.Summary,
            MarkdownBody = draft.MarkdownBody,
            LanguagesJson = JsonSerializer.Serialize(new[] { "java", "csharp" }),
        };

        db.DynamicLessons.Add(entity);
        draft.Status = "confirmed";
        await db.SaveChangesAsync(ct);
        return ToDto(entity, includeBody: true, false);
    }

    private static LessonDto ToDto(DynamicLesson d, bool includeBody, bool completed) =>
        new(
            d.Slug,
            d.Title,
            d.TrackSlug,
            d.Module,
            d.Order,
            d.Summary,
            ParseLanguages(d.LanguagesJson),
            "user_requested",
            includeBody ? d.MarkdownBody : null,
            completed
        );

    private static List<string> ParseLanguages(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? [];
        }
        catch
        {
            return [];
        }
    }

    private static string Normalize(string s) =>
        new string(s.ToLowerInvariant().Where(char.IsLetterOrDigit).ToArray());

    private static string Slugify(string title)
    {
        var chars = title.ToLowerInvariant()
            .Select(c => char.IsLetterOrDigit(c) ? c : '-')
            .ToArray();
        var slug = new string(chars);
        while (slug.Contains("--", StringComparison.Ordinal))
            slug = slug.Replace("--", "-", StringComparison.Ordinal);
        return slug.Trim('-');
    }
}
