using Claims.Core.Entities;
using Claims.Core.Repository.Base;

namespace Claims.Core.Repository
{
    public interface IClaimRepository : ICosmosDbRepositoryAsync<Claim>
    {

    }
}
