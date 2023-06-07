using AutoMapper;
using Claims.Application.Requests.Cover;
using Claims.Application.Responses;
using Claims.Core.Entities;

namespace Claims.Infrastructure.Mappings
{
    public class CoverProfile : Profile
    {
        public CoverProfile()
        {
            CreateMap<CreateCoverRequest, Cover>();
            CreateMap<Cover, CoverResponse>();
        }
    }
}
