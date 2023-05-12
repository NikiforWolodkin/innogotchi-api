using AutoMapper;
using DataLayer.Interfaces;
using DataLayer.Models;
using ServiceLayer.Dtos;
using ServiceLayer.Exceptions;
using ServiceLayer.Interfaces;
using ServiceLayer.RequestDtos;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Guid AddUser(UserSignupDto request)
        {
            User user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            user = _userRepository.AddUser(user);

            return user.Id;
        }

        public async Task<UserDto> GetUserAsync(Guid id)
        {
            User user = await _userRepository.GetUserAsync(id)
                ?? throw new NotFoundException("User not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            User user = await _userRepository.GetUserAsync(email)
                ?? throw new NotFoundException("User not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<ICollection<UserDto>> GetUsersAsync()
        {
            ICollection<User> users = await _userRepository.GetUsersAsync();

            return _mapper.Map<ICollection<UserDto>>(users);
        }

        public async Task<bool> ValidatePassword(Guid id, string password)
        {
            User user = await _userRepository.GetUserAsync(id)
                ?? throw new NotFoundException("User not found.");

            return BCrypt.Net.BCrypt.Verify(user.PasswordHash, password);
        }
    }
}
