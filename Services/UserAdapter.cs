using AutoMapper;
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

        public ICollection<UserDto> GetUsers()
        {
            IList<User> users = _userRepository.GetUsers().ToList();

            IList<UserDto> usersDto = _mapper.Map<List<UserDto>>(users);

            for (int i = 0; i < usersDto.Count; i++)
            {
                usersDto[i].FarmName = _userRepository.GetUserFarmName(users[i]);
            }

            return usersDto;
        }

        public UserDto GetUser(string email)
        {
            User user = _userRepository.GetUser(email);

            UserDto userDto = _mapper.Map<UserDto>(user);

            userDto.FarmName = _userRepository.GetUserFarmName(user);

            return userDto;
        }

        public bool UserExists(string email)
        {
            return _userRepository.UserExists(email);
        }
    }
}
