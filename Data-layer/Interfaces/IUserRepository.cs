using Data_layer.Models;

namespace Data_layer.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsers();
        Task<User> GetUser(Guid id);
        User AddUser(User user);
        void DeleteUser(User user);
        void UpdateDatabase();
    }
}
