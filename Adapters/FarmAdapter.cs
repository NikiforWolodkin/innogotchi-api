using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using Microsoft.AspNetCore.Identity;

namespace innogotchi_api.Services
{
    public class FarmAdapter : IFarmAdapter
    {
        private readonly IFarmRepository _farmRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FarmAdapter (IFarmRepository farmRepository, IUserRepository userRepository, IMapper mapper)
        {
            _farmRepository = farmRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public FarmResponseDto AddFarm(string name, string userEmail)
        {
            User user = _userRepository.GetUser(userEmail);

            Farm farm = new Farm();

            farm.Name = name;

            _farmRepository.AddFarm(user, farm);

            return GetFarm(farm.Name);
        }

        public void DeleteFarm(string name)
        {
            Farm farm = _farmRepository.GetFarm(name);

            _farmRepository.DeleteFarm(farm);
        }

        public CollaborationDto AddFarmCollaboration(string name, string collaboratorEmail)
        {
            Collaboration collaboration = new Collaboration();

            collaboration.FarmName = name;
            collaboration.UserEmail = collaboratorEmail;
            collaboration.Status = string.Empty;

            collaboration = _farmRepository.AddFarmCollaboration(collaboration);

            CollaborationDto response = _mapper.Map<CollaborationDto>(collaboration);

            return response;
        }

        public void DeleteFarmCollaboration(string name, string collaboratorEmail)
        {
            Collaboration collaboration =
                _farmRepository.GetFarmCollaboration(name, collaboratorEmail);

            _farmRepository.DeleteFarmCollaboration(collaboration);
        }

        public bool FarmCollaborationExists(string name, string collaboratorEmail)
        {
            return _farmRepository.FarmCollaborationExists(name, collaboratorEmail);
        }

        public bool FarmExists(string name)
        {
            return _farmRepository.FarmExists(name);
        }

        public FarmResponseDto GetFarm(string name)
        {
            Farm farm = _farmRepository.GetFarm(name);

            FarmResponseDto response = _mapper.Map<FarmResponseDto>(farm);

            response.AverageHappinessDayCount =
                _farmRepository.GetFarmAverageHappinessDayCount(farm);
            response.AverageQuenchingPeriod =
                _farmRepository.GetFarmAverageQuenchingPeriod(farm);
            response.AverageFeedingPeriod =
                _farmRepository.GetFarmAverageFeedingPeriod(farm);
            response.AveragePetAge =
                _farmRepository.GetFarmAveragePetAge(farm);
            response.AlivePetsCount = 
                _farmRepository.GetFarmAlivePetsCount(farm);
            response.DeadPetsCount =
                _farmRepository.GetFarmDeadPetsCount(farm);

            return response;
        }

        public ICollection<FarmResponseDto> GetFarms()
        {
            IList<Farm> farms = _farmRepository.GetFarms().ToList();
                
            IList<FarmResponseDto> response = _mapper.Map<IList<FarmResponseDto>>(farms);

            for (int i = 0; i < farms.Count(); i++) 
            {
                response[i].AverageHappinessDayCount =
                _farmRepository.GetFarmAverageHappinessDayCount(farms[i]);
                response[i].AverageQuenchingPeriod =
                    _farmRepository.GetFarmAverageQuenchingPeriod(farms[i]);
                response[i].AverageFeedingPeriod = 
                    _farmRepository.GetFarmAverageFeedingPeriod(farms[i]);
                response[i].AveragePetAge = 
                    _farmRepository.GetFarmAveragePetAge(farms[i]);
                response[i].AlivePetsCount = 
                    _farmRepository.GetFarmAlivePetsCount(farms[i]);
                response[i].DeadPetsCount = 
                    _farmRepository.GetFarmDeadPetsCount(farms[i]);
            }

            return response;
        }

        public ICollection<FarmResponseDto> GetFarms(string collaboratorName)
        {
            IList<Farm> farms = _farmRepository.GetFarms(collaboratorName).ToList();

            IList<FarmResponseDto> response = _mapper.Map<IList<FarmResponseDto>>(farms);

            for (int i = 0; i < farms.Count(); i++)
            {
                response[i].AverageHappinessDayCount =
                _farmRepository.GetFarmAverageHappinessDayCount(farms[i]);
                response[i].AverageQuenchingPeriod =
                    _farmRepository.GetFarmAverageQuenchingPeriod(farms[i]);
                response[i].AverageFeedingPeriod =
                    _farmRepository.GetFarmAverageFeedingPeriod(farms[i]);
                response[i].AveragePetAge =
                    _farmRepository.GetFarmAveragePetAge(farms[i]);
                response[i].AlivePetsCount =
                    _farmRepository.GetFarmAlivePetsCount(farms[i]);
                response[i].DeadPetsCount =
                    _farmRepository.GetFarmDeadPetsCount(farms[i]);
            }

            return response;
        }
    }
}
