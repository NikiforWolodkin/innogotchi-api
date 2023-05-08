
using Data_layer.Data;
using Data_layer.Interfaces;
using Data_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_layer.Repositories
{
    public class AvatarRepository : IAvatarRepository
    {
        private readonly DataContext _context;

        public AvatarRepository(DataContext context)
        {
            _context = context;
        }

        public Avatar AddAvatar(Avatar avatar, User user)
        {
            user.Avatar = avatar;

            _context.Avatars.Add(avatar);

            _context.SaveChanges();

            return avatar;
        }

        public async Task<Avatar> GetAvatar(Guid id)
        {
            return await _context.Avatars.FindAsync(id);
        }

        public async Task<ICollection<Avatar>> GetAvatars()
        {
            return await _context.Avatars.ToListAsync();
        }
    }
}
