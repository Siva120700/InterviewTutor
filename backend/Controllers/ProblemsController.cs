using InterviewTutor.Api.Data;
using InterviewTutor.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewTutor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProblemsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var items = await db.Problems.AsNoTracking()
            .OrderBy(p => p.Order)
            .Select(p => new ProblemDto(p.Id, p.Slug, p.Title, p.Difficulty, p.TrackSlug, p.PromptMarkdown, null, null, p.ComplexityNotes))
            .ToListAsync(ct);
        return Ok(items);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug, CancellationToken ct)
    {
        var p = await db.Problems.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug, ct);
        if (p is null) return NotFound();
        return Ok(new ProblemDto(p.Id, p.Slug, p.Title, p.Difficulty, p.TrackSlug, p.PromptMarkdown, p.JavaSolution, p.CsharpSolution, p.ComplexityNotes));
    }
}
