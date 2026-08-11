using InterviewTutor.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTutor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressController(ProgressService progress) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct) => Ok(await progress.GetAsync(ct));

    [HttpPost("lessons/{lessonId}/complete")]
    public async Task<IActionResult> Complete(string lessonId, [FromQuery] bool completed = true, CancellationToken ct = default) =>
        Ok(await progress.SetCompletedAsync(lessonId, completed, ct));

    [HttpGet("suggested")]
    public async Task<IActionResult> Suggested([FromQuery] int take = 5, CancellationToken ct = default) =>
        Ok(await progress.SuggestNextAsync(take, ct));
}
