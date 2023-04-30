using innogotchi_api.Models;

namespace innogotchi_api.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(string email);
        public bool UserExists(string email);
        public string GetUserFarmName(User user);
    }
}
