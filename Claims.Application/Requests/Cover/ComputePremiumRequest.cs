using Claims.Core.Enums;

namespace Claims.Application.Requests.Cover
{
    public class ComputePremiumRequest
    {
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public CoverType CoverType{ get; set; }
    }
}
