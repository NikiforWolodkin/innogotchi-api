using BusinessLayer.ResponseDtos;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Interfaces
{
    public interface IAvatarService
    {
        Task<AvatarDto> GetAvatarAsync(Guid id);
        Task AddAvatarAsync(IFormFile image, Guid userId);
    }
}
