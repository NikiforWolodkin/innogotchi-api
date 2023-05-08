using Data_layer.Data;
using Data_layer.Interfaces;
using Data_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_layer.Repositories
{
    public class CollaborationRepository : ICollaborationRepository
    {
        private readonly DataContext _context;

        public CollaborationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Collaboration> AddCollaboration(Collaboration collaboration)
        {
            _context.Collaborations.Add(collaboration);

            _context.SaveChanges();

            return await GetCollaboration(collaboration.UserId, collaboration.FarmName);
        }

        public void DeleteCollaboration(Collaboration collaboration)
        {
            _context.Collaborations.Remove(collaboration);

            _context.SaveChangesAsync();
        }

        public async Task<Collaboration> GetCollaboration(Guid userId, string farmName)
        {
            return await _context.Collaborations.FindAsync(farmName, userId);
        }

        public async Task<ICollection<Collaboration>> GetCollaborations()
        {
            return await _context.Collaborations.ToListAsync();
        }

        public async Task<ICollection<Collaboration>> GetCollaborations(string farmName)
        {
            return await _context.Collaborations
                .Where(c => c.FarmName == farmName)
                .ToListAsync();
        }

        public async Task<ICollection<Collaboration>> GetCollaborations(Guid userId)
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
