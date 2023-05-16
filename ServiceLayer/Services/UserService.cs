using AutoMapper;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Dtos;
using DataLayer.RequestDtos;
using BusinessLayer.RequestDtos;

namespace DataLayer.Services
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
            User user = await _userRepository.GetUserAsync(id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            User user = await _userRepository.GetUserAsync(email);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<ICollection<UserDto>> GetUsersAsync()
        {
            ICollection<User> users = await _userRepository.GetUsersAsync();

            return _mapper.Map<ICollection<UserDto>>(users);
        }

        public async Task<UserDto> UpdateUserProfileAsync(Guid id, UserUpdateProfileDto request)
        {
            User user = await _userRepository.GetUserAsync(id);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            await _userRepository.UpdateDatabaseAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserPasswordAsync(Guid id, string password)
        {
            User user = await _userRepository.GetUserAsync(id);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            await _userRepository.UpdateDatabaseAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> ValidatePassword(Guid id, string password)
        {
            User user = await _userRepository.GetUserAsync(id);

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
