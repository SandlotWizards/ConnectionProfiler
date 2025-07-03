using ConnectionProfiler.Core.Enums;
using ConnectionProfiler.Core.Interfaces;
using ConnectionProfiler.Core.Models;
using SandlotWizards.CommandLineParser.Core;
using System;
using System.Threading.Tasks;

namespace SandlotWizards.ConnectionProfiler.Commands;

public class ConnectionProfileAddCommand : ICommand
{
    public async Task ExecuteAsync(CommandContext context)
    {
        var provider = context.Resolve<IConnectionProfileProvider>();

        var name = context.Arguments["name"];
        var environment = Enum.Parse<EnvironmentName>(context.Arguments["environment"], ignoreCase: true);
        var type = Enum.Parse<ConnectionProfileType>(context.Arguments["type"], ignoreCase: true);

        ConnectionProfile profile = type switch
        {
            ConnectionProfileType.Sql => new SqlConnectionProfile
            {
                Name = name,
                Environment = environment,
                Server = context.Arguments["server"],
                Database = context.Arguments["database"],
                UserId = context.Arguments["user"],
                Password = context.Arguments["password"]
            },
            ConnectionProfileType.GitHub => new GitHubConnectionProfile
            {
                Name = name,
                Environment = environment,
                Organization = context.Arguments["organization"],
                Repository = context.Arguments["repository"],
                PersonalAccessToken = context.Arguments["pat"]
            },
            _ => throw new ArgumentException("Unsupported profile type")
        };

        await provider.SaveProfileAsync(profile);
        Console.WriteLine($"Connection profile '{name}' added.");
    }
}
