using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IFarmService
    {
        Task<ICollection<FarmDto>> GetFarmsAsync();
        Task<ICollection<FarmDto>> GetFarmsAsync(Guid collaboratorId);
        Task<FarmDto> GetFarmAsync(string name);
        Task<string> AddFarmAsync(FarmAddDto request, Guid userId);
        Task DeleteFarmAsync(string name);
        Task UpdateFarmInnogotchisAsync(string name);
    }
}
