using Claims.Application.Requests.Claim;
using Claims.Application.Responses;

namespace Claims.Application.Services
{
    public interface IClaimService
    {
        Task<IEnumerable<ClaimResponse>> GetAllAsync();

        Task<ClaimResponse> GetByIdAsync(string id);

        Task<ClaimResponse> CreateAsync(CreateClaimRequest request);

        Task DeleteAsync(string id);
    }
}
