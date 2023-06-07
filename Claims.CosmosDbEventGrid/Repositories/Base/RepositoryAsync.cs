using Claims.CosmosDbEventGrid.Contexts;
using Claims.CosmosDbEventGrid.Entities.Base;
using Claims.CosmosDbEventGrid.Interfaces.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.CosmosDbEventGrid.Repositories.Base
{
    public class RepositoryAsync<T, TId> : IRepositoryAsync<T, TId> where T : class, IEntity<TId>
    {
        protected readonly AuditContext _auditContext;

        public RepositoryAsync(AuditContext auditContext)
        {
            _auditContext = auditContext;
        }

        public IQueryable<T> Entities => _auditContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _auditContext.Set<T>().AddAsync(entity);
            await _auditContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _auditContext.Set<T>().Remove(entity);
            await _auditContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _auditContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            return await _auditContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            T exists = _auditContext.Set<T>().Find(entity.Id);
            if (exists == null)
                return;
            _auditContext.Entry(exists).CurrentValues.SetValues(entity);
            await _auditContext.SaveChangesAsync();
        }
    }
}
