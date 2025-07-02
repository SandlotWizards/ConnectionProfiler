using ConnectionProfiler.Core.Enums;
using ConnectionProfiler.Core.Models;

namespace ConnectionProfiler.Core.Interfaces
{
    public interface IConnectionProfileProvider
    {
        Task<IReadOnlyList<ConnectionProfile>> GetProfilesAsync(EnvironmentName environment);
        Task<ConnectionProfile?> GetProfileAsync(string name, EnvironmentName environment);
        Task SaveProfileAsync(ConnectionProfile profile);
        Task DeleteProfileAsync(string name, EnvironmentName environment);
    }
}
