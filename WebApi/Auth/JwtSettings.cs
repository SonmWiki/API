namespace WebApi.Auth;

public record JwtSettings
{
    public const string SectionName = "Jwt";

    public required string Authority { get; init; }
    public required string Audience { get; init; }
    public required string Issuer { get; init; }
    public required bool RequireHttpsMetadata { get; init; }
    public required bool ValidateIssuer { get; init; }
    public required bool ValidateAudience { get; init; }
    public required bool ValidateLifetime { get; init; }
    public required bool ValidateIssuerSigningKey { get; init; }
    public required TimeSpan ClockSkew { get; init; }
}