using innogotchi_api.Dtos;

namespace innogotchi_api.Interfaces
{
    public interface IFarmAdapter
    {
        ICollection<FarmDto> GetFarms();
        ICollection<FarmDto> GetFarms(string collaboratorName);
        FarmDto GetFarm(string name);
        bool FarmExists(string name);
    }
}
