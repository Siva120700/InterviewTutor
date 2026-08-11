using InterviewTutor.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTutor.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<DynamicLesson> DynamicLessons => Set<DynamicLesson>();
    public DbSet<LessonDraft> LessonDrafts => Set<LessonDraft>();
    public DbSet<LessonProgress> LessonProgress => Set<LessonProgress>();
    public DbSet<ChatThread> ChatThreads => Set<ChatThread>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Problem> Problems => Set<Problem>();
    public DbSet<MockSession> MockSessions => Set<MockSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DynamicLesson>()
            .HasIndex(x => x.Slug)
            .IsUnique();

        modelBuilder.Entity<LessonProgress>()
            .HasIndex(x => x.LessonId)
            .IsUnique();

        modelBuilder.Entity<Problem>()
            .HasIndex(x => x.Slug)
            .IsUnique();

        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.Thread)
            .WithMany(t => t.Messages)
            .HasForeignKey(m => m.ThreadId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
