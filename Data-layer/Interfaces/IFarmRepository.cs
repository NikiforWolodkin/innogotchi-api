using Data_layer.Models;

namespace Data_layer.Interfaces
{
    public interface IFarmRepository
    {
        Task<ICollection<Farm>> GetFarms();
        Task<ICollection<Farm>> GetFarms(Guid collaboratorId);
        Task<Farm> GetFarm(string name);
        Farm AddFarm(Farm farm, User user);
        void DeleteFarm(Farm farm);
        void UpdateDatabase();
    }
}
