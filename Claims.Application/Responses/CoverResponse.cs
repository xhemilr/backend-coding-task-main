using Claims.Core.Enums;
using Newtonsoft.Json;

namespace Claims.Application.Responses
{
    public class CoverResponse
    {
        public string Id { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public CoverType Type { get; set; }

        public decimal Premium { get; set; }
    }
}
