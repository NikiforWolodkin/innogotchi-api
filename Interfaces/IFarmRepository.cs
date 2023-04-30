using innogotchi_api.Models;

namespace innogotchi_api.Interfaces
{
    public interface IFarmRepository
    {
        ICollection<Farm> GetFarms();
        ICollection<Farm> GetCollaborationFarms();
        ICollection<Farm> GetCollaborationFarms(string collaboratorEmail);
        Farm GetFarm(string name);
        bool FarmExists(string name);
        int GetFarmAverageFeedingPeriod(string name);
        int GetFarmAverageQuenchingPeriod(string name);
        int GetFarmAverageHappinessDayCount(string name);
        int GetFarmAlivePetsCount(string name);
        int GetFarmDeadPetsCount(string name);
        int GetFarmAveragePetAge(string name);
    }
}
