namespace InterviewTutor.Api.Models;

public class DynamicLesson
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Slug { get; set; } = "";
    public string Title { get; set; } = "";
    public string TrackSlug { get; set; } = "";
    public string Module { get; set; } = "Custom";
    public int Order { get; set; }
    public string Summary { get; set; } = "";
    public string MarkdownBody { get; set; } = "";
    public string LanguagesJson { get; set; } = "[]";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class LessonDraft
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    public string TrackSlug { get; set; } = "";
    public string Module { get; set; } = "Custom";
    public string Summary { get; set; } = "";
    public string MarkdownBody { get; set; } = "";
    public string Status { get; set; } = "pending"; // pending | confirmed | cancelled
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class LessonProgress
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string LessonId { get; set; } = "";
    public bool Completed { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class ChatThread
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string LessonId { get; set; } = "";
    public string? ProblemId { get; set; }
    public string Title { get; set; } = "Doubt chat";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<ChatMessage> Messages { get; set; } = [];
}

public class ChatMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ThreadId { get; set; }
    public ChatThread? Thread { get; set; }
    public string Role { get; set; } = "user"; // user | assistant | system
    public string Content { get; set; } = "";
    public string? MetaJson { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Problem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Slug { get; set; } = "";
    public string Title { get; set; } = "";
    public string Difficulty { get; set; } = "Medium";
    public string TrackSlug { get; set; } = "dsa";
    public string PromptMarkdown { get; set; } = "";
    public string JavaSolution { get; set; } = "";
    public string CsharpSolution { get; set; } = "";
    public string ComplexityNotes { get; set; } = "";
    public int Order { get; set; }
}

public class MockSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Mode { get; set; } = "hld"; // hld | cs | behavioral
    public int DurationMinutes { get; set; } = 30;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; }
    public string TranscriptJson { get; set; } = "[]";
    public string? RubricJson { get; set; }
}
