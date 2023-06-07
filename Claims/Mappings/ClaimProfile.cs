using AutoMapper;
using Claims.Application.Requests.Claim;
using Claims.Application.Responses;
using Claims.Core.Entities;

namespace Claims.Infrastructure.Mappings
{
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<CreateClaimRequest, Claim>();
            CreateMap<Claim, ClaimResponse>();
        }
    }
}
