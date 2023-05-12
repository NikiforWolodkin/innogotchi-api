using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public interface IInnogotchiRepository
    {
        Task<ICollection<Innogotchi>> GetInnogotchisAsync();
        Task<ICollection<Innogotchi>> GetInnogotchisAsync(string farmName);
        Task<Innogotchi> GetInnogotchiAsync(string name);
        Innogotchi AddInnogotchi(Innogotchi innogotchi, Farm farm);
        FeedingAndQuenching AddInnogotchiFeedingAndQuenching(Innogotchi innogotchi, FeedingAndQuenching feedingAndQuenching);
        void DeleteInnogotchi(Innogotchi innogotchi);
        void UpdateDatabase();
    }
}
