using DataLayer.Interfaces;
using DataLayer.Models;

namespace Data_layer
{
    public class UserUnitOfWork : IUserUOW
    {
        private readonly IUserRepository _userRepository;
        private readonly IAvatarRepository _avatarRepository;

        public UserUnitOfWork(IUserRepository userRepository, IAvatarRepository avatarRepository)
        {
            _userRepository = userRepository;
            _avatarRepository = avatarRepository;
        }

        public User AddUser(string email, string passwordHash, string firstName, string lastName)
        {
            User user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName
            };

            return _userRepository.AddUser(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            User user = await _userRepository.GetUserAsync(id);

            await _userRepository.DeleteUserAsync(user);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _userRepository.GetUserAsync(id);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _userRepository.GetUserAsync(email);
        }

        public async Task<Avatar> GetUserAvatarAsync(Guid id)
        {
            User user = await _userRepository.GetUserAsync(id);

            return await _avatarRepository.GetAvatarAsync(user.Avatar.Id);
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> UpdateUserAsync(Guid id, string firstName = null, string lastName = null, byte[] avatarImageBinary = null)
        {
            User user = await _userRepository.GetUserAsync(id);

            if (firstName is not null)
            {
                user.FirstName = firstName;
            }
            if (lastName is not null)
            {
                user.LastName = lastName;
            }
            if (avatarImageBinary is not null)
            {
                Avatar avatar = new Avatar
                {
                    Image = avatarImageBinary
                };

                _avatarRepository.AddAvatar(avatar, user);
            }

            return user;
        }
    }
}
