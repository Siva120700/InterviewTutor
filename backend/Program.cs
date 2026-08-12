using InterviewTutor.Api;
using InterviewTutor.Api.Data;
using InterviewTutor.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("openai");

// Local default; cloud hosts set PORT / ASPNETCORE_URLS
if (builder.Environment.IsDevelopment()
    && string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"))
    && string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("PORT")))
{
    builder.WebHost.UseUrls("http://localhost:5080");
}
else if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("PORT"))
         && string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
{
    var port = Environment.GetEnvironmentVariable("PORT")!;
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

var connRaw = builder.Configuration.GetConnectionString("Default")
           ?? Environment.GetEnvironmentVariable("ConnectionStrings__Default")
           ?? "Host=localhost;Port=5433;Database=interviewtutor;Username=interviewtutor;Password=interviewtutor";
var conn = PgConnectionString.Normalize(connRaw);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));
builder.Services.AddSingleton<MarkdownLessonLoader>();
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<ProgressService>();
builder.Services.AddScoped<AiTutorService>();

var corsOrigins = (builder.Configuration["CORS_ORIGINS"]
                   ?? Environment.GetEnvironmentVariable("CORS_ORIGINS")
                   ?? "http://localhost:5173,http://127.0.0.1:5173")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("frontend", p =>
        p.WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    await ProblemSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("frontend");
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

// SPA fallback (React Router) — keep API routes above
app.MapFallbackToFile("index.html");

app.Run();
