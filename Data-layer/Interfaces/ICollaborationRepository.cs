using DataLayer.Enums;
using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public interface ICollaborationRepository
    {
        Task<ICollection<Collaboration>> GetCollaborationsAsync();
        Task<ICollection<Collaboration>> GetCollaborationsAsync(string farmName);
        Task<ICollection<Collaboration>> GetCollaborationsAsync(Guid userId);
        Task<ICollection<Collaboration>> GetCollaborationsAsync(string farmName, CollaborationStatus status);
        Task<ICollection<Collaboration>> GetCollaborationsAsync(Guid userId, CollaborationStatus status);
        Task<Collaboration> GetCollaborationAsync(Guid userId, string farmName);
        Task<Collaboration> AddCollaborationAsync(Collaboration collaboration);
        Task DeleteCollaborationAsync(Collaboration collaboration);
        Task UpdateDatabaseAsync();
    }
}
