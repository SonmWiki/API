namespace WebApi.Settings;

public record CorsSettings
{
    public const string SectionName = "Cors";

    public bool Enabled { get; init; }
    public required string[] AllowedOrigins { get; init; }
}