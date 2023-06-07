using Microsoft.Azure.Cosmos;

namespace Claims.Core.Interfaces
{
    public interface ICosmosDbContainer
    {
        Container _container { get; }
    }
}
