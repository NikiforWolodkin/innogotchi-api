using DataLayer.Data;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class FarmRepository : IFarmRepository
    {
        private readonly DataContext _context;

        public FarmRepository(DataContext context)
        {
            _context = context;
        }

        public Farm AddFarm(Farm farm, User user)
        {
            user.Farm = farm;

            _context.Farms.Add(farm);

            _context.SaveChanges();

            return farm;
        }

        public void DeleteFarm(Farm farm)
        {
            _context.Remove(farm);

            _context.SaveChangesAsync();
        }

        public async Task<Farm> GetFarmAsync(string name)
        {
            return await _context.Farms.FindAsync(name);
        }

        public async Task<ICollection<Farm>> GetFarmsAsync()
        {
            return await _context.Farms.ToListAsync();
        }

        public async Task<ICollection<Farm>> GetFarmsAsync(Guid collaboratorId)
        {
            return await _context.Collaborations
                .Include(c => c.Farm)
                .Where(c => c.UserId == collaboratorId)
                .Select(c => c.Farm)
                .ToListAsync();
        }

        public void UpdateDatabase()
        {
            _context.SaveChangesAsync();
        }
    }
}
