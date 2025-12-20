namespace WellInsightEngine.Infrastructure.Services.Auth.Options;

public sealed record KlerkAuthorizationOptions
{
    public string Issuer { get; init; } = string.Empty;
    public string[] AuthorizedParties { get; init; } = [];
    public string[] AllowedEmails { get; init; } = [];
    public bool Disabled { get; init; } = false;
}