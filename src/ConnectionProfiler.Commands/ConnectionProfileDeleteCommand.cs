using ConnectionProfiler.Core.Enums;
using ConnectionProfiler.Core.Interfaces;
using SandlotWizards.CommandLineParser.Core;
using System;
using System.Threading.Tasks;

namespace SandlotWizards.ConnectionProfiler.Commands;

public class ConnectionProfileDeleteCommand : ICommand
{
    public async Task ExecuteAsync(CommandContext context)
    {
        var provider = context.Resolve<IConnectionProfileProvider>();

        var name = context.Arguments["name"];
        var environment = Enum.Parse<EnvironmentName>(context.Arguments["environment"], ignoreCase: true);

        var existing = await provider.GetProfileAsync(name, environment);
        if (existing is null)
        {
            Console.WriteLine($"Connection profile '{name}' not found in environment '{environment}'.");
            return;
        }

        await provider.DeleteProfileAsync(name, environment);
        Console.WriteLine($"Connection profile '{name}' deleted.");
    }
}
