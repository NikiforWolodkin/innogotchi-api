using innogotchi_api.Data;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using Microsoft.EntityFrameworkCore;

namespace innogotchi_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context) 
        {
            _context = context;
        }
        public ICollection<User> GetUsers()
        {
            return _context.Users
                .Include(u => u.Farms)
                .Include(u => u.Collaborations)
                .ToList();
        }
        public User GetUser(string email)
        {
            return _context.Users
                .Include(u => u.Farms)
                .Include(u => u.Collaborations)
                .Where(u => u.Email == email)
                .FirstOrDefault();
        }

        public bool UserExists(string email) 
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public string GetUserFarmName(User user)
        {
            return user.Farms.FirstOrDefault().Name;
        }
    }
}
