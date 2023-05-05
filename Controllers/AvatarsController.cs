using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AvatarsController : Controller
    {
        private readonly IAvatarAdapter _avatarAdapter;

        public AvatarsController(IAvatarAdapter avatarAdapter)
        {
            _avatarAdapter = avatarAdapter;
        }

        [HttpGet("ids")]
        [ProducesResponseType(200, Type = typeof(ICollection<AvatarIdDto>))]
        public IActionResult GetAvatars()
        {
            return Ok(_avatarAdapter.GetAvatarIds());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(IFormFile))]
        public IActionResult GetAvatar(int id)
        {
            if (_avatarAdapter.AvatarExists(id))
            {
                return NotFound("Avatar not found.");
            }

            return Ok(_avatarAdapter.GetAvatar(id));
        }

        [HttpPost("ids")]
        [ProducesResponseType(200, Type = typeof(AvatarIdDto))]
        public IActionResult AddAvatar(IFormFile image)
        {
            return Ok(_avatarAdapter.AddAvatar(image));
        }
    }
}
