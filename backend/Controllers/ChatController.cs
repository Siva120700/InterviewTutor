using InterviewTutor.Api.Models;
using InterviewTutor.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTutor.Api.Controllers;

[ApiController]
[Route("api")]
public class ChatController(AiTutorService ai) : ControllerBase
{
    [HttpGet("lessons/{lessonId}/chat")]
    public async Task<IActionResult> GetThread(string lessonId, CancellationToken ct)
    {
        var thread = await ai.GetThreadAsync(lessonId, ct);
        return Ok(thread);
    }

    [HttpPost("lessons/{lessonId}/chat")]
    public async Task<IActionResult> Send(string lessonId, [FromBody] SendChatRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message required");
        try
        {
            return Ok(await ai.ChatAsync(lessonId, request, ct));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("drafts/{id:guid}")]
    public async Task<IActionResult> GetDraft(Guid id, CancellationToken ct)
    {
        var draft = await ai.GetDraftAsync(id, ct);
        return draft is null ? NotFound() : Ok(draft);
    }

    [HttpPost("drafts/{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id, [FromBody] ConfirmDraftRequest? request, CancellationToken ct)
    {
        var lesson = await ai.ConfirmDraftAsync(id, request ?? new ConfirmDraftRequest(), ct);
        return lesson is null ? NotFound() : Ok(lesson);
    }

    [HttpPost("drafts/{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        await ai.CancelDraftAsync(id, ct);
        return NoContent();
    }
}
