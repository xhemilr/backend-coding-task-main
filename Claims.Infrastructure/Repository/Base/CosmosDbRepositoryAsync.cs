using Claims.Core.Entities.Base;
using Claims.Core.Repository.Base;
using Claims.Core.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Claims.Infrastructure.Repository.Base
{
    public abstract class CosmosDbRepositoryAsync<T> : ICosmosDbRepositoryAsync<T>, IContainerContext<T> where T : ICosmosDbEntity
    {
        public abstract string ContainerName { get; }

        public abstract string GenerateId(string entity);

        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly Container _container;

        public CosmosDbRepositoryAsync(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            _cosmosDbContainerFactory = cosmosDbContainerFactory;
            _container = _cosmosDbContainerFactory.GetContainer(ContainerName)._container;
        }

        public async Task<T> AddItemAsync(T item)
        {
            item.Id = GenerateId();
            return await _container.CreateItemAsync(item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, ResolvePartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var query = @$"SELECT * FROM c";
            FeedIterator<T> resultSetIterator = _container.GetItemQueryIterator<T>(new QueryDefinition(query));
            List<T> results = new List<T>();
            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<T> response = await resultSetIterator.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            var result = await _container.UpsertItemAsync(item, ResolvePartitionKey(item.Id));
            return result;
        }

        public string GenerateId() => Guid.NewGuid().ToString();

        public PartitionKey ResolvePartitionKey(string id) => new PartitionKey(id);
    }
}
