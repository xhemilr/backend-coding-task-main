using Claims.Core.Entities;
using Claims.Core.Interfaces;
using Claims.Core.Repository;
using Claims.Infrastructure.Repository.Base;
using Microsoft.Extensions.Logging;

namespace Claims.Infrastructure.Repository
{
    public class CoverRepository : CosmosDbRepositoryAsync<Cover>, ICoverRepository
    {
        public override string ContainerName { get; } = "Cover";

        public override string GenerateId(string entity) => Guid.NewGuid().ToString();

        public CoverRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) : base(cosmosDbContainerFactory)
        {
        }
    }
}
