using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.ResponseDtos;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using DataLayer.Exceptions;

namespace BusinessLayer.Services
{
    public class AvatarService : IAvatarService
    {
        private readonly IAvatarRepository _avatarRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AvatarService(IAvatarRepository avatarRepository, IUserRepository userRepository, IMapper mapper)
        {
            _avatarRepository = avatarRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task AddAvatarAsync(IFormFile image, Guid userId)
        {
            User user = await _userRepository.GetUserAsync(userId);

            byte[] imageBinary;
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                imageBinary = ms.ToArray();
            }

            Avatar avatar = new Avatar
            {
                Image = imageBinary
            };

            _avatarRepository.AddAvatar(avatar, user);
        }

        public async Task<AvatarDto> GetAvatarAsync(Guid id)
        {
            Avatar avatar = await _avatarRepository.GetAvatarAsync(id);

            return _mapper.Map<AvatarDto>(avatar);
        }
    }
}
