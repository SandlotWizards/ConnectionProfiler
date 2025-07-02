using ConnectionProfiler.Core.Enums;
using ConnectionProfiler.Core.Interfaces;
using ConnectionProfiler.Core.Models;
using System.Text.Json;

namespace ConnectionProfiler.LocalHost.Services;

public class FileBasedConnectionProfileProvider : IConnectionProfileProvider
{
    private readonly string _basePath;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public FileBasedConnectionProfileProvider(string basePath)
    {
        _basePath = basePath;
    }

    private string GetFilePath(EnvironmentName environment) =>
        Path.Combine(_basePath, $"{environment}.connectionprofiles.json");

    public async Task<IReadOnlyList<ConnectionProfile>> GetProfilesAsync(EnvironmentName environment)
    {
        var path = GetFilePath(environment);
        if (!File.Exists(path)) return [];

        using var stream = File.OpenRead(path);
        var profiles = await JsonSerializer.DeserializeAsync<List<ConnectionProfile>>(stream, _jsonOptions);
        return profiles ?? [];
    }

    public async Task<ConnectionProfile?> GetProfileAsync(string name, EnvironmentName environment)
    {
        var profiles = await GetProfilesAsync(environment);
        return profiles.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task SaveProfileAsync(ConnectionProfile profile)
    {
        var path = GetFilePath(profile.Environment);
        var profiles = (await GetProfilesAsync(profile.Environment)).ToList();
        profiles.RemoveAll(p => p.Name.Equals(profile.Name, StringComparison.OrdinalIgnoreCase));
        profiles.Add(profile);

        using var stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, profiles, _jsonOptions);
    }

    public async Task DeleteProfileAsync(string name, EnvironmentName environment)
    {
        var path = GetFilePath(environment);
        var profiles = (await GetProfilesAsync(environment)).ToList();
        profiles.RemoveAll(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        using var stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, profiles, _jsonOptions);
    }
}
