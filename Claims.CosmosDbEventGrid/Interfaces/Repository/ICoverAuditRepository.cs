using Claims.CosmosDbEventGrid.Entities;
using Claims.CosmosDbEventGrid.Interfaces.Repository.Base;

namespace Claims.CosmosDbEventGrid.Interfaces.Repository
{
    public interface ICoverAuditRepository : IRepositoryAsync<CoverAudit, int>
    {
    }
}
