using System;
using Claims.CosmosDbEventGrid.Entities.Base;

namespace Claims.CosmosDbEventGrid.Entities
{
    public class CoverAudit : IEntity<int>
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public string CoverId { get; set; }

        public string HttpRequestType { get; set; }
    }
}
