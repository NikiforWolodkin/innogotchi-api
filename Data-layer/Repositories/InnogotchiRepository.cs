using DataLayer.Exceptions;
using DataLayer.Data;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class InnogotchiRepository : IInnogotchiRepository
    {
        private readonly DataContext _context;

        public InnogotchiRepository(DataContext context)
        {
            _context = context;
        }

        public Innogotchi AddInnogotchi(Innogotchi innogotchi, Farm farm)
        {
            try
            {
                farm.Innogotchis.Add(innogotchi);

                _context.Innogotchis.Add(innogotchi);

                _context.SaveChanges();

                return innogotchi;
            }
            catch
            {
                throw new DbAddException("Can't add innogotchi");
            }
        }

        public FeedingAndQuenching AddInnogotchiFeedingAndQuenching(Innogotchi innogotchi, FeedingAndQuenching feedingAndQuenching)
        {
            try
            {
                innogotchi.FeedingAndQuenchings.Add(feedingAndQuenching);

                _context.FeedingsAndQuenchings.Add(feedingAndQuenching);

                _context.SaveChanges();

                return feedingAndQuenching;
            }
            catch
            {
                throw new DbAddException("Can't feed or quench innogotchi");
            }
        }

        public async Task DeleteInnogotchiAsync(Innogotchi innogotchi)
        {
            try
            {
                _context.Innogotchis.Remove(innogotchi);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DbDeleteException("Can't delete innogotchi.");
            }
        }

        public async Task<Innogotchi> GetInnogotchiAsync(string name)
        {
            try
            {
                return await _context.Innogotchis
                    .Include(inno => inno.FeedingAndQuenchings)
                    .FirstAsync(inno => inno.Name == name);
            }
            catch
            {
                throw new NotFoundException("Innogotchi not found.");
            }
        }

        public async Task<ICollection<Innogotchi>> GetInnogotchisAsync()
        {
            return await _context.Innogotchis
                .Include(inno => inno.FeedingAndQuenchings)
                .ToListAsync();
        }

        public async Task<ICollection<Innogotchi>> GetInnogotchisAsync(string farmName)
        {
            try
            {
                var farm = await _context.Farms
                    .Include(farm => farm.Innogotchis)
                    .ThenInclude(inno => inno.FeedingAndQuenchings)
                    .FirstAsync(farm => farm.Name == farmName);

                return farm.Innogotchis;
            }
            catch
            {
                throw new NotFoundException("Farm not found.");
            }
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
