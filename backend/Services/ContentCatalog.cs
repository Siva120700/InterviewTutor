namespace InterviewTutor.Api.Services;

public static class ContentCatalog
{
    public static readonly IReadOnlyDictionary<string, (string Title, string Group, string Description)> Tracks =
        new Dictionary<string, (string, string, string)>(StringComparer.OrdinalIgnoreCase)
        {
            ["dsa"] = ("DSA", "Interview", "Foundations through advanced algorithms — pair with Practice → DSA Sheet"),
            ["dsa-patterns"] = ("DSA Patterns", "Interview", "Pattern templates + problem lists + NeetCode/Striver/Aditya YouTube"),
            ["lld"] = ("LLD", "Interview", "OOP, SOLID, and class design"),
            ["hld"] = ("HLD", "Interview", "System design, trade-offs, capacity"),
            ["senior-fs"] = ("Senior FS", "Interview", "APIs, caching, concurrency, leadership"),
            ["platform"] = ("Auth, Cache & Security", "Platform", "JWT, AuthN/AuthZ, caching, API security, and reliability"),
            ["cs-databases"] = ("Databases", "CS Basics", "Indexes, transactions, isolation, Redis"),
            ["cs-networking"] = ("Networking", "CS Basics", "TCP/HTTP, DNS/TLS, LB/CDN"),
            ["cs-os"] = ("Operating Systems", "CS Basics", "Processes, memory, sync, deadlocks"),
            ["java"] = ("Java", "Languages", "Core Java through Spring Boot interview topics"),
            ["dotnet"] = (".NET / C#", "Languages", "C#, async, ASP.NET Core, EF Core"),
            ["react"] = ("React", "Languages", "Basics through senior React — hooks, performance, architecture"),
        };
}
