using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;
using DataLayer.Interfaces;
using DataLayer.Models;
using ServiceLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class FarmService : IFarmService
    {
        private readonly IFarmRepository _farmRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FarmService(IFarmRepository farmRepository, IUserRepository userRepository, IMapper mapper)
        {
            _farmRepository = farmRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<string> AddFarmAsync(FarmAddDto request, Guid userId)
        {
            User user = await _userRepository.GetUserAsync(userId);

            Farm farm = new Farm()
            {
                Name = request.Name,
            };

            farm = _farmRepository.AddFarm(farm, user);

            return farm.Name;
        }

        public async void DeleteFarmAsync(string name)
        {
            Farm farm = await _farmRepository.GetFarmAsync(name);

            _farmRepository.DeleteFarm(farm);
        }

        public async Task<FarmDto> GetFarmAsync(string name)
        {
            Farm farm = await _farmRepository.GetFarmAsync(name)
                ?? throw new NotFoundException("Farm not found.");

            return _mapper.Map<FarmDto>(farm);
        }

        public async Task<ICollection<FarmDto>> GetFarmsAsync()
        {
            ICollection<Farm> farms = await _farmRepository.GetFarmsAsync();

            return _mapper.Map<ICollection<FarmDto>>(farms);
        }

        public async Task<ICollection<FarmDto>> GetFarmsAsync(Guid collaboratorId)
        {
            ICollection<Farm> farms = await _farmRepository.GetFarmsAsync(collaboratorId);

            return _mapper.Map<ICollection<FarmDto>>(farms);
        }
    }
}