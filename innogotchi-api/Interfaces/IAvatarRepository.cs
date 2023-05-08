using innogotchi_api.Dtos;
using innogotchi_api.Models;

namespace innogotchi_api.Interfaces
{
    public interface IAvatarRepository
    {
        ICollection<Avatar> GetAvatars();
        Avatar GetAvatar(int id);
        bool AvatarExists(int id);
        Avatar AddAvatar(byte[] image);
    }
}
