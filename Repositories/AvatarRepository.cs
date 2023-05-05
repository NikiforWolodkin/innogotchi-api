using innogotchi_api.Data;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;

namespace innogotchi_api.Repositories
{
    public class AvatarRepository : IAvatarRepository
    {
        private readonly DataContext _context;

        public AvatarRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public Avatar AddAvatar(byte[] image)
        {
            Avatar avatar = new Avatar();

            avatar.Image = image;

            _context.Avatars.Add(avatar);

            _context.SaveChanges();

            return GetAvatar(avatar.Id);
        }

        public bool AvatarExists(int id)
        {
            return _context.Avatars.Any(a => a.Id == id);
        }

        public Avatar GetAvatar(int id)
        {
            return _context.Avatars
                .Where(a => a.Id == id)
                .FirstOrDefault();
        }

        public ICollection<Avatar> GetAvatars()
        {
            return _context.Avatars.ToList();
        }
    }
}
