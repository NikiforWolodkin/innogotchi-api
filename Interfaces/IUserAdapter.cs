using innogotchi_api.Dtos;

namespace innogotchi_api.Interfaces
{
    public interface IUserAdapter
    {
        ICollection<UserResponseDto> GetUsers();
        UserResponseDto GetUser(string email);
        bool UserExists(string email);
        UserResponseDto AddUser(UserRequestDto request);
        UserResponseDto UpdateUser(UserRequestDto request);
        void DeleteUser(string email);
        /// <summary>
        /// Verifies user password.
        /// </summary>
        /// <returns>True if password is correct, false otherwise.</returns>
        bool ValidateUserPassword(string email, string password);
    }
}
