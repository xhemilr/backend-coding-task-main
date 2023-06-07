using Claims.Core.Interfaces;
using Claims.Infrastructure.Configurations;
using Microsoft.Azure.Cosmos;

namespace Claims.Infrastructure.CosmosDbData
{
    public class CosmosDbContainerFactory : ICosmosDbContainerFactory
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseName;
        private readonly List<ContainerInfo> _containers;

        public CosmosDbContainerFactory(CosmosClient cosmosClient, string databaseName, List<ContainerInfo> containers)
        {
            _cosmosClient = cosmosClient;
            _databaseName = databaseName;
            _containers = containers;
        }

        public async Task EnsureDbSetupAsync()
        {
            DatabaseResponse database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseName);

            foreach (ContainerInfo container in _containers)
            {
                await database.Database.CreateContainerIfNotExistsAsync(container.Name, $"{container.PartitionKey}");
            }
        }

        public ICosmosDbContainer GetContainer(string containerName)
        {
            if (_containers.Where(x => x.Name == containerName) == null)
            {
                throw new ArgumentException($"Unable to find container: {containerName}");
            }

            return new CosmosDbContainer(_cosmosClient, _databaseName, containerName);
        }
    }
}
