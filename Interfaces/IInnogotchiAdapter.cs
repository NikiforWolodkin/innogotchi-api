using innogotchi_api.Dtos;

namespace innogotchi_api.Interfaces
{
    public interface IInnogotchiAdapter
    {
        ICollection<InnogotchiResponseDto> GetInnogotchis();
        ICollection<InnogotchiResponseDto> GetInnogotchis(string farmName);
        InnogotchiResponseDto GetInnogotchi(string name);
        bool InnogotchiExists(string name);
        InnogotchiResponseDto AddInnogotchi(InnogotchiRequestDto request, string farmName);
        InnogotchiResponseDto AddInnogotchiFeedingAndQuenching(string name, string action);
        bool CanFeedInnogotchi(string name, string userEmail);
    }
}
