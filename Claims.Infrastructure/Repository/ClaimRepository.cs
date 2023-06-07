using Claims.Core.Entities;
using Claims.Core.Interfaces;
using Claims.Core.Repository;
using Claims.Infrastructure.Repository.Base;
using Microsoft.Extensions.Logging;

namespace Claims.Infrastructure.Repository
{
    public class ClaimRepository : CosmosDbRepositoryAsync<Claim>, IClaimRepository
    {
        public override string ContainerName { get; } = "Claim";

        public override string GenerateId(string entity) => Guid.NewGuid().ToString();

        public ClaimRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) : base(cosmosDbContainerFactory)
        {
        }
    }
}
