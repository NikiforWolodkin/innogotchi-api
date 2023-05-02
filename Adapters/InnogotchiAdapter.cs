using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Helpers;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using Microsoft.AspNetCore.Identity;

namespace innogotchi_api.Services
{
    public class InnogotchiAdapter : IInnogotchiAdapter
    {
        private readonly IInnogotchiRepository _innogotchiRepository;
        private readonly IFarmRepository _farmRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public InnogotchiAdapter(IInnogotchiRepository innogotchiRepository, IFarmRepository farmRepository, IUserRepository userRepository, IMapper mapper)
        {
            _innogotchiRepository = innogotchiRepository;
            _farmRepository = farmRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public ICollection<InnogotchiResponseDto> GetInnogotchis()
        {
            IList<Innogotchi> innogotchis = _innogotchiRepository.GetInnogotchis().ToList();

            IList<InnogotchiResponseDto> innogotchisDto = _mapper.Map<IList<InnogotchiResponseDto>>(innogotchis);

            for (int i = 0; i < innogotchisDto.Count; i++)
            {
                innogotchisDto[i].Age = _innogotchiRepository.GetInnogotchiAge(innogotchis[i]);
                innogotchisDto[i].IsDead= _innogotchiRepository.IsInnogotchiDead(innogotchis[i]);
                innogotchisDto[i].HungerLevel = _innogotchiRepository
                    .GetInnogotchiHungerLevel(innogotchis[i]);
                innogotchisDto[i].ThirstLevel = _innogotchiRepository
                    .GetInnogotchiThirstLevel(innogotchis[i]);
                innogotchisDto[i].HappinessDayCount= _innogotchiRepository
                    .GetInnogotchiHappinessDayCount(innogotchis[i]);
            }

            return innogotchisDto;
        }

        public ICollection<InnogotchiResponseDto> GetInnogotchis(string farmName)
        {
            IList<Innogotchi> innogotchis = _innogotchiRepository.GetInnogotchis(farmName).ToList();

            IList<InnogotchiResponseDto> innogotchisDto = _mapper.Map<IList<InnogotchiResponseDto>>(innogotchis);

            for (int i = 0; i < innogotchisDto.Count; i++)
            {
                innogotchisDto[i].Age = _innogotchiRepository.GetInnogotchiAge(innogotchis[i]);
                innogotchisDto[i].IsDead = _innogotchiRepository.IsInnogotchiDead(innogotchis[i]);
                innogotchisDto[i].HungerLevel = _innogotchiRepository
                    .GetInnogotchiHungerLevel(innogotchis[i]);
                innogotchisDto[i].ThirstLevel = _innogotchiRepository
                    .GetInnogotchiThirstLevel(innogotchis[i]);
                innogotchisDto[i].HappinessDayCount = _innogotchiRepository
                    .GetInnogotchiHappinessDayCount(innogotchis[i]);
            }

            return innogotchisDto;
        }

        public InnogotchiResponseDto GetInnogotchi(string name)
        {
            Innogotchi innogotchi = _innogotchiRepository.GetInnogotchi(name);

            InnogotchiResponseDto innogotchiDto = _mapper.Map<InnogotchiResponseDto>(innogotchi);

            innogotchiDto.Age = _innogotchiRepository.GetInnogotchiAge(innogotchi);
            innogotchiDto.IsDead = _innogotchiRepository.IsInnogotchiDead(innogotchi);
            innogotchiDto.HungerLevel = _innogotchiRepository
                .GetInnogotchiHungerLevel(innogotchi);
            innogotchiDto.ThirstLevel = _innogotchiRepository
                .GetInnogotchiThirstLevel(innogotchi);
            innogotchiDto.HappinessDayCount = _innogotchiRepository
                .GetInnogotchiHappinessDayCount(innogotchi);

            return innogotchiDto;
        }

        public bool InnogotchiExists(string name) 
        {
            return _innogotchiRepository.InnogotchiExists(name);
        }

        public InnogotchiResponseDto AddInnogotchi(InnogotchiRequestDto request, string farmName)
        {
            Innogotchi innogotchi = new Innogotchi();

            innogotchi.Name = request.Name;
            innogotchi.CreationDate = DateTime.Now;

            FeedingAndQuenching feedingAndQuenching = new FeedingAndQuenching();

            feedingAndQuenching.FeedingTime = DateTime.Now;
            feedingAndQuenching.QuenchingTime = DateTime.Now;
            feedingAndQuenching.FeedingPeriod = 0;
            feedingAndQuenching.QuenchingPeriod = 0;
            feedingAndQuenching.UnhappyDays = 0;

            Farm farm = _farmRepository.GetFarm(farmName);

            _innogotchiRepository.AddInnogotchi(innogotchi, feedingAndQuenching, farm);

            return GetInnogotchi(request.Name);
        }

        public InnogotchiResponseDto AddInnogotchiFeedingAndQuenching(string name, string action)
        {
            Innogotchi innogotchi = _innogotchiRepository.GetInnogotchi(name);

            FeedingAndQuenching lastFeedingAndQuenching = innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .FirstOrDefault();


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

            if (action == "FEED")
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

            return GetInnogotchi(innogotchi.Name);
        }

        public bool CanFeedInnogotchi(string name, string userEmail)
        {
            User user = _userRepository.GetUser(userEmail);

            if (user.Farm.Innogotchis.Select(i => i.Name).Contains(name))
            {
                return true;
            }

            IList<Farm> collaborations = _farmRepository.GetFarms(userEmail).ToList();

            foreach (var farm in collaborations)
            {
                if (farm.Innogotchis.Select(i => i.Name).Contains(name))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
