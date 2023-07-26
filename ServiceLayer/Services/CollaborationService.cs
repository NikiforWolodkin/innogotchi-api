using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;
using DataLayer.Enums;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Exceptions;

namespace BusinessLayer.Services
{
    public class CollaborationService : ICollaborationService
    {
        private readonly ICollaborationRepository _collaborationRepository;
        private readonly IFarmRepository _farmRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CollaborationService(ICollaborationRepository collaborationRepository, IFarmRepository farmRepository,
            IUserRepository userRepository, IMapper mapper)
        {
            _collaborationRepository = collaborationRepository;
            _farmRepository = farmRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task AddCollaborationAsync(string farmName, CollaborationAddDto request)
        {
            User user = await _userRepository.GetUserAsync(request.CollaboratorId);

            Farm farm = await _farmRepository.GetFarmAsync(farmName);

            Collaboration collaboration = new Collaboration
            {
                UserId = user.Id,
                FarmName = farm.Name,
            };

            await _collaborationRepository.AddCollaborationAsync(collaboration);
        }

        public async Task<CollaborationDto> ChangeCollaborationStatusAsync(string farmName, Guid userId, CollaborationChangeStatusDto request)
        {
            User user = await _userRepository.GetUserAsync(userId);

            Farm farm = await _farmRepository.GetFarmAsync(farmName);

            Collaboration collaboration = await _collaborationRepository.GetCollaborationAsync(user.Id, farm.Name);

            collaboration.Status = request.Status;

            await _collaborationRepository.UpdateDatabaseAsync();

            return _mapper.Map<CollaborationDto>(collaboration);
        }

        public async Task<ICollection<CollaborationDto>> GetAcceptedCollaborationsAsync(string farmName)
        {
            Farm farm = await _farmRepository.GetFarmAsync(farmName);

            ICollection<Collaboration> collaborations = await _collaborationRepository
                .GetCollaborationsAsync(farm.Name, CollaborationStatus.Accepted);

            return _mapper.Map<ICollection<CollaborationDto>>(collaborations);
        }

        public async Task<ICollection<CollaborationDto>> GetAcceptedCollaborationsAsync(Guid userId)
        {
            User user = await _userRepository.GetUserAsync(userId);

            ICollection<Collaboration> collaborations = await _collaborationRepository
                .GetCollaborationsAsync(user.Id, CollaborationStatus.Accepted);

            return _mapper.Map<ICollection<CollaborationDto>>(collaborations);
        }

        public async Task<ICollection<CollaborationDto>> GetCollaborationsAsync(string farmName)
        {
            Farm farm = await _farmRepository.GetFarmAsync(farmName);

            ICollection<Collaboration> collaborations = await _collaborationRepository
                .GetCollaborationsAsync(farm.Name);

            return _mapper.Map<ICollection<CollaborationDto>>(collaborations);
        }

        public async Task<ICollection<CollaborationDto>> GetCollaborationsAsync(Guid userId)
        {
            User user = await _userRepository.GetUserAsync(userId);

            ICollection<Collaboration> collaborations = await _collaborationRepository
                .GetCollaborationsAsync(user.Id);

            return _mapper.Map<ICollection<CollaborationDto>>(collaborations);
        }

        public async Task<ICollection<CollaborationDto>> GetPenidngCollaborationsAsync(string farmName)
        {
            Farm farm = await _farmRepository.GetFarmAsync(farmName);

            ICollection<Collaboration> collaborations = await _collaborationRepository
                .GetCollaborationsAsync(farm.Name, CollaborationStatus.Pending);

            return _mapper.Map<ICollection<CollaborationDto>>(collaborations);
        }

        public async Task<ICollection<CollaborationDto>> GetPenidngCollaborationsAsync(Guid userId)
        {
            User user = await _userRepository.GetUserAsync(userId);

            ICollection<Collaboration> collaborations = await _collaborationRepository
                .GetCollaborationsAsync(user.Id, CollaborationStatus.Accepted);

            return _mapper.Map<ICollection<CollaborationDto>>(collaborations);
        }
    }
}
