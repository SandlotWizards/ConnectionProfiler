using ConnectionProfiler.Core.Interfaces;
using ConnectionProfiler.LocalHost.Credentials;
using System.Runtime.InteropServices;

namespace ConnectionProfiler.LocalHost.Services
{
    public class CredentialManagerSecretResolver : ISecretResolver
    {
        public Task<string> ResolveAsync(string secretKey)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException("Credential Manager is only supported on Windows.");

            var credential = CredentialManager.GetCredential(secretKey);
            return Task.FromResult(credential?.Password ?? string.Empty);
        }
    }
}
