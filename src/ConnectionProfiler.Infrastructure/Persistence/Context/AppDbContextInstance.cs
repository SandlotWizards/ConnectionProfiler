using Microsoft.EntityFrameworkCore;

namespace ConnectionProfiler.Infrastructure.Persistence.Context
{
    public static class AppDbContextInstance
    {
        public static AppDbContext CreateInstance(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new AppDbContext(optionsBuilder.Options);
        }

        public static AppDbContext CreateInMemoryInstance(string databaseName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName);
            var dbContext = new AppDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();
            dbContext.Database.EnsureDeleted();
            return dbContext;
        }
    }
}