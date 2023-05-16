using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.ResponseDtos;
using DataLayer.Models;
using DataLayer.Dtos;

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

            CreateMap<Farm, FarmDto>()
                .ForMember(dest => dest.AlivePetsCount, opt => opt.MapFrom(
                    src => FarmHelper.GetFarmAlivePetsCount(src)))
                .ForMember(dest => dest.DeadPetsCount, opt => opt.MapFrom(
                    src => FarmHelper.GetFarmDeadPetsCount(src)))
                .ForMember(dest => dest.AverageFeedingPeriod, opt => opt.MapFrom(
                    src => FarmHelper.GetFarmAverageFeedingPeriod(src)))
                .ForMember(dest => dest.AverageQuenchingPeriod, opt => opt.MapFrom(
                    src => FarmHelper.GetFarmAverageQuenchingPeriod(src)))
                .ForMember(dest => dest.AverageHappyDaysAmount, opt => opt.MapFrom(
                    src => FarmHelper.GetFarmAverageHappyDaysAmount(src)));

            CreateMap<Innogotchi, InnogotchiDto>()
                .ForMember(dest => dest.IsDead, opt => opt.MapFrom(
                    src => src.DeathDate != null ? true : false))
                .ForMember(dest => dest.HungerLevel, opt => opt.MapFrom(
                    src => InnogotchiHelper.GetInnogotchiHungerLevel(src)))
                .ForMember(dest => dest.ThirstLevel, opt => opt.MapFrom(
                    src => InnogotchiHelper.GetInnogotchiThirstLevel(src)))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(
                    src => InnogotchiHelper.GetInnogotchiAge(src)))
                .ForMember(dest => dest.HappyDays, opt => opt.MapFrom(
                    src => InnogotchiHelper.GetInnogotchiHappinessDayCount(src)));

            CreateMap<Collaboration, CollaborationDto>()
                .ForMember(dest => dest.CollaboratorId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CollaboratorFirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.CollaboratorLastName, opt => opt.MapFrom(src => src.User.LastName));

            CreateMap<Avatar, AvatarDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)));
        }
    }
}
