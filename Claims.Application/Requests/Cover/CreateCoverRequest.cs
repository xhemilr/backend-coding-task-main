using Claims.Core.Enums;

namespace Claims.Application.Requests.Cover
{
    public class CreateCoverRequest
    {
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public CoverType Type { get; set; }

        public decimal Premium { get; set; }
    }
}
