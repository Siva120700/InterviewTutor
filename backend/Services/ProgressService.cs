using InterviewTutor.Api.Data;
using InterviewTutor.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTutor.Api.Services;

public class ProgressService(AppDbContext db, CatalogService catalog)
{
    public async Task<ProgressDto> GetAsync(CancellationToken ct = default)
    {
        var completed = await db.LessonProgress
            .Where(p => p.Completed)
            .Select(p => p.LessonId)
            .ToListAsync(ct);
        var total = (await catalog.GetAllLessonsAsync(false, ct)).Count;
        return new ProgressDto(completed, total, completed.Count);
    }

    public async Task<ProgressDto> SetCompletedAsync(string lessonId, bool completed, CancellationToken ct = default)
    {
        var row = await db.LessonProgress.FirstOrDefaultAsync(p => p.LessonId == lessonId, ct);
        if (row is null)
        {
            row = new LessonProgress { LessonId = lessonId, Completed = completed };
            db.LessonProgress.Add(row);
        }
        else
        {
            row.Completed = completed;
            row.UpdatedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync(ct);
        return await GetAsync(ct);
    }

    public async Task<IReadOnlyList<LessonDto>> SuggestNextAsync(int take = 5, CancellationToken ct = default)
    {
        var completed = (await db.LessonProgress.Where(p => p.Completed).Select(p => p.LessonId).ToListAsync(ct))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var all = await catalog.GetAllLessonsAsync(false, ct);
        return all.Where(l => !completed.Contains(l.Id)).OrderBy(l => l.Order).Take(take).ToList();
    }
}
