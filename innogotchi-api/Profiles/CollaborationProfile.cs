using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Models;

namespace innogotchi_api.Profiles
{
    public class CollaborationProfile : Profile
    {
        public CollaborationProfile()
        {
            CreateMap<Collaboration, CollaborationDto>();
        }
    }
}
