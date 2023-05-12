using DataLayer.Data;
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
            _context.Collaborations.Add(collaboration);

            _context.SaveChanges();

            return await GetCollaborationAsync(collaboration.UserId, collaboration.FarmName);
        }

        public void DeleteCollaboration(Collaboration collaboration)
        {
            _context.Collaborations.Remove(collaboration);

            _context.SaveChangesAsync();
        }

        public async Task<Collaboration> GetCollaborationAsync(Guid userId, string farmName)
        {
            return await _context.Collaborations.FindAsync(farmName, userId);
        }

        public async Task<ICollection<Collaboration>> GetCollaborationsAsync()
        {
            return await _context.Collaborations.ToListAsync();
        }

        public async Task<ICollection<Collaboration>> GetCollaborationsAsync(string farmName)
        {
            return await _context.Collaborations
                .Where(c => c.FarmName == farmName)
                .ToListAsync();
        }

        public async Task<ICollection<Collaboration>> GetCollaborationsAsync(Guid userId)
        {
            return await _context.Collaborations
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public void UpdateDatabase()
        {
            _context.SaveChangesAsync();
        }
    }
}
