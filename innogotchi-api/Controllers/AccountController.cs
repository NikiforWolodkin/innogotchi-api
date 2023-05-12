using BusinessLayer.Interfaces;
using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;
using FluentValidation;
using innogotchi_api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Dtos;
using ServiceLayer.Interfaces;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFarmService _farmService;
        private readonly IInnogotchiService _innochiService;
        private readonly IValidator<FarmAddDto> _addFarmValidator;

        public AccountController(IUserService userService, IFarmService farmService,
            IInnogotchiService innochiService, IValidator<FarmAddDto> addFarmValidator)
        {
            _userService = userService;
            _farmService = farmService;
            _innochiService = innochiService;
            _addFarmValidator = addFarmValidator;
        }

        [HttpGet, Authorize]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var id = JwtClaims.GetId(User.Identity);

            return Ok(await _userService.GetUserAsync(id));
        }



        [HttpGet("farm"), Authorize]
        [ProducesResponseType(200, Type = typeof(FarmDto))]
        public async Task<IActionResult> GetCurrentUserFarm()
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            return Ok(await _farmService.GetFarmAsync(user.FarmName));
        }

        [HttpPost("farm"), Authorize]
        [ProducesResponseType(201, Type = typeof(string))]
        public async Task<IActionResult> AddCurrentUserFarm(FarmAddDto request)
        {
            var results = await _addFarmValidator.ValidateAsync(request);

            if (!results.IsValid)
            {
                return BadRequest(results.Errors.Select(err => new { Error = err.ErrorMessage }));
            }

            var id = JwtClaims.GetId(User.Identity);

            var farm = await _farmService.AddFarmAsync(request, id);

            return Created($"api/farms/{farm}", farm);
        }

        [HttpDelete("farm"), Authorize]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteCurrentUserFarm()
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            _farmService.DeleteFarmAsync(user.FarmName);

            return NoContent();
        }



        [HttpGet("farm/innogotchis"), Authorize]
        [ProducesResponseType(200, Type = typeof(ICollection<InnogotchiDto>))]
        public async Task<IActionResult> GetCurrentUserFarmInnogotchis()
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            return Ok(await _innochiService.GetInnogotchisAsync(user.FarmName));
        }

        [HttpGet("farm/innogotchis/{name}"), Authorize]
        [ProducesResponseType(200, Type = typeof(InnogotchiDto))]
        public async Task<IActionResult> GetCurrentUserFarmInnogotchi(string name)
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            return Ok(await _innochiService.GetInnogotchiAsync(name));
        }

        [HttpPost("farm/innogotchis"), Authorize]
        [ProducesResponseType(201, Type = typeof(string))]
        public async Task<IActionResult> AddCurrentUserFarmInnogotchi(InnogotchiAddDto request)
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            var innogotchi = _innochiService.AddInnogotchiAsync(request, user.FarmName);

            return Created($"api/farms/{user.FarmName}/innogotchis/{innogotchi}", innogotchi);
        }



        [HttpGet("collaboration-farms"), Authorize]
        [ProducesResponseType(200, Type = typeof(ICollection<FarmDto>))]
        public async Task<IActionResult> GetCurrentUserCollaborations()
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            return Ok(await _farmService.GetFarmsAsync(user.Id));
        }

    }
}
