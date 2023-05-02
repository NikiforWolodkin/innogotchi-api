using innogotchi_api.Models;

namespace innogotchi_api.Interfaces
{
    public interface IFarmRepository
    {
        ICollection<Farm> GetFarms();
        ICollection<Farm> GetFarms(string collaboratorEmail);
        Farm GetFarm(string name);
        bool FarmExists(string name);
        Farm AddFarm(User user, Farm farm);
        void DeleteFarm(Farm farm);
        Collaboration GetFarmCollaboration(string name, string collaboratorEmail);
        bool FarmCollaborationExists(string name, string collaboratorEmail);
        Collaboration AddFarmCollaboration(Collaboration collaboration);
        void DeleteFarmCollaboration(Collaboration collaboration);
        int GetFarmAverageFeedingPeriod(Farm farm);
        int GetFarmAverageQuenchingPeriod(Farm farm);
        int GetFarmAverageHappinessDayCount(Farm farm);
        int GetFarmAlivePetsCount(Farm farm);
        int GetFarmDeadPetsCount(Farm farm);
        int GetFarmAveragePetAge(Farm farm);
        void UpdateDatabase();
    }
}
