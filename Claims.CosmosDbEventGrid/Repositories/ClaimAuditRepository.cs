using Claims.CosmosDbEventGrid.Contexts;
using Claims.CosmosDbEventGrid.Entities;
using Claims.CosmosDbEventGrid.Interfaces.Repository;
using Claims.CosmosDbEventGrid.Repositories.Base;

namespace Claims.CosmosDbEventGrid.Repositories
{
    public class ClaimAuditRepository : RepositoryAsync<ClaimAudit, int>, IClaimAuditRepository
    {
        public ClaimAuditRepository(AuditContext auditContext) : base(auditContext)
        {

        }
    }
}
