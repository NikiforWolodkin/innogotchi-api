using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;

namespace BusinessLayer.Interfaces
{
    public interface IInnogotchiService
    {
        Task<bool> CanFeedInnogotchiAsync(string name, Guid userId);
        Task<ICollection<InnogotchiDto>> GetInnogotchisAsync();
        Task<ICollection<InnogotchiDto>> GetInnogotchisAsync(string farmName);
        Task<InnogotchiDto> GetInnogotchiAsync(string name);
        Task<string> AddInnogotchiAsync(InnogotchiAddDto request, string farmName);
        Task<InnogotchiDto> FeedOrQuenchInnogotchiAsync(string name, InnogotchiFeedOrQuenchDto request);
    }
}
