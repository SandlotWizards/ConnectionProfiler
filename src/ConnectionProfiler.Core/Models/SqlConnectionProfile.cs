using ConnectionProfiler.Core.Enums;

namespace ConnectionProfiler.Core.Models
{
    public record SqlConnectionProfile : ConnectionProfile
    {
        public override ConnectionProfileType ProfileType => ConnectionProfileType.Sql;
        public required string Server { get; init; }
        public required string Database { get; init; }
        public required string UserId { get; init; } // Secret
        public required string Password { get; init; } // Secret
    }
}
