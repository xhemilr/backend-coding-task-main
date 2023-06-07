using Claims.CosmosDbEventGrid.Contexts;
using Claims.CosmosDbEventGrid.Entities;
using Claims.CosmosDbEventGrid.Interfaces.Repository;
using Claims.CosmosDbEventGrid.Repositories.Base;

namespace Claims.CosmosDbEventGrid.Repositories
{
    public class CoverAuditRepository : RepositoryAsync<CoverAudit, int>, ICoverAuditRepository
    {
        public CoverAuditRepository(AuditContext auditContext) : base(auditContext)
        {

        }
    }
}
