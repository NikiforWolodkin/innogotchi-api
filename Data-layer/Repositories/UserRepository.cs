using DataLayer.Data;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public User AddUser(User user)
        {
            _context.Users.Add(user);

            _context.SaveChanges();

            return user;
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);

            _context.SaveChangesAsync();
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public void UpdateDatabase()
        {
            _context.SaveChangesAsync();
        }
    }
}
