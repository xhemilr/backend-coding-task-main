using Claims.Application.Requests.Cover;
using Claims.Application.Responses;
using Claims.Core.Enums;

namespace Claims.Application.Services
{
    public interface ICoverService
    {
        Task<decimal> ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType);

        Task<IEnumerable<CoverResponse>> GetAllAsync();

        Task<CoverResponse> GetByIdAsync(string id);

        Task<CoverResponse> CreateAsync(CreateCoverRequest request);

        Task DeleteAsync(string id);
    }
}
