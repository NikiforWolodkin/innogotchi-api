using BusinessLayer.Interfaces;
using BusinessLayer.ResponseDtos;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Services;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmsController : Controller
    {
        private readonly IFarmService _farmService;
        private readonly IInnogotchiService _innogotchiService;

        public FarmsController(IFarmService farmService, IInnogotchiService innogotchiService)
        {
            _farmService = farmService;
            _innogotchiService = innogotchiService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<FarmDto>))]
        public async Task<IActionResult> GetFarmsAsync()
        {
            return Ok(await _farmService.GetFarmsAsync());
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(FarmDto))]
        public async Task<IActionResult> GetFarmASync(string name)
        {
            return Ok(await _farmService.GetFarmAsync(name));
        }

        [HttpGet("{name}/innogotchis")]
        [ProducesResponseType(200, Type = typeof(FarmDto))]
        public async Task<IActionResult> GetFarmInnogotchis(string name)
        {
            return Ok(await _innogotchiService.GetInnogotchisAsync(name));
        }
    }
}
