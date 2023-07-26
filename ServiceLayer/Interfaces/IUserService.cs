using BusinessLayer.RequestDtos;
using DataLayer.Dtos;
using DataLayer.RequestDtos;

namespace DataLayer.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserAsync(Guid id);
        Task<UserDto> GetUserAsync(string email);
        Task<UserDto> UpdateUserProfileAsync(Guid id, UserUpdateProfileDto request);
        Task<UserDto> UpdateUserPasswordAsync(Guid id, string password);
        Task<bool> ValidatePassword(Guid id, string password); 
        Guid AddUser(UserSignupDto request);
    }
}
