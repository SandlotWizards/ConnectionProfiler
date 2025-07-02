using ConnectionProfiler.Core.Enums;

namespace ConnectionProfiler.Core.Models
{
    public record GitHubConnectionProfile : ConnectionProfile
    {
        public override ConnectionProfileType ProfileType => ConnectionProfileType.GitHub;
        public required string Organization { get; init; }
        public required string Repository { get; init; }
        public required string PersonalAccessToken { get; init; } // Secret
    }
}
