using Claims.Core.Entities.Base;

namespace Claims.Core.Interfaces
{
    public interface IContainerContext<T> where T : ICosmosDbEntity
    {
        string ContainerName { get; }
        string GenerateId(string entityId);
    }
}
