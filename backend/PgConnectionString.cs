using System.Text;

namespace InterviewTutor.Api;

public static class PgConnectionString
{
    /// <summary>
    /// Accepts Npgsql key=value strings or postgres/postgresql URIs (Neon/Render style).
    /// </summary>
    public static string Normalize(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new ArgumentException("Database connection string is empty.");

        raw = raw.Trim().Trim('"');

        if (!raw.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
            && !raw.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
        {
            // Ensure SSL for Neon-style hosts if user forgot it
            if (raw.Contains("neon.tech", StringComparison.OrdinalIgnoreCase)
                && !raw.Contains("SSL Mode", StringComparison.OrdinalIgnoreCase)
                && !raw.Contains("Ssl Mode", StringComparison.OrdinalIgnoreCase))
            {
                return raw.TrimEnd(';') + ";SSL Mode=Require;Trust Server Certificate=true";
            }

            return raw;
        }

        var uri = new Uri(raw);
        var userInfo = uri.UserInfo.Split(':', 2);
        var user = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
        var database = uri.AbsolutePath.Trim('/');
        if (string.IsNullOrEmpty(database))
            database = "neondb";

        var sslMode = "Require";
        string? channelBinding = null;
        var query = uri.Query.TrimStart('?');
        if (!string.IsNullOrEmpty(query))
        {
            foreach (var part in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var kv = part.Split('=', 2);
                var key = Uri.UnescapeDataString(kv[0]);
                var val = kv.Length > 1 ? Uri.UnescapeDataString(kv[1]) : "";
                if (key.Equals("sslmode", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(val))
                    sslMode = MapSslMode(val);
                else if (key.Equals("channel_binding", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(val))
                    channelBinding = MapChannelBinding(val);
            }
        }

        var port = uri.Port > 0 ? uri.Port : 5432;
        var sb = new StringBuilder();
        sb.Append($"Host={uri.Host};");
        sb.Append($"Port={port};");
        sb.Append($"Database={database};");
        sb.Append($"Username={user};");
        sb.Append($"Password={password};");
        sb.Append($"SSL Mode={sslMode};");
        sb.Append("Trust Server Certificate=true");
        if (channelBinding is not null)
            sb.Append($";Channel Binding={channelBinding}");
        return sb.ToString();
    }

    private static string MapSslMode(string value) => value.ToLowerInvariant() switch
    {
        "disable" => "Disable",
        "allow" => "Allow",
        "prefer" => "Prefer",
        "require" => "Require",
        "verify-ca" => "VerifyCA",
        "verify-full" => "VerifyFull",
        _ => "Require"
    };

    private static string MapChannelBinding(string value) => value.ToLowerInvariant() switch
    {
        "disable" => "Disable",
        "prefer" => "Prefer",
        "require" => "Require",
        _ => "Prefer"
    };
}
