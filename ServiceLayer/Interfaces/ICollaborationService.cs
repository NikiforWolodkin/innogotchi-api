using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;
using DataLayer.Enums;

namespace BusinessLayer.Interfaces
{
    public interface ICollaborationService
    {
        Task<ICollection<CollaborationDto>> GetCollaborationsAsync(string farmName);
        Task<ICollection<CollaborationDto>> GetCollaborationsAsync(Guid userId);
        Task<ICollection<CollaborationDto>> GetPenidngCollaborationsAsync(string farmName);
        Task<ICollection<CollaborationDto>> GetPenidngCollaborationsAsync(Guid userId);
        Task<ICollection<CollaborationDto>> GetAcceptedCollaborationsAsync(string farmName);
        Task<ICollection<CollaborationDto>> GetAcceptedCollaborationsAsync(Guid userId);
        Task AddCollaborationAsync(string farmName, CollaborationAddDto request);
        Task<CollaborationDto> ChangeCollaborationStatusAsync(string farmName, Guid userId, CollaborationChangeStatusDto request);
    }
}
