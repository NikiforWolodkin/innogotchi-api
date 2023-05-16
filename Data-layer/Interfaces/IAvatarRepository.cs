using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public interface IAvatarRepository
    {
        Task<ICollection<Avatar>> GetAvatarsAsync();
        Task<Avatar> GetAvatarAsync(Guid id);
        Avatar AddAvatar(Avatar avatar, User user);
    }
}
