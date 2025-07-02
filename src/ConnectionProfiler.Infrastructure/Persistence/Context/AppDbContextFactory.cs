using ConnectionProfiler.Infrastructure.Constants;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SandlotWizards.SharedKernel.Configuration;

namespace ConnectionProfiler.Infrastructure.Persistence.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var primaryDbConnectionConfig = new PrimaryDbConnectionConfig();
            configuration.GetSection(nameof(PrimaryDbConnectionConfig)).Bind(primaryDbConnectionConfig);

            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = primaryDbConnectionConfig.ServerName,
                InitialCatalog = InfrastructureConstants.AppDbCollectionName,
                UserID = primaryDbConnectionConfig.UserName,
                Password = primaryDbConnectionConfig.Password,
                TrustServerCertificate = true,
                PersistSecurityInfo = true
            }.ToString();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var dbContext = new AppDbContext(optionsBuilder.Options);
            return dbContext;
        }
    }
}