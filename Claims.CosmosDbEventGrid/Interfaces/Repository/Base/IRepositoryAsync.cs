using Claims.CosmosDbEventGrid.Entities.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.CosmosDbEventGrid.Interfaces.Repository.Base
{
    public interface IRepositoryAsync<T, in TId> where T : class, IEntity<TId>
    {
        /// <summary>
        /// Returns entites as Quarable.
        /// </summary>
        IQueryable<T> Entities { get; }


        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Gets entity by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns T entity if found, null if not found.</returns>
        Task<T> GetByIdAsync(TId id);

        /// <summary>
        /// Adds new entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Returns T entity if successfully added, null if failed.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates existing entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(T entity);
    }
}
