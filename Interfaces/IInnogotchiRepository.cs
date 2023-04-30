using innogotchi_api.Models;

namespace innogotchi_api.Interfaces
{
    public interface IInnogotchiRepository
    {
        ICollection<Innogotchi> GetInnogotchis();
        ICollection<Innogotchi> GetInnogotchis(string farmName);
        Innogotchi GetInnogotchi(string name);
        bool InnogotchiExists(string name);
        bool IsInnogotchiDead(Innogotchi innogotchi);
        int GetInnogotchiAge(Innogotchi innogotchi);
        int GetInnogotchiHappinessDayCount(Innogotchi innogotchi);
        string GetInnogotchiHungerLevel(Innogotchi innogotchi);
        string GetInnogotchiThirstLevel(Innogotchi innogotchi);
    }
}
