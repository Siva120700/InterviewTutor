using Microsoft.AspNetCore.Mvc;

namespace InterviewTutor.Api.Controllers;

[ApiController]
[Route("api/dsa-sheet")]
public class DsaSheetController(IWebHostEnvironment env, IConfiguration config) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var path = ResolveSheetPath();
        if (path is null || !System.IO.File.Exists(path))
            return NotFound(new { error = "DSA practice sheet not found." });

        return PhysicalFile(path, "application/json");
    }

    private string? ResolveSheetPath()
    {
        var configured = config["CONTENT_ROOT"] ?? config["ContentRoot"];
        var root = Path.GetFullPath(
            configured is null
                ? Path.Combine(env.ContentRootPath, "..", "content")
                : Path.IsPathRooted(configured)
                    ? configured
                    : Path.Combine(env.ContentRootPath, configured));

        var candidate = Path.Combine(root, "dsa-sheet", "practice-sheet.json");
        return candidate;
    }
}
