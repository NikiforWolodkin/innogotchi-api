using innogotchi_api.Models;

namespace innogotchi_api.Interfaces
{
    public interface IInnogotchiRepository
    {
        ICollection<Innogotchi> GetInnogotchis();
        ICollection<Innogotchi> GetInnogotchis(string farmName);
        Innogotchi GetInnogotchi(string name);
        bool InnogotchiExists(string name);
        /// <summary>
        /// Checks if innogtchi is dead and sets the death date if it isn't set yet.
        /// </summary>
        Innogotchi AddInnogotchi(Innogotchi innogotchi, FeedingAndQuenching feedingAndQuenching, Farm farm);
        FeedingAndQuenching AddInnogotchiFeedingAndQuenching(Innogotchi innogotchi, FeedingAndQuenching feedingAndQuenching);
        bool IsInnogotchiDead(Innogotchi innogotchi);
        int GetInnogotchiAge(Innogotchi innogotchi);
        int GetInnogotchiHappinessDayCount(Innogotchi innogotchi);
        int GetInnogotchiAverageFeedingPeriod(Innogotchi innogotchi);
        int GetInnogotchiAverageQuenchingPeriod(Innogotchi innogotchi);
        string GetInnogotchiHungerLevel(Innogotchi innogotchi);
        string GetInnogotchiThirstLevel(Innogotchi innogotchi);
    }
}
