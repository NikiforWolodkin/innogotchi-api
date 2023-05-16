using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public interface IFarmRepository
    {
        Task<ICollection<Farm>> GetFarmsAsync();
        Task<ICollection<Farm>> GetFarmsAsync(Guid collaboratorId);
        Task<Farm> GetFarmAsync(string name);
        Farm AddFarm(Farm farm, User user);
        Task DeleteFarmAsync(Farm farm);
        Task UpdateDatabaseAsync();
    }
}
