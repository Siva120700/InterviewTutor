using InterviewTutor.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTutor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController(CatalogService catalog) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await catalog.GetTracksAsync(ct));

    [HttpGet("{trackSlug}/lessons/{lessonId}")]
    public async Task<IActionResult> GetLesson(string trackSlug, string lessonId, CancellationToken ct)
    {
        var lesson = await catalog.GetLessonAsync(lessonId, ct);
        if (lesson is null) return NotFound();
        if (!string.Equals(lesson.TrackSlug, trackSlug, StringComparison.OrdinalIgnoreCase))
            return NotFound();
        return Ok(lesson);
    }
}
