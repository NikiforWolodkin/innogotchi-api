using innogotchi_api.Data;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;

namespace innogotchi_api.Repositories
{
    public class FarmRepository : IFarmRepository
    {
        private readonly DataContext _context;

        public FarmRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Farm> GetFarms()
        {
            return _context.Farms.ToList();
        }

        public ICollection<Farm> GetCollaborationFarms()
        {
            var collaborations = _context.Users
                .Select(u => u.Collaborations)
                .SelectMany(l => l)
                .Select(c => c.FarmName)
                .ToList();

            return _context.Farms.Where(f => collaborations.Contains(f.Name)).ToList();
        }

        public ICollection<Farm> GetCollaborationFarms(string collaboratorEmail)
        {
            var collaborations = _context.Users
                .Select(u => u.Collaborations)
                .SelectMany(l => l)
                .Where(c => c.UserEmail == collaboratorEmail)
                .Select(c => c.FarmName)
                .ToList();

            return _context.Farms.Where(f => collaborations.Contains(f.Name)).ToList();
        }


        public Farm GetFarm(string name)
        {
            return _context.Farms.Where(f => f.Name == name).FirstOrDefault();
        }

        public bool FarmExists(string name)
        {
            return _context.Farms.Any(f => f.Name == name);
        }

        public int GetFarmAveragePetAge(string name)
        {
            throw new NotImplementedException();
        }

        public int GetFarmAlivePetsCount(string name)
        {
            throw new NotImplementedException();
        }

        public int GetFarmAverageFeedingPeriod(string name)
        {
            throw new NotImplementedException();
        }

        public int GetFarmAverageHappinessDayCount(string name)
        {
            throw new NotImplementedException();
        }

        public int GetFarmAverageQuenchingPeriod(string name)
        {
            throw new NotImplementedException();
        }

        public int GetFarmDeadPetsCount(string name)
        {
            throw new NotImplementedException();
        }

    }
}
