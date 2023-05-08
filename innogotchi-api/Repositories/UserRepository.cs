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
                .Include(u => u.Farm)
                .ThenInclude(f => f.Innogotchis)
                .Include(u => u.Collaborations)
                .ToList();
        }
        public User GetUser(string email)
        {
            return _context.Users
                .Include(u => u.Farm)
                .ThenInclude(f => f.Innogotchis)
                .Include(u => u.Collaborations)
                .Where(u => u.Email == email)
                .FirstOrDefault();
        }

        public bool UserExists(string email) 
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public User AddUser(User user)
        {
            _context.Users.Add(user);

            _context.SaveChanges();

            return GetUser(user.Email);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);

            _context.SaveChanges();
        }

        public void UpdateDatabase()
        {
            _context.SaveChanges();
        }
    }
}
