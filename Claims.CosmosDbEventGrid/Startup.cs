using Claims.CosmosDbEventGrid.Contexts;
using Claims.CosmosDbEventGrid.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;

[assembly: FunctionsStartup(typeof(Claims.CosmosDbEventGrid.Startup))]

namespace Claims.CosmosDbEventGrid
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            Serilog.Core.Logger logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.Console()
               .CreateLogger();
            services.AddLogging(lb => lb.AddSerilog(logger));

            services.ConfigureSqlDb(configuration);

            services.RegisterRepositories();
        }
    }
}
