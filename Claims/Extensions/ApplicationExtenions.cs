using Claims.Core.Interfaces;

namespace Claims.Extensions
{
    public static class ApplicationExtenions
    {
        public static void EnsureCosmosDbIsCreated(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ICosmosDbContainerFactory factory = serviceScope.ServiceProvider.GetService<ICosmosDbContainerFactory>();

                factory.EnsureDbSetupAsync().Wait();
            }
        }
    }
}
