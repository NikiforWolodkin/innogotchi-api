using DataLayer.Models;

namespace Data_layer
{
    public interface IUserUOW
    {
        Task<ICollection<User>> GetUsersAsync();
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserAsync(string email);
        Task<Avatar> GetUserAvatarAsync(Guid id);
        Task<User> UpdateUserAsync(Guid id, string firstName = null, string lastName = null, byte[] avatarImageBinary = null);
        Task DeleteUserAsync(Guid id);
        User AddUser(string email, string passwordHash, string firstName, string lastName);
    }
}
