using DataLayer.Exceptions;
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
            try
            {
                user.Farm = farm;

                _context.Farms.Add(farm);

                _context.SaveChanges();

                return farm;
            }
            catch
            {
                throw new DbAddException("Can't add farm");
            }
        }

        public async Task DeleteFarmAsync(Farm farm)
        {
            try
            {
                _context.Remove(farm);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DbDeleteException("Can't delete farm.");
            }
        }

        public async Task<Farm> GetFarmAsync(string name)
        {
            try
            {
                return await _context.Farms
                    .Include(farm => farm.Innogotchis)
                    .ThenInclude(inno => inno.FeedingAndQuenchings)
                    .FirstAsync(farm => farm.Name == name);
            }
            catch
            {
                throw new NotFoundException("Farm not found.");
            }
            
        }

        public async Task<ICollection<Farm>> GetFarmsAsync()
        {
            return await _context.Farms
                .Include(farm => farm.Innogotchis)
                .ThenInclude(inno => inno.FeedingAndQuenchings)
                .ToListAsync();
        }

        public async Task<ICollection<Farm>> GetFarmsAsync(Guid collaboratorId)
        {
            return await _context.Collaborations
                .Include(collab => collab.Farm)
                .Where(collab => collab.UserId == collaboratorId)
                .Select(collab => collab.Farm)
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
