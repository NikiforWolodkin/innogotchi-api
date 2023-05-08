using innogotchi_api.Data;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace innogotchi_api.Repositories
{
    public class FarmRepository : IFarmRepository
    {
        private readonly DataContext _context;
        private readonly IInnogotchiRepository _innogotchiRepository;

        public FarmRepository(DataContext context, IInnogotchiRepository innogotchiRepository)
        {
            _context = context;
            _innogotchiRepository = innogotchiRepository;
        }

        public ICollection<Farm> GetFarms()
        {
            return _context.Farms
                .Include(f => f.Innogotchis)
                .ThenInclude(i => i.FeedingAndQuenchings)
                .ToList();
        }

        public ICollection<Farm> GetFarms(string collaboratorEmail)
        {
            return _context.Collaborations
                .Include(c => c.Farm)
                .ThenInclude(f => f.Innogotchis)
                .ThenInclude(i => i.FeedingAndQuenchings)
                .Include(c => c.User)
                .Where(c => c.UserEmail == collaboratorEmail)
                .Select(c => c.Farm)
                .ToList();
        }

        public Farm GetFarm(string name)
        {
            return _context.Farms
                .Include(f => f.Innogotchis)
                .ThenInclude(i => i.FeedingAndQuenchings)
                .Include(f => f.Collaborations)
                .Where(f => f.Name == name)
                .FirstOrDefault();
        }

        public bool FarmExists(string name)
        {
            return _context.Farms.Any(f => f.Name == name);
        }

        public Farm AddFarm(User user, Farm farm)
        {
            user.Farm = farm;

            _context.Farms.Add(farm);

            _context.SaveChanges();

            return farm;
        }

        public void DeleteFarm(Farm farm)
        {
            _context.Farms.Remove(farm);

            _context.SaveChanges();
        }

        public Collaboration GetFarmCollaboration(string name, string userEmail)
        {
            return _context.Collaborations
                .Include(c => c.Farm)
                .Include(c => c.User)
                .Where(c => c.UserEmail == userEmail && c.FarmName == name)
                .FirstOrDefault();
        }

        public bool FarmCollaborationExists(string name, string userEmail)
        {
            return _context.Collaborations
                .Any(c => c.UserEmail == userEmail && c.FarmName == name);
        }

        public Collaboration AddFarmCollaboration(Collaboration collaboration)
        {
            _context.Collaborations.Add(collaboration);

            _context.SaveChanges();

            return GetFarmCollaboration(collaboration.FarmName, collaboration.UserEmail);
        }

        public void DeleteFarmCollaboration(Collaboration collaboration)
        {
            _context.Collaborations.Remove(collaboration);

            _context.SaveChanges();
        }

        public int GetFarmAveragePetAge(Farm farm)
        {
            if (farm.Innogotchis.IsNullOrEmpty())
            {
                return -1;
            }

            return (int)farm.Innogotchis
                .Select(i => _innogotchiRepository.GetInnogotchiAge(i))
                .Average();
        }

        public int GetFarmAlivePetsCount(Farm farm)
        {
            if (farm.Innogotchis.IsNullOrEmpty())
            {
                return -1;
            }

            return farm.Innogotchis
                .Where(i => _innogotchiRepository.IsInnogotchiDead(i) == false)
                .Count();
        }

        public int GetFarmDeadPetsCount(Farm farm)
        {
            if (farm.Innogotchis.IsNullOrEmpty())
            {
                return -1;
            }

            return farm.Innogotchis
                .Where(i => _innogotchiRepository.IsInnogotchiDead(i) == true)
                .Count();
        }

        public int GetFarmAverageFeedingPeriod(Farm farm)
        {
            if (farm.Innogotchis.IsNullOrEmpty())
            {
                return -1;
            }

            return (int)farm.Innogotchis
                .Select(i => _innogotchiRepository.GetInnogotchiAverageFeedingPeriod(i))
                .Average();
        }

        public int GetFarmAverageQuenchingPeriod(Farm farm)
        {
            if (farm.Innogotchis.IsNullOrEmpty())
            {
                return -1;
            }

            return (int)farm.Innogotchis
                .Select(i => _innogotchiRepository.GetInnogotchiAverageQuenchingPeriod(i))
                .Average();
        }

        public int GetFarmAverageHappinessDayCount(Farm farm)
        {
            if (farm.Innogotchis.IsNullOrEmpty())
            {
                return -1;
            }

            return (int)farm.Innogotchis
                .Select(i => _innogotchiRepository.GetInnogotchiHappinessDayCount(i))
                .Average();
        }

        public void UpdateDatabase()
        {
            _context.SaveChanges();
        }
    }
}
