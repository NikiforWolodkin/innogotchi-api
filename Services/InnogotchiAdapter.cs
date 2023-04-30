using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;

namespace innogotchi_api.Services
{
    public class InnogotchiAdapter : IInnogotchiAdapter
    {
        private readonly IInnogotchiRepository _innogotchiRepository;
        private readonly IMapper _mapper;

        public InnogotchiAdapter(IInnogotchiRepository innogotchiRepository, IMapper mapper)
        {
            _innogotchiRepository = innogotchiRepository;
            _mapper = mapper;
        }

        public ICollection<InnogotchiDto> GetInnogotchis()
        {
            IList<Innogotchi> innogotchis = _innogotchiRepository.GetInnogotchis().ToList();

            IList<InnogotchiDto> innogotchisDto = _mapper.Map<IList<InnogotchiDto>>(innogotchis);

            for (int i = 0; i < innogotchisDto.Count; i++)
            {
                innogotchisDto[i].Age = _innogotchiRepository.GetInnogotchiAge(innogotchis[i]);
                innogotchisDto[i].IsDead= _innogotchiRepository.IsInnogotchiDead(innogotchis[i]);
                //innogotchisDto[i].HungerLevel = _innogotchiRepository
                //    .GetInnogotchiHungerLevel(innogotchis[i]);
                //innogotchisDto[i].ThirstLevel= _innogotchiRepository
                //    .GetInnogotchiThirstLevel(innogotchis[i]);
                //innogotchisDto[i].HappinessDayCount= _innogotchiRepository
                //    .GetInnogotchiHappinessDayCount(innogotchis[i]);
            }

            return innogotchisDto;
        }

        public ICollection<InnogotchiDto> GetInnogotchis(string farmName)
        {
            IList<Innogotchi> innogotchis = _innogotchiRepository.GetInnogotchis(farmName).ToList();

            IList<InnogotchiDto> innogotchisDto = _mapper.Map<IList<InnogotchiDto>>(innogotchis);

            for (int i = 0; i < innogotchisDto.Count; i++)
            {
                innogotchisDto[i].Age = _innogotchiRepository.GetInnogotchiAge(innogotchis[i]);
                innogotchisDto[i].IsDead = _innogotchiRepository.IsInnogotchiDead(innogotchis[i]);
                //innogotchisDto[i].HungerLevel = _innogotchiRepository
                //    .GetInnogotchiHungerLevel(innogotchis[i]);
                //innogotchisDto[i].ThirstLevel= _innogotchiRepository
                //    .GetInnogotchiThirstLevel(innogotchis[i]);
                //innogotchisDto[i].HappinessDayCount= _innogotchiRepository
                //    .GetInnogotchiHappinessDayCount(innogotchis[i]);
            }

            return innogotchisDto;
        }

        public InnogotchiDto GetInnogotchi(string name)
        {
            Innogotchi innogotchi = _innogotchiRepository.GetInnogotchi(name);

            InnogotchiDto innogotchiDto = _mapper.Map<InnogotchiDto>(innogotchi);

            innogotchiDto.Age = _innogotchiRepository.GetInnogotchiAge(innogotchi);
            innogotchiDto.IsDead = _innogotchiRepository.IsInnogotchiDead(innogotchi);
            //innogotchiDto.HungerLevel = _innogotchiRepository
            //    .GetInnogotchiHungerLevel(innogotchi);
            //innogotchiDto.ThirstLevel = _innogotchiRepository
            //    .GetInnogotchiThirstLevel(innogotchi);
            //innogotchiDto.HappinessDayCount = _innogotchiRepository
            //    .GetInnogotchiHappinessDayCount(innogotchi);

            return innogotchiDto;
        }

        public bool InnogotchiExists(string name) 
        {
            return _innogotchiRepository.InnogotchiExists(name);
        }
    }
}
