using Claims.CosmosDbEventGrid.Entities;
using Claims.CosmosDbEventGrid.Interfaces.Repository.Base;

namespace Claims.CosmosDbEventGrid.Interfaces.Repository
{
    public interface IClaimAuditRepository : IRepositoryAsync<ClaimAudit, int>
    {
    }
}
