namespace InterviewTutor.Api.Models;

public record LessonFrontmatter
{
    public string Id { get; init; } = "";
    public string Title { get; init; } = "";
    public string Track { get; init; } = "";
    public string Module { get; init; } = "";
    public int Order { get; init; }
    public List<string> Languages { get; init; } = [];
    public string Summary { get; init; } = "";
}

public record LessonDto(
    string Id,
    string Title,
    string TrackSlug,
    string Module,
    int Order,
    string Summary,
    IReadOnlyList<string> Languages,
    string Source,
    string? MarkdownBody = null,
    bool Completed = false
);

public record ModuleDto(string Name, IReadOnlyList<LessonDto> Lessons);

public record TrackDto(
    string Slug,
    string Title,
    string Group,
    string Description,
    IReadOnlyList<ModuleDto> Modules
);

public record ProgressDto(IReadOnlyList<string> CompletedLessonIds, int TotalLessons, int CompletedCount);

public record ChatMessageDto(Guid Id, string Role, string Content, DateTime CreatedAt, object? Meta = null);

public record ChatThreadDto(Guid Id, string LessonId, string? ProblemId, string Title, IReadOnlyList<ChatMessageDto> Messages);

public record SendChatRequest(string Message, string? Mode, string PreferredLanguage = "java", Guid? ThreadId = null);

public record SendChatResponse(ChatThreadDto Thread, ChatMessageDto AssistantMessage, LessonDraftDto? Draft = null, LessonDto? SuggestedExisting = null);

public record LessonDraftDto(Guid Id, string Title, string TrackSlug, string Module, string Summary, string MarkdownBody, string Status);

public record ConfirmDraftRequest(string? EditedTitle = null, string? EditedMarkdown = null, string? TrackSlug = null, string? Module = null);

public record ProblemDto(
    Guid Id,
    string Slug,
    string Title,
    string Difficulty,
    string TrackSlug,
    string PromptMarkdown,
    string? JavaSolution,
    string? CsharpSolution,
    string ComplexityNotes
);

public record StartMockRequest(string Mode, int DurationMinutes = 30);

public record MockTurnRequest(string Message);

public record MockSessionDto(
    Guid Id,
    string Mode,
    int DurationMinutes,
    DateTime StartedAt,
    DateTime? EndedAt,
    IReadOnlyList<ChatMessageDto> Transcript,
    string? Rubric
);
