using innogotchi_api.Data;
using innogotchi_api.Helpers;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using Microsoft.EntityFrameworkCore;

namespace innogotchi_api.Repositories
{
    public class InnogotchiRepository : IInnogotchiRepository
    {
        private readonly DataContext _context;

        public InnogotchiRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Innogotchi> GetInnogotchis()
        {
            return _context.Innogotchis
                .Include(i => i.FeedingAndQuenchings)
                .ToList();
        }

        public ICollection<Innogotchi> GetInnogotchis(string farmName)
        {
            return _context.Farms
                .Include(f => f.Innogotchis)
                .Where(f => f.Name == farmName)
                .FirstOrDefault()
                .Innogotchis;
        }

        public Innogotchi GetInnogotchi(string name)
        {
            return _context.Innogotchis
                .Include(i => i.FeedingAndQuenchings)
                .Where(i => i.Name == name)
                .FirstOrDefault();
        }

        public bool InnogotchiExists(string name) 
        {
            return _context.Innogotchis.Any(i => i.Name == name);
        }

        public bool IsInnogotchiDead(Innogotchi innogotchi)
        {
            FeedingAndQuenching innogotchiFeedingAndQuenchingTime =
                innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .FirstOrDefault();

            DateTime lastFeedingTime = innogotchiFeedingAndQuenchingTime.FeedingTime;
            DateTime lastQuenchingTime = innogotchiFeedingAndQuenchingTime.QuenchingTime;

            int feedingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastFeedingTime);
            int quenchingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastQuenchingTime);

            if (feedingTimespan < TimeConverter.DEAD_FEEDING_THRESHOLD
                && quenchingTimespan < TimeConverter.DEAD_QUENCHING_THRESHOLD)
            {
                return false;
            }

            return true;
        }

        public int GetInnogotchiAge(Innogotchi innogotchi)
        {
            DateTime creationDate = _context.Innogotchis
                .Where(i => i.Name == innogotchi.Name)
                .FirstOrDefault()
                .CreationDate;

            return TimeConverter.ConvertToInnogotchiTime(DateTime.Now - creationDate);
        }

        public int GetInnogotchiHappinessDayCount(Innogotchi innogotchi)
        {
            throw new NotImplementedException();
        }

        public string GetInnogotchiHungerLevel(Innogotchi innogotchi)
        {
            throw new NotImplementedException();
        }


        public string GetInnogotchiThirstLevel(Innogotchi innogotchi)
        {
            throw new NotImplementedException();
        }
    }
}
