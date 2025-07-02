using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ConnectionProfiler.Core.Interfaces;

namespace ConnectionProfiler.Infrastructure.Services;

public class KeyVaultSecretResolver : ISecretResolver
{
    private readonly SecretClient _client;

    public KeyVaultSecretResolver(Uri vaultUri)
    {
        _client = new SecretClient(vaultUri, new DefaultAzureCredential());
    }

    public async Task<string> ResolveAsync(string secretKey)
    {
        try
        {
            KeyVaultSecret secret = await _client.GetSecretAsync(secretKey);
            return secret.Value;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}
