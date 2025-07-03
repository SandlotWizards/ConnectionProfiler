using ConnectionProfiler.Core.Enums;
using ConnectionProfiler.Core.Interfaces;
using ConnectionProfiler.Core.Models;
using SandlotWizards.CommandLineParser.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandlotWizards.ConnectionProfiler.Commands;

public class ConnectionProfileUpdateCommand : ICommand
{
    public async Task ExecuteAsync(CommandContext context)
    {
        var provider = context.Resolve<IConnectionProfileProvider>();

        var name = context.Arguments["name"];
        var environment = Enum.Parse<EnvironmentName>(context.Arguments["environment"], ignoreCase: true);
        var existing = await provider.GetProfileAsync(name, environment);

        if (existing is null)
        {
            Console.WriteLine($"Connection profile '{name}' not found.");
            return;
        }

        ConnectionProfile updated = existing switch
        {
            SqlConnectionProfile sql => sql with
            {
                Server = context.Arguments.GetValueOrDefault("server", sql.Server),
                Database = context.Arguments.GetValueOrDefault("database", sql.Database),
                UserId = context.Arguments.GetValueOrDefault("user", sql.UserId),
                Password = context.Arguments.GetValueOrDefault("password", sql.Password)
            },
            GitHubConnectionProfile git => git with
            {
                Organization = context.Arguments.GetValueOrDefault("organization", git.Organization),
                Repository = context.Arguments.GetValueOrDefault("repository", git.Repository),
                PersonalAccessToken = context.Arguments.GetValueOrDefault("pat", git.PersonalAccessToken)
            },
            _ => existing
        };

        await provider.SaveProfileAsync(updated);
        Console.WriteLine($"Connection profile '{name}' updated.");
    }
}
