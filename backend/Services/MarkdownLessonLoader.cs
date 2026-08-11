using InterviewTutor.Api.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace InterviewTutor.Api.Services;

public class MarkdownLessonLoader
{
    private readonly string _contentRoot;
    private readonly IDeserializer _yaml;

    public MarkdownLessonLoader(IConfiguration config, IWebHostEnvironment env)
    {
        var configured = config["CONTENT_ROOT"] ?? config["ContentRoot"];
        _contentRoot = Path.GetFullPath(
            configured is null
                ? Path.Combine(env.ContentRootPath, "..", "content")
                : Path.IsPathRooted(configured)
                    ? configured
                    : Path.Combine(env.ContentRootPath, configured));

        _yaml = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
    }

    public string ContentRoot => _contentRoot;

    public IReadOnlyList<LessonDto> LoadSeedLessons(bool includeBody)
    {
        if (!Directory.Exists(_contentRoot))
            return [];

        var results = new List<LessonDto>();
        foreach (var file in Directory.EnumerateFiles(_contentRoot, "*.md", SearchOption.AllDirectories))
        {
            var parsed = ParseFile(file, includeBody);
            if (parsed is not null)
                results.Add(parsed);
        }

        return results
            .OrderBy(l => l.TrackSlug)
            .ThenBy(l => l.Order)
            .ThenBy(l => l.Title)
            .ToList();
    }

    public LessonDto? LoadSeedLesson(string id, bool includeBody = true)
    {
        if (!Directory.Exists(_contentRoot))
            return null;

        foreach (var file in Directory.EnumerateFiles(_contentRoot, "*.md", SearchOption.AllDirectories))
        {
            var parsed = ParseFile(file, includeBody);
            if (parsed is not null && string.Equals(parsed.Id, id, StringComparison.OrdinalIgnoreCase))
                return parsed;
        }

        return null;
    }

    private LessonDto? ParseFile(string path, bool includeBody)
    {
        var text = File.ReadAllText(path);
        if (!text.StartsWith("---"))
            return null;

        var end = text.IndexOf("---", 3, StringComparison.Ordinal);
        if (end < 0)
            return null;

        var front = text[3..end].Trim();
        var body = text[(end + 3)..].Trim();

        LessonFrontmatter meta;
        try
        {
            meta = _yaml.Deserialize<LessonFrontmatter>(front) ?? new LessonFrontmatter();
        }
        catch
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(meta.Id) || string.IsNullOrWhiteSpace(meta.Track))
            return null;

        var track = meta.Track.Trim().ToLowerInvariant();
        return new LessonDto(
            meta.Id.Trim(),
            string.IsNullOrWhiteSpace(meta.Title) ? meta.Id : meta.Title.Trim(),
            track,
            string.IsNullOrWhiteSpace(meta.Module) ? "General" : meta.Module.Trim(),
            meta.Order,
            meta.Summary?.Trim() ?? "",
            meta.Languages ?? [],
            "seed",
            includeBody ? body : null
        );
    }
}
