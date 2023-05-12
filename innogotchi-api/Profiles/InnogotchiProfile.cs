using AutoMapper;
using BusinessLayer.ResponseDtos;
using DataLayer.Models;
using ServiceLayer.Dtos;

namespace ClientLayer.Profiles
{
    public class InnogotchiProfile : Profile
    {
        public InnogotchiProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.AvatarId, opt => opt.MapFrom(
                    src => src.Avatar != null ? src.Avatar.Id : (Guid?)null))
                .ForMember(dest => dest.FarmName, opt => opt.MapFrom(
                    src => src.Farm != null ? src.Farm.Name: null));

            CreateMap<Farm, FarmDto>();

            CreateMap<Innogotchi, InnogotchiDto>();
        }
    }
}
