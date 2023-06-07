using Claims.Core.Entities.Base;

namespace Claims.Core.Repository.Base
{
    public interface ICosmosDbRepositoryAsync<T> where T : ICosmosDbEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetItemAsync(string id);

        Task<T> AddItemAsync(T item);

        Task<T> UpdateItemAsync(T item);

        Task DeleteItemAsync(string id);

    }
}
