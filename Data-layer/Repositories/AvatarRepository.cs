using DataLayer.Exceptions;
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
            try
            {
                user.Avatar = avatar;

                _context.Avatars.Add(avatar);

                _context.SaveChanges();

                return avatar;
            }
            catch
            {
                throw new DbAddException("Can't add avatar.");
            }
        }

        public async Task<Avatar> GetAvatarAsync(Guid id)
        {
            try
            {
                return await _context.Avatars.FirstAsync(avatar => avatar.Id == id);
            }
            catch
            {
                throw new NotFoundException("Avatar not found.");
            }
        }

        public async Task<ICollection<Avatar>> GetAvatarsAsync()
        {
            return await _context.Avatars.ToListAsync();

            //return await _context.Avatars
            //    .FromSql($"SELECT * FROM dbo.Avatars")
            //    .ToListAsync();
        }
    }
}
