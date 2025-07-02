using ConnectionProfiler.Core.Enums;

namespace ConnectionProfiler.Core.Models
{
    public abstract record ConnectionProfile
    {
        public required string Name { get; init; }
        public required EnvironmentName Environment { get; init; }
        public abstract ConnectionProfileType ProfileType { get; }
    }
}
