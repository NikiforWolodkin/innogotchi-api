using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;

namespace innogotchi_api.Services
{
    public class FarmAdapter : IFarmAdapter
    {
        private readonly IFarmRepository _farmRepository;
        private readonly IMapper _mapper;

        public FarmAdapter (IFarmRepository farmRepository, IMapper mapper)
        {
            _farmRepository = farmRepository;
            _mapper = mapper;
        }

        public ICollection<FarmDto> GetFarms()
        {
            ICollection<FarmDto> farms =
                _mapper.Map<ICollection<FarmDto>>(_farmRepository.GetFarms());

            return farms;
        }

        public ICollection<FarmDto> GetFarms(string collaboratorName)
        {
            throw new NotImplementedException();
        }

        public FarmDto GetFarm(string name)
        {
            FarmDto farm = _mapper.Map<FarmDto>(_farmRepository.GetFarm(name));

            return farm;
        }

        public bool FarmExists(string name)
        {
            return _farmRepository.FarmExists(name);
        }
    }
}
