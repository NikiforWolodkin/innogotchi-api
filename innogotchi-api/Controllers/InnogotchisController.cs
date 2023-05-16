using BusinessLayer.Interfaces;
using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;
using InnogotchiApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InnogotchisController : Controller
    {
        private readonly IInnogotchiService _innogotchiService;

        public InnogotchisController(IInnogotchiService innogotchiService)
        {
            _innogotchiService = innogotchiService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<InnogotchiDto>))]
        public async Task<IActionResult> GetInnogotchis()
        {
            return Ok(await _innogotchiService.GetInnogotchisAsync());
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(ICollection<InnogotchiDto>))]
        public async Task<IActionResult> GetInnogotchi(string name)
        {
            return Ok(await _innogotchiService.GetInnogotchiAsync(name));
        }

        [HttpPut("{name}"), Authorize]
        [ProducesResponseType(200, Type = typeof(ICollection<InnogotchiDto>))]
        public async Task<IActionResult> FeedOrQuenchInnogotchi(string name, InnogotchiFeedOrQuenchDto request)
        {
            var id = JwtClaims.GetId(User.Identity);

            if (!await _innogotchiService.CanFeedInnogotchiAsync(name, id))
            {
                return BadRequest("You can't feed this innogotchi.");
            }

            return Ok(await _innogotchiService.FeedOrQuenchInnogotchiAsync(name, request));
        }
    }
}
