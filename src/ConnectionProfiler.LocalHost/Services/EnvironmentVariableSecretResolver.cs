using ConnectionProfiler.Core.Interfaces;

namespace ConnectionProfiler.LocalHost.Services;

public class EnvironmentVariableSecretResolver : ISecretResolver
{
    public Task<string> ResolveAsync(string secretKey)
    {
        var value = Environment.GetEnvironmentVariable(secretKey);
        return Task.FromResult(value ?? string.Empty);
    }
}
