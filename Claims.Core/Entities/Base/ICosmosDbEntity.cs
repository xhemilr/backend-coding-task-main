using Newtonsoft.Json;

namespace Claims.Core.Entities.Base
{
    public interface ICosmosDbEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
