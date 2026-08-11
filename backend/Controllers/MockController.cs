using InterviewTutor.Api.Models;
using InterviewTutor.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTutor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MockController(AiTutorService ai) : ControllerBase
{
    [HttpPost("start")]
    public async Task<IActionResult> Start([FromBody] StartMockRequest request, CancellationToken ct) =>
        Ok(await ai.StartMockAsync(request, ct));

    [HttpPost("{id:guid}/message")]
    public async Task<IActionResult> Message(Guid id, [FromBody] MockTurnRequest request, CancellationToken ct)
    {
        var session = await ai.MockTurnAsync(id, request, ct);
        return session is null ? NotFound() : Ok(session);
    }

    [HttpPost("{id:guid}/end")]
    public async Task<IActionResult> End(Guid id, CancellationToken ct)
    {
        var session = await ai.EndMockAsync(id, ct);
        return session is null ? NotFound() : Ok(session);
    }
}
