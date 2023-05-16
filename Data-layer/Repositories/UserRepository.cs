using DataLayer.Data;
using DataLayer.Exceptions;
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
            try
            {
                _context.Users.Add(user);

                _context.SaveChanges();

                return user;
            }
            catch
            {
                throw new DbAddException("Can't add user.");
            }
        }

        public async Task DeleteUserAsync(User user)
        {
            try
            {
                _context.Users.Remove(user);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DbDeleteException("Can't delete user.");
            }
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            try
            {
                return await _context.Users
                    .Include(user => user.Farm)
                    .Include(user => user.Avatar)
                    .FirstAsync(user => user.Id == id);
            }
            catch
            {
                throw new NotFoundException("User not found.");
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            try
            {
                return await _context.Users
                    .Include(user => user.Farm)
                    .Include(user => user.Avatar)
                    .FirstAsync(user => user.Email == email);
            }
            catch
            {
                throw new NotFoundException("User not found.");
            } 
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            return await _context.Users
                .Include(user => user.Farm)
                .Include(user => user.Avatar)
                .ToListAsync();
        }

        public async Task UpdateDatabaseAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DbAddException("Can't update changes.");
            }
        }
    }
}
