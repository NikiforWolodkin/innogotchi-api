using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public interface ICollaborationRepository
    {
        Task<ICollection<Collaboration>> GetCollaborationsAsync();
        Task<ICollection<Collaboration>> GetCollaborationsAsync(string farmName);
        Task<ICollection<Collaboration>> GetCollaborationsAsync(Guid userId);
        Task<Collaboration> GetCollaborationAsync(Guid userId, string farmName);
        Task<Collaboration> AddCollaborationAsync(Collaboration collaboration);
        void DeleteCollaboration(Collaboration collaboration);
        void UpdateDatabase();
    }
}
