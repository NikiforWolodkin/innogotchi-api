using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Models;

namespace innogotchi_api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, UserDto>();
        }
    }
}
