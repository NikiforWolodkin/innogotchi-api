using Data_layer.Models;

namespace Data_layer.Interfaces
{
    public interface IInnogotchiRepository
    {
        Task<ICollection<Innogotchi>> GetInnogotchis();
        Task<ICollection<Innogotchi>> GetInnogotchis(string farmName);
        Task<Innogotchi> GetInnogotchi(string name);
        Innogotchi AddInnogotchi(Innogotchi innogotchi, Farm farm);
        FeedingAndQuenching AddInnogotchiFeedingAndQuenching(Innogotchi innogotchi, FeedingAndQuenching feedingAndQuenching);
        void DeleteInnogotchi(Innogotchi innogotchi);
        void UpdateDatabase();
    }
}
