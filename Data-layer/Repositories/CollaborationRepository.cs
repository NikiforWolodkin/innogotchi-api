using DataLayer.Exceptions;
using DataLayer.Data;
using DataLayer.Enums;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class CollaborationRepository : ICollaborationRepository
    {
        private readonly DataContext _context;

        public CollaborationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Collaboration> AddCollaborationAsync(Collaboration collaboration)
        {
            try
            {
                _context.Collaborations.Add(collaboration);

                _context.SaveChanges();

                return collaboration;
            }
            catch
            {
                throw new DbAddException("Can't add collaboration.");
            }
            
        }

        public async Task DeleteCollaborationAsync(Collaboration collaboration)
        {
            try
            {
                _context.Collaborations.Remove(collaboration);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DbDeleteException("Can't delete collaboration.");
            }
        }

        public async Task<Collaboration> GetCollaborationAsync(Guid userId, string farmName)
        {
            try
            {
                return await _context.Collaborations
                    .Include(collab => collab.Farm)
                    .Include(collab => collab.User)
                    .FirstAsync(collab => collab.FarmName == farmName && collab.UserId == userId);
            }
            catch
            {
                throw new NotFoundException("Collaboration not found.");
            }
        }

        public async Task<ICollection<Collaboration>> GetCollaborationsAsync()
        {
            return await _context.Collaborations
                .Include(collab => collab.Farm)
                .Include(collab => collab.User)
                .ToListAsync();
        }

        public async Task<ICollection<Collaboration>> GetCollaborationsAsync(string farmName)
        {
            return await _context.Collaborations
                .Include(collab => collab.Farm)
                .Include(collab => collab.User)
                .Where(collab => collab.FarmName == farmName)
                .ToListAsync();
        }

        public async Task<ICollection<Collaboration>> GetCollaborationsAsync(Guid userId)
        {
            return await _context.Collaborations
                .Include(collab => collab.Farm)
                .Include(collab => collab.User)
                .Where(collab => collab.UserId == userId)
                .ToListAsync();
        }

        public async Task<ICollection<Collaboration>> GetCollaborationsAsync(string farmName, CollaborationStatus status)
        {
            return await _context.Collaborations
                .Include(collab => collab.Farm)
                .Include(collab => collab.User)
                .Where(collab => collab.FarmName == farmName && collab.Status == status)
                .ToListAsync();
        }

        public async Task<ICollection<Collaboration>> GetCollaborationsAsync(Guid userId, CollaborationStatus status)
        {
            return await _context.Collaborations
                .Include(collab => collab.Farm)
                .Include(collab => collab.User)
                .Where(collab => collab.UserId == userId && collab.Status == status)
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
