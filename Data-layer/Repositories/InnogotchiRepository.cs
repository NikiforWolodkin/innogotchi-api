using Data_layer.Data;
using Data_layer.Interfaces;
using Data_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_layer.Repositories
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
            farm.Innogotchis.Add(innogotchi);

            _context.Innogotchis.Add(innogotchi);

            _context.SaveChanges();

            return innogotchi;
        }

        public FeedingAndQuenching AddInnogotchiFeedingAndQuenching(Innogotchi innogotchi, FeedingAndQuenching feedingAndQuenching)
        {
            innogotchi.FeedingAndQuenchings.Add(feedingAndQuenching);

            _context.FeedingsAndQuenchings.Add(feedingAndQuenching);

            _context.SaveChanges();

            return feedingAndQuenching;
        }

        public void DeleteInnogotchi(Innogotchi innogotchi)
        {
            _context.Innogotchis.Remove(innogotchi);

            _context.SaveChangesAsync();
        }

        public async Task<Innogotchi> GetInnogotchi(string name)
        {
            return await _context.Innogotchis.FindAsync(name);
        }

        public async Task<ICollection<Innogotchi>> GetInnogotchis()
        {
            return await _context.Innogotchis.ToListAsync();
        }

        public async Task<ICollection<Innogotchi>> GetInnogotchis(string farmName)
        {
            var farm = await _context.Farms
                .Include(f => f.Innogotchis)
                .FirstOrDefaultAsync(f => f.Name == farmName);
            
            return farm?.Innogotchis;
        }

        public void UpdateDatabase()
        {
            _context.SaveChangesAsync();
        }
    }
}
