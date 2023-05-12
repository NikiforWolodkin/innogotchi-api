using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;
using DataLayer.Interfaces;
using DataLayer.Models;
using ServiceLayer.Exceptions;

namespace BusinessLayer.Services
{
    public class InnogotchiService : IInnogotchiService
    {
        private readonly IInnogotchiRepository _innogotchiRepository;
        private readonly IFarmRepository _farmRepository;
        private readonly IMapper _mapper;

        public InnogotchiService(IInnogotchiRepository innogotchiRepository, IFarmRepository farmRepository, IMapper mapper)
        {
            _innogotchiRepository = innogotchiRepository;
            _farmRepository = farmRepository;
            _mapper = mapper;
        }

        public async Task<string> AddInnogotchiAsync(InnogotchiAddDto request, string farmName)
        {
            Farm farm = await _farmRepository.GetFarmAsync(farmName);

            Innogotchi innogotchi = new Innogotchi
            {
                Name = request.Name,
            };

            innogotchi = _innogotchiRepository.AddInnogotchi(innogotchi, farm);

            return innogotchi.Name;
        }

        public async Task<InnogotchiDto> FeedOrQuenchInnogotchiAsync(string name, InnogotchiFeedOrQuenchDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<InnogotchiDto> GetInnogotchiAsync(string name)
        {
            Innogotchi innogotchi = await _innogotchiRepository.GetInnogotchiAsync(name)
                ?? throw new NotFoundException("Innogotchi not found.");

            return _mapper.Map<InnogotchiDto>(innogotchi);
        }

        public async Task<ICollection<InnogotchiDto>> GetInnogotchisAsync()
        {
            ICollection<Innogotchi> innogotchis = await _innogotchiRepository.GetInnogotchisAsync(); 

            return _mapper.Map<ICollection<InnogotchiDto>>(innogotchis);
        }

        public async Task<ICollection<InnogotchiDto>> GetInnogotchisAsync(string farmName)
        {
            Farm farm = await _farmRepository.GetFarmAsync(farmName)
                ?? throw new NotFoundException("Farm not found.");

            ICollection<Innogotchi> innogotchis = await _innogotchiRepository.GetInnogotchisAsync(farmName);

            return _mapper.Map<ICollection<InnogotchiDto>>(innogotchis);
        }
    }
}
