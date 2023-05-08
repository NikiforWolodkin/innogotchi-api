using innogotchi_api.Models;

namespace innogotchi_api.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(string email);
        bool UserExists(string email);
        User AddUser(User user);
        void DeleteUser(User user);
        void UpdateDatabase();
    }
}
