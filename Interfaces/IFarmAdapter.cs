using innogotchi_api.Dtos;
using System.Globalization;

namespace innogotchi_api.Interfaces
{
    public interface IFarmAdapter
    {
        ICollection<FarmResponseDto> GetFarms();
        ICollection<FarmResponseDto> GetFarms(string collaboratorName);
        FarmResponseDto GetFarm(string name);
        bool FarmExists(string name);
        FarmResponseDto AddFarm(string name, string userEmail);
        void DeleteFarm(string name);
        bool FarmCollaborationExists(string name, string collaboratorEmail);
        CollaborationDto AddFarmCollaboration(string name, string collaboratorEmail);
        void DeleteFarmCollaboration(string name, string collaboratorEmail);
    }
}
