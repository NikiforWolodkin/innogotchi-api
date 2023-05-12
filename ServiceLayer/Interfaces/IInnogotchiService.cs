using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;

namespace BusinessLayer.Interfaces
{
    public interface IInnogotchiService
    {
        Task<ICollection<InnogotchiDto>> GetInnogotchisAsync();
        Task<ICollection<InnogotchiDto>> GetInnogotchisAsync(string farmName);
        Task<InnogotchiDto> GetInnogotchiAsync(string name);
        Task<string> AddInnogotchiAsync(InnogotchiAddDto request, string farmName);
        Task<InnogotchiDto> FeedOrQuenchInnogotchiAsync(string name, InnogotchiFeedOrQuenchDto request);
    }
}
