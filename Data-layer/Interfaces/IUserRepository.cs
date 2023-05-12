using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsersAsync();
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserAsync(string email);
        User AddUser(User user);
        void DeleteUser(User user);
        void UpdateDatabase();
    }
}
