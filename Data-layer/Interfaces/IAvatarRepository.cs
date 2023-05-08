using Data_layer.Models;

namespace Data_layer.Interfaces
{
    public interface IAvatarRepository
    {
        Task<ICollection<Avatar>> GetAvatars();
        Task<Avatar> GetAvatar(Guid id);
        Avatar AddAvatar(Avatar avatar, User user);
    }
}
