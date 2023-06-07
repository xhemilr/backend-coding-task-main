using Claims.Application.Services;
using Claims.Core.Entities;
using Claims.Core.Interfaces;
using Claims.Core.Repository;
using Claims.Infrastructure.Configurations;
using Claims.Infrastructure.CosmosDbData;
using Claims.Infrastructure.Repository;
using Claims.Infrastructure.Services;
using Microsoft.Azure.Cosmos;
using System.Text.Json.Serialization;

namespace Claims.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        internal static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<IClaimRepository, ClaimRepository>()
                .AddTransient<ICoverRepository, CoverRepository>();
        }

        internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IClaimService, ClaimService>()
                .AddScoped<ICoverService, CoverService>()
                .AddSingleton<IDateTimeService, DateTimeService>()
                .AddSingleton<IPremiumCalculationService, PermiumCalculationService>();
        }

        internal static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration config)
        {
            var cosmosDbOptions = config.GetSection(nameof(CosmosDbConfig)).Get<CosmosDbConfig>();
            var cosmosClient = new CosmosClient(cosmosDbOptions.Account, cosmosDbOptions.Key);
            CosmosDbContainerFactory cosmosDbClientFactory = new CosmosDbContainerFactory(cosmosClient, cosmosDbOptions.DatabaseName, cosmosDbOptions.Containers);

            services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbClientFactory);
            return services;
        }

        internal static IServiceCollection RegisterHttpClints(this IServiceCollection services, IConfiguration config)
        {
            var autidUrlConfig = config.GetSection(nameof(AuditEnpointConfig)).Get<AuditEnpointConfig>();
            
            services.AddHttpClient(nameof(Claim), client =>
            {
                client.BaseAddress = new Uri(autidUrlConfig.ClaimAuditEndpoint);
                client.DefaultRequestHeaders.Add("aeg-event-type", "Notification");
            });

            services.AddHttpClient(nameof(Cover), client =>
            {
                client.BaseAddress = new Uri(autidUrlConfig.CoverAuditEndpoint);
                client.DefaultRequestHeaders.Add("aeg-event-type", "Notification");
            });
            return services;
        }
    }
}
