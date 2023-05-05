using innogotchi_api.Data;
using innogotchi_api.Helpers;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public Innogotchi AddInnogotchi(Innogotchi innogotchi, FeedingAndQuenching feedingAndQuenching, Farm farm)
        {
            innogotchi.FeedingAndQuenchings = new List<FeedingAndQuenching>();

            innogotchi.FeedingAndQuenchings.Add(feedingAndQuenching);

            farm.Innogotchis.Add(innogotchi);

            _context.Innogotchis.Add(innogotchi);

            _context.FeedingsAndQuenchings.Add(feedingAndQuenching);

            _context.SaveChanges();

            return GetInnogotchi(innogotchi.Name);
        }

        public bool InnogotchiExists(string name) 
        {
            return _context.Innogotchis.Any(i => i.Name == name);
        }

        /// <summary>
        /// Checks if innogtchi is dead and sets the death date if it isn't set yet.
        /// </summary>
        public bool IsInnogotchiDead(Innogotchi innogotchi)
        {
            if (innogotchi.DeathDate is not null)
            {
                return true;
            }

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

            if (quenchingTimespan > feedingTimespan)
            {
                innogotchi.DeathDate = lastQuenchingTime
                    .AddDays(TimeConverter.DEAD_QUENCHING_THRESHOLD / TimeConverter.INNOGOTCHI_TIME_MODIFIER);
            }
            else
            {
                innogotchi.DeathDate = lastFeedingTime
                    .AddDays(TimeConverter.DEAD_FEEDING_THRESHOLD / TimeConverter.INNOGOTCHI_TIME_MODIFIER);
            }

            _context.SaveChanges();

            return true;
        }

        public int GetInnogotchiAge(Innogotchi innogotchi)
        {
            if (IsInnogotchiDead(innogotchi))
            {
                DateTime deathDate = _context.Innogotchis
                    .Where(i => i.Name == innogotchi.Name)
                    .FirstOrDefault()
                    .CreationDate;

                return TimeConverter.ConvertToInnogotchiTime(DateTime.Now - deathDate);
            }

            DateTime creationDate = _context.Innogotchis
                .Where(i => i.Name == innogotchi.Name)
                .FirstOrDefault()
                .CreationDate;

            return TimeConverter.ConvertToInnogotchiTime(DateTime.Now - creationDate);
        }

        public int GetInnogotchiHappinessDayCount(Innogotchi innogotchi)
        {
            IList<FeedingAndQuenching> feedingsAndQuenchings = innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .ToList();

            DateTime lastFeedingTime = feedingsAndQuenchings.First().FeedingTime;
            DateTime lastQuenchingTime = feedingsAndQuenchings.First().QuenchingTime;

            int feedingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastFeedingTime);
            int quenchingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastQuenchingTime);

            int happyDays = 0;

            if (feedingTimespan >= TimeConverter.NORMAL_FEEDING_THRESHOLD)
            {
                feedingTimespan = feedingTimespan - TimeConverter.NORMAL_FEEDING_THRESHOLD;
            }
            else
            {
                feedingTimespan = 0;
            }
            if (quenchingTimespan >= TimeConverter.NORMAL_QUENCHING_THRESHOLD)
            {
                quenchingTimespan = quenchingTimespan - TimeConverter.NORMAL_QUENCHING_THRESHOLD;
            }
            else
            {
                quenchingTimespan = 0;
            }

            happyDays += (GetInnogotchiAge(innogotchi)
                - (feedingTimespan > quenchingTimespan ? feedingTimespan : quenchingTimespan))
                - (int)feedingsAndQuenchings.Select(f => f.UnhappyDays).Sum();

            return happyDays;
        }

        public string GetInnogotchiHungerLevel(Innogotchi innogotchi)
        {
            FeedingAndQuenching innogotchiFeedingAndQuenchingTime =
                innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .FirstOrDefault();

            DateTime lastFeedingTime = innogotchiFeedingAndQuenchingTime.FeedingTime;

            int feedingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastFeedingTime);
            
            switch (feedingTimespan)
            {
                case < TimeConverter.NORMAL_FEEDING_THRESHOLD:
                    return "FULL";
                case < TimeConverter.HUNGER_FEEDING_THRESHOLD:
                    return "NORMAL";
                case < TimeConverter.DEAD_FEEDING_THRESHOLD:
                    return "HUNGRY";
                default:
                    return "DEAD";
            }
        }

        public string GetInnogotchiThirstLevel(Innogotchi innogotchi)
        {
            FeedingAndQuenching innogotchiFeedingAndQuenchingTime =
                innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .FirstOrDefault();

            DateTime lastQuenchingTime = innogotchiFeedingAndQuenchingTime.QuenchingTime;

            int quenchingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastQuenchingTime);

            switch (quenchingTimespan)
            {
                case < TimeConverter.NORMAL_QUENCHING_THRESHOLD:
                    return "FULL";
                case < TimeConverter.HUNGER_QUENCHING_THRESHOLD:
                    return "NORMAL";
                case < TimeConverter.DEAD_QUENCHING_THRESHOLD:
                    return "THIRSTY";
                default:
                    return "DEAD";
            }
        }

        public int GetInnogotchiAverageFeedingPeriod(Innogotchi innogotchi)
        {
            IList<FeedingAndQuenching> feedingsAndQuenchings = innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .ToList();

            if (innogotchi.FeedingAndQuenchings.Count > 1)
            {
                feedingsAndQuenchings = feedingsAndQuenchings
                    .Take(innogotchi.FeedingAndQuenchings.Count - 1)
                    .ToList();
            }

            DateTime lastFeedingTime = feedingsAndQuenchings.First().FeedingTime;

            int feedingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastFeedingTime);
            
            IList<int> feedingPeriods = feedingsAndQuenchings.Select(f => (int)f.FeedingPeriod).ToList();

            feedingPeriods.Add(feedingTimespan);

            return (int)feedingPeriods.Average();
        }

        public int GetInnogotchiAverageQuenchingPeriod(Innogotchi innogotchi)
        {
            IList<FeedingAndQuenching> feedingsAndQuenchings = innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .ToList();

            if (innogotchi.FeedingAndQuenchings.Count > 1)
            {
                feedingsAndQuenchings = feedingsAndQuenchings
                    .Take(innogotchi.FeedingAndQuenchings.Count - 1)
                    .ToList();
            }

            DateTime lastQuenchingTime = feedingsAndQuenchings.First().QuenchingTime;

            int quenchingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastQuenchingTime);

            IList<int> quenchingPeriods = feedingsAndQuenchings.Select(f => (int)f.FeedingPeriod).ToList();

            quenchingPeriods.Add(quenchingTimespan);

            return (int)quenchingPeriods.Average();
        }

        public FeedingAndQuenching AddInnogotchiFeedingAndQuenching(Innogotchi innogotchi,FeedingAndQuenching feedingAndQuenching)
        {
            innogotchi.FeedingAndQuenchings.Add(feedingAndQuenching);

            _context.FeedingsAndQuenchings.Add(feedingAndQuenching);

            _context.SaveChanges();

            return _context.FeedingsAndQuenchings
                .Where(f => f.Id == feedingAndQuenching.Id)
                .FirstOrDefault();
        }
    }
}
