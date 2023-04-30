using innogotchi_api.Dtos;

namespace innogotchi_api.Interfaces
{
    public interface IUserAdapter
    {
        ICollection<UserDto> GetUsers();
        UserDto GetUser(string email);
        bool UserExists(string email);
    }
}
