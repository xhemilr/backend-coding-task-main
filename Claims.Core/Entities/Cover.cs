using Claims.Core.Entities.Base;
using Claims.Core.Enums;
using Newtonsoft.Json;

namespace Claims.Core.Entities
{
    public class Cover : ICosmosDbEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateOnly StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateOnly EndDate { get; set; }

        [JsonProperty(PropertyName = "claimType")]
        public CoverType Type { get; set; }

        [JsonProperty(PropertyName = "premium")]
        public decimal Premium { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; StartDate: {StartDate}; EndDate: {EndDate}; Type: {Type}; Premium: {Premium}";
        }
    }
}