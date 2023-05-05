using AutoMapper;
using Azure.Core;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;

namespace innogotchi_api.Services
{
    public class UserAdapter : IUserAdapter
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserAdapter(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public ICollection<UserResponseDto> GetUsers()
        {
            IList<User> users = _userRepository.GetUsers().ToList();

            IList<UserResponseDto> usersDto = _mapper.Map<List<UserResponseDto>>(users);

            for (int i = 0; i < users.Count(); i++)
            {
                usersDto[i].FarmName = users[i].Farm?.Name;
                usersDto[i].AvatarId = users[i].Avatar?.Id;
            }

            return usersDto;
        }

        public UserResponseDto GetUser(string email)
        {
            User user = _userRepository.GetUser(email);

            UserResponseDto response = _mapper.Map<UserResponseDto>(user);

            response.FarmName = user.Farm?.Name;
            response.AvatarId = user.Avatar?.Id;

            return response;
        }

        public bool UserExists(string email)
        {
            return _userRepository.UserExists(email);
        }

        public UserResponseDto AddUser(UserRequestDto request)
        {
            User user = new User();

            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            user = _userRepository.AddUser(user);

            UserResponseDto response = GetUser(request.Email);

            return response;
        }

        public UserResponseDto UpdateUser(UserRequestDto request)
        {
            User user = _userRepository.GetUser(request.Email);

            if (request.FirstName is not null)
                user.FirstName = request.FirstName;
            if (request.LastName is not null)
                user.LastName = request.LastName;
            if (request.Password is not null)
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            _userRepository.UpdateDatabase();

            UserResponseDto response = GetUser(request.Email);

            return response;
        }

        public void DeleteUser(string email)
        {
            User user = _userRepository.GetUser(email);

            _userRepository.DeleteUser(user);
        }

        /// <summary>
        /// Verifies user password.
        /// </summary>
        /// <returns>True if password is correct, false otherwise.</returns>
        public bool ValidateUserPassword(string email, string password)
        {
            User user = _userRepository.GetUser(email);

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
