using innogotchi_api.Dtos;

namespace innogotchi_api.Interfaces
{
    public interface IInnogotchiAdapter
    {
        ICollection<InnogotchiDto> GetInnogotchis();
        ICollection<InnogotchiDto> GetInnogotchis(string farmName);
        InnogotchiDto GetInnogotchi(string name);
        bool InnogotchiExists(string name);
    }
}
