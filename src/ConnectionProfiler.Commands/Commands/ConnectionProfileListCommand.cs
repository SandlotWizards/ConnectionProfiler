using ConnectionProfiler.Core.Enums;
using ConnectionProfiler.Core.Interfaces;
using SandlotWizards.CommandLineParser.Core;
using System;
using System.Threading.Tasks;

namespace SandlotWizards.ConnectionProfiler.Commands.Commands;

public class ConnectionProfileListCommand : ICommand
{
    public async Task ExecuteAsync(CommandContext context)
    {
        var provider = context.Resolve<IConnectionProfileProvider>();
        var environment = Enum.Parse<EnvironmentName>(context.Arguments["environment"], ignoreCase: true);

        var profiles = await provider.GetProfilesAsync(environment);

        if (profiles.Count == 0)
        {
            Console.WriteLine($"No connection profiles found for environment '{environment}'.");
            return;
        }

        Console.WriteLine($"Connection profiles for environment '{environment}':\n");
        foreach (var profile in profiles)
        {
            Console.WriteLine($"- {profile.Name} [{profile.ProfileType}]");
        }
    }
}
