
using DataLayer.Data;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
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

        public async Task<Avatar> GetAvatarAsync(Guid id)
        {
            return await _context.Avatars.FindAsync(id);
        }

        public async Task<ICollection<Avatar>> GetAvatarsAsync()
        {
            return await _context.Avatars.ToListAsync();
        }
    }
}
