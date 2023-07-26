using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsersAsync();
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserAsync(string email);
        User AddUser(User user);
        Task DeleteUserAsync(User user);
        Task UpdateDatabaseAsync();
    }
}
