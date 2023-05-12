using ServiceLayer.Dtos;
using ServiceLayer.RequestDtos;

namespace ServiceLayer.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserAsync(Guid id);
        Task<UserDto> GetUserAsync(string email);
        Task<bool> ValidatePassword(Guid id, string password); 
        Guid AddUser(UserSignupDto request);
    }
}
