using Data_layer.Data;
using Data_layer.Interfaces;
using Data_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_layer.Repositories
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

        public async Task<User> GetUser(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public void UpdateDatabase()
        {
            _context.SaveChangesAsync();
        }
    }
}
