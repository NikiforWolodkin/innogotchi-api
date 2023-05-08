using Data_layer.Models;

namespace Data_layer.Interfaces
{
    public interface ICollaborationRepository
    {
        Task<ICollection<Collaboration>> GetCollaborations();
        Task<ICollection<Collaboration>> GetCollaborations(string farmName);
        Task<ICollection<Collaboration>> GetCollaborations(Guid userId);
        Task<Collaboration> GetCollaboration(Guid userId, string farmName);
        Task<Collaboration> AddCollaboration(Collaboration collaboration);
        void DeleteCollaboration(Collaboration collaboration);
        void UpdateDatabase();
    }
}
