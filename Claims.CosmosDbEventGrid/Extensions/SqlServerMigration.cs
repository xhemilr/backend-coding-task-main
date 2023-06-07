using Claims.CosmosDbEventGrid.Extensions;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Claims.CosmosDbEventGrid.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.WebJobs.Description;

[assembly: WebJobsStartup(typeof(SqlServerMigration), "DbMigration")]

namespace Claims.CosmosDbEventGrid.Extensions
{
    /// <summary>
    /// Background task for injecting DB Initializer and Migrate.
    /// </summary>
    public class SqlServerMigration : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<DbInitializer>();
        }
    }

    [Extension("Migrate")]
    internal class DbInitializer : IExtensionConfigProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AuditContext>();

            dbContext.Database.Migrate();
        }
    }
}
