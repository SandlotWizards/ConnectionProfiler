using ConnectionProfiler.Core.Enums;
using ConnectionProfiler.Core.Interfaces;
using ConnectionProfiler.Core.Models;
using ConnectionProfiler.Infrastructure.Models;
using ConnectionProfiler.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ConnectionProfiler.Infrastructure.Services
{
    public class EfCoreConnectionProfileProvider : IConnectionProfileProvider
    {
        private readonly AppDbContext _db; 
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        public EfCoreConnectionProfileProvider(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<ConnectionProfile>> GetProfilesAsync(EnvironmentName environment)
        {
            var entries = await _db.Profiles
                .Where(p => p.Environment == environment.ToString())
                .ToListAsync();

            return entries
                .Select(p => JsonSerializer.Deserialize<ConnectionProfile>(p.PayloadJson, _jsonOptions))
                .Where(p => p is not null)
                .Cast<ConnectionProfile>()
                .ToList();
        }

        public async Task<ConnectionProfile?> GetProfileAsync(string name, EnvironmentName environment)
        {
            var entry = await _db.Profiles
                .FirstOrDefaultAsync(p => p.Name == name && p.Environment == environment.ToString());

            if (entry == null) return null;

            return JsonSerializer.Deserialize<ConnectionProfile>(entry.PayloadJson, _jsonOptions);
        }

        public async Task SaveProfileAsync(ConnectionProfile profile)
        {
            var existing = await _db.Profiles
                .FirstOrDefaultAsync(p => p.Name == profile.Name && p.Environment == profile.Environment.ToString());

            var json = JsonSerializer.Serialize(profile, _jsonOptions);
            if (existing is null)
            {
                _db.Profiles.Add(new SerializedConnectionProfile
                {
                    Name = profile.Name,
                    Environment = profile.Environment.ToString(),
                    Type = profile.ProfileType.ToString(),
                    PayloadJson = json
                });
            }
            else
            {
                existing.Type = profile.ProfileType.ToString();
                existing.PayloadJson = json;
            }

            await _db.SaveChangesAsync();
        }

        public async Task DeleteProfileAsync(string name, EnvironmentName environment)
        {
            var entry = await _db.Profiles
                .FirstOrDefaultAsync(p => p.Name == name && p.Environment == environment.ToString());

            if (entry != null)
            {
                _db.Profiles.Remove(entry);
                await _db.SaveChangesAsync();
            }
        }
    }

}
