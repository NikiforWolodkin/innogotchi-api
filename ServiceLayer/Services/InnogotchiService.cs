using AutoMapper;
using BusinessLayer.Enums;
using BusinessLayer.Interfaces;
using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;
using DataLayer.Interfaces;
using DataLayer.Models;
using InnogotchiApi.Helpers;
using DataLayer.Exceptions;

namespace BusinessLayer.Services
{
    public class InnogotchiService : IInnogotchiService
    {
        private readonly IInnogotchiRepository _innogotchiRepository;
        private readonly IFarmRepository _farmRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICollaborationRepository _collaborationRepository;
        private readonly IMapper _mapper;

        public InnogotchiService(IInnogotchiRepository innogotchiRepository, IFarmRepository farmRepository,
            IUserRepository userRepository, ICollaborationRepository collaborationRepository, IMapper mapper)
        {
            _innogotchiRepository = innogotchiRepository;
            _farmRepository = farmRepository;
            _userRepository = userRepository;
            _collaborationRepository = collaborationRepository;
            _mapper = mapper;
        }

        public async Task<string> AddInnogotchiAsync(InnogotchiAddDto request, string farmName)
        {
            Farm farm = await _farmRepository.GetFarmAsync(farmName);

            Innogotchi innogotchi = new Innogotchi
            {
                Name = request.Name,
                CreationDate = DateTime.Now,
            };

            innogotchi = _innogotchiRepository.AddInnogotchi(innogotchi, farm);

            FeedingAndQuenching feedingAndQuenching = new FeedingAndQuenching
            {
                FeedingTime = DateTime.Now,
                QuenchingTime = DateTime.Now,
                FeedingPeriod = 0,
                QuenchingPeriod = 0,
                UnhappyDays = 0
            };

            _innogotchiRepository.AddInnogotchiFeedingAndQuenching(innogotchi, feedingAndQuenching);

            return innogotchi.Name;
        }

        public async Task<bool> CanFeedInnogotchiAsync(string name, Guid userId)
        {
            Innogotchi innogotchi = await _innogotchiRepository.GetInnogotchiAsync(name);

            User user = await _userRepository.GetUserAsync(userId);

            try
            {
                Farm userFarm = await _farmRepository.GetFarmAsync(user.Farm?.Name);

                if (userFarm.Innogotchis.Any(inno => inno.Name == name))
                {
                    return true;
                }
            }
            catch { }

            var collaborations = await _collaborationRepository.GetCollaborationsAsync();

            var innogotchisToCheck = collaborations
                .Where(collab => collab.UserId == userId)
                .SelectMany(collab => collab.Farm.Innogotchis)
                .ToList();

            foreach (var inno in innogotchisToCheck)
            {
                if (inno.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<InnogotchiDto> FeedOrQuenchInnogotchiAsync(string name, InnogotchiFeedOrQuenchDto request)
        {
            Innogotchi innogotchi = await _innogotchiRepository.GetInnogotchiAsync(name);

            FeedingAndQuenching lastFeedingAndQuenching = innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .First();

            DateTime lastFeedingTime = lastFeedingAndQuenching.FeedingTime;
            DateTime lastQuenchingTime = lastFeedingAndQuenching.QuenchingTime;

            int feedingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastFeedingTime);
            int quenchingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastQuenchingTime);

            FeedingAndQuenching feedingAndQuenching = new FeedingAndQuenching();

            feedingAndQuenching.FeedingPeriod = feedingTimespan;
            feedingAndQuenching.QuenchingPeriod = quenchingTimespan;

            int unhappyDays = 0;

            if (feedingTimespan >= TimeConverter.NORMAL_FEEDING_THRESHOLD)
            {
                feedingTimespan = feedingTimespan - TimeConverter.NORMAL_FEEDING_THRESHOLD;
            }
            else
            {
                feedingTimespan = 0;
            }
            if (quenchingTimespan >= TimeConverter.NORMAL_QUENCHING_THRESHOLD)
            {
                quenchingTimespan = quenchingTimespan - TimeConverter.NORMAL_QUENCHING_THRESHOLD;
            }
            else
            {
                quenchingTimespan = 0;
            }

            unhappyDays = quenchingTimespan >= feedingTimespan ? quenchingTimespan : feedingTimespan;

            feedingAndQuenching.UnhappyDays = unhappyDays;

            if (request.Action == InnogotchiAction.Feed)
            {
                feedingAndQuenching.FeedingTime = DateTime.Now;
                feedingAndQuenching.QuenchingTime = lastQuenchingTime;
            }
            else
            {
                feedingAndQuenching.FeedingTime = lastFeedingTime;
                feedingAndQuenching.QuenchingTime = DateTime.Now;
            }

            _innogotchiRepository.AddInnogotchiFeedingAndQuenching(innogotchi, feedingAndQuenching);

            return _mapper.Map<InnogotchiDto>(innogotchi);
        }

        public async Task<InnogotchiDto> GetInnogotchiAsync(string name)
        {
            Innogotchi innogotchi = await _innogotchiRepository.GetInnogotchiAsync(name);

            return _mapper.Map<InnogotchiDto>(innogotchi);
        }

        public async Task<ICollection<InnogotchiDto>> GetInnogotchisAsync()
        {
            ICollection<Innogotchi> innogotchis = await _innogotchiRepository.GetInnogotchisAsync(); 

            return _mapper.Map<ICollection<InnogotchiDto>>(innogotchis);
        }

        public async Task<ICollection<InnogotchiDto>> GetInnogotchisAsync(string farmName)
        {
            Farm farm = await _farmRepository.GetFarmAsync(farmName);

            ICollection<Innogotchi> innogotchis = await _innogotchiRepository.GetInnogotchisAsync(farmName);

            return _mapper.Map<ICollection<InnogotchiDto>>(innogotchis);
        }
    }
}
