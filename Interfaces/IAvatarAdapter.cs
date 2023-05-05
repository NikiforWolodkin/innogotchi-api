using innogotchi_api.Dtos;
using innogotchi_api.Models;

namespace innogotchi_api.Interfaces
{
    public interface IAvatarAdapter
    {
        ICollection<AvatarIdDto> GetAvatarIds();
        IFormFile GetAvatar(int id);
        bool AvatarExists(int id);
        AvatarIdDto AddAvatar(IFormFile image);
    }
}
