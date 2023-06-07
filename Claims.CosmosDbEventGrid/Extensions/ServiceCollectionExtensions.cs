using Claims.CosmosDbEventGrid.Contexts;
using Claims.CosmosDbEventGrid.Interfaces.Repository;
using Claims.CosmosDbEventGrid.Interfaces.Repository.Base;
using Claims.CosmosDbEventGrid.Repositories;
using Claims.CosmosDbEventGrid.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.CosmosDbEventGrid.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureSqlDb(this IServiceCollection services, IConfiguration config)
        {
            return
                services.AddDbContext<AuditContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        }

        internal static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
                .AddTransient<IClaimAuditRepository, ClaimAuditRepository>()
                .AddTransient<ICoverAuditRepository, CoverAuditRepository>();
        }
    }
}
