﻿using Claims.Core.Entities.Base;
using Claims.Core.Enums;
using Newtonsoft.Json;

namespace Claims.Core.Entities
{
    public class Claim : ICosmosDbEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "coverId")]
        public string CoverId { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "claimType")]
        public ClaimType Type { get; set; }

        [JsonProperty(PropertyName = "damageCost")]
        public decimal DamageCost { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; CoverId: {CoverId}; Created: {Created}; Name: {Name}; Type: {Type}; DemageCost: {DamageCost}";
        }
    }
}
