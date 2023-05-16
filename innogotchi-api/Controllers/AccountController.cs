using BusinessLayer.Interfaces;
using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;
using DataLayer.Enums;
using FluentValidation;
using InnogotchiApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Dtos;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace InnogotchiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFarmService _farmService;
        private readonly IInnogotchiService _innogotchiService;
        private readonly ICollaborationService _collaborationService;
        private readonly IAvatarService _avatarService;
        private readonly IValidator<FarmAddDto> _addFarmValidator;
        private readonly IValidator<InnogotchiAddDto> _innogotchiAddValidator;
        private readonly IValidator<UserUpdatePasswordDto> _userUpdatePasswordValidator;
        private readonly IValidator<UserUpdateProfileDto> _userUpdateProfileValidator;

        public AccountController(IUserService userService, IFarmService farmService,
            IInnogotchiService innochiService, ICollaborationService collaborationService,
            IAvatarService avatarService, IValidator<FarmAddDto> addFarmValidator,
            IValidator<InnogotchiAddDto> innogotchiAddValidator, IValidator<UserUpdatePasswordDto> userUpdatePasswordValidator,
            IValidator<UserUpdateProfileDto> userUpdateProfileValidator)
        {
            _userService = userService;
            _farmService = farmService;
            _innogotchiService = innochiService;
            _collaborationService = collaborationService;
            _addFarmValidator = addFarmValidator;
            _innogotchiAddValidator = innogotchiAddValidator;
            _avatarService = avatarService;
            _userUpdatePasswordValidator = userUpdatePasswordValidator;
            _userUpdateProfileValidator = userUpdateProfileValidator;
        }

        [HttpGet, Authorize]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var id = JwtClaims.GetId(User.Identity);

            return Ok(await _userService.GetUserAsync(id));
        }



        [HttpPut("password"), Authorize]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateCurrentUserPassword(UserUpdatePasswordDto request)
        {
            var id = JwtClaims.GetId(User.Identity);

            if (!await _userService.ValidatePassword(id, request.OldPassword))
            {
                return BadRequest(new { Error = "Incorrect password." });
            }

            var results = await _userUpdatePasswordValidator.ValidateAsync(request);

            if (!results.IsValid)
            {
                return BadRequest(results.Errors.Select(err => new { Error = err.ErrorMessage }));
            }

            return Ok(await _userService.UpdateUserPasswordAsync(id, request.NewPassword));
        }

        [HttpPut, Authorize]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateCurrentUserProfile(UserUpdateProfileDto request)
        {
            var id = JwtClaims.GetId(User.Identity);

            var results = await _userUpdateProfileValidator.ValidateAsync(request);

            if (!results.IsValid)
            {
                return BadRequest(results.Errors.Select(err => new { Error = err.ErrorMessage }));
            }

            return Ok(await _userService.UpdateUserProfileAsync(id, request));
        }


        [HttpGet("avatar"), Authorize]
        [ProducesResponseType(200, Type = typeof(AvatarDto))]
        public async Task<IActionResult> GetCurrentUserAvatarAsync()
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            return Ok(await _avatarService.GetAvatarAsync((Guid)user.AvatarId));
        }

        [HttpPost("avatar"), Authorize]
        [ProducesResponseType(201, Type = typeof(string))]
        public async Task<IActionResult> AddCurrentUserAvatarAsync(IFormFile image)
        {
            var id = JwtClaims.GetId(User.Identity);

            await _avatarService.AddAvatarAsync(image, id);

            return Created("api/account/avatar", "avatar");
        }



        [HttpGet("collaborations"), Authorize]
        [ProducesResponseType(200, Type = typeof(CollaborationDto))]
        public async Task<IActionResult> GetCurrentUserCollaborationsAsync()
        {
            var id = JwtClaims.GetId(User.Identity);

            return Ok(await _collaborationService.GetCollaborationsAsync(id));
        }

        [HttpPut("collaborations/{farmName}"), Authorize]
        [ProducesResponseType(200, Type = typeof(CollaborationDto))]
        public async Task<IActionResult> GetCurrentUserCollaborationsAsync(string farmName, CollaborationChangeStatusDto request)
        {
            var id = JwtClaims.GetId(User.Identity);

            return Ok(await _collaborationService.ChangeCollaborationStatusAsync(farmName, id, request));
        }



        [HttpGet("farm"), Authorize]
        [ProducesResponseType(200, Type = typeof(FarmDto))]
        public async Task<IActionResult> GetCurrentUserFarm()
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            await _farmService.UpdateFarmInnogotchisAsync(user.FarmName);

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

            var user = await _userService.GetUserAsync(id);

            var farm = await _farmService.AddFarmAsync(request, user.Id);

            return Created($"api/farms/{farm}", farm);
        }

        [HttpDelete("farm"), Authorize]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteCurrentUserFarm()
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            await _farmService.DeleteFarmAsync(user.FarmName);

            return NoContent();
        }



        [HttpGet("farm/collaborations"), Authorize]
        [ProducesResponseType(200, Type = typeof(ICollection<CollaborationDto>))]
        public async Task<IActionResult> GetCurrentUserFarmCollaborations()
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            return Ok(await _collaborationService.GetCollaborationsAsync(user.FarmName));
        }

        [HttpPost("farm/collaborations"), Authorize]
        [ProducesResponseType(201, Type = typeof(CollaborationStatus))]
        public async Task<IActionResult> AddCurrentUserFarmCollaboration(CollaborationAddDto request)
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            await _collaborationService.AddCollaborationAsync(user.FarmName, request);

            return Created("api/farm/collaborations", CollaborationStatus.Pending);
        }



        [HttpGet("farm/innogotchis"), Authorize]
        [ProducesResponseType(200, Type = typeof(ICollection<InnogotchiDto>))]
        public async Task<IActionResult> GetCurrentUserFarmInnogotchis()
        {
            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            return Ok(await _innogotchiService.GetInnogotchisAsync(user.FarmName));
        }

        [HttpGet("farm/innogotchis/{name}"), Authorize]
        [ProducesResponseType(200, Type = typeof(InnogotchiDto))]
        public async Task<IActionResult> GetCurrentUserFarmInnogotchi(string name)
        {
            var id = JwtClaims.GetId(User.Identity);

            return Ok(await _innogotchiService.GetInnogotchiAsync(name));
        }

        [HttpPut("farm/innogotchis/{name}"), Authorize]
        [ProducesResponseType(200, Type = typeof(InnogotchiDto))]
        public async Task<IActionResult> AddCurrentUserFarmInnogotchiFeedingAndQuenching(string name, InnogotchiFeedOrQuenchDto request)
        {
            var id = JwtClaims.GetId(User.Identity);

            if (!await _innogotchiService.CanFeedInnogotchiAsync(name, id))
            {
                return BadRequest("You can't feed this innogotchi.");
            }

            return Ok(await _innogotchiService.FeedOrQuenchInnogotchiAsync(name, request));
        }

        [HttpPost("farm/innogotchis"), Authorize]
        [ProducesResponseType(201, Type = typeof(string))]
        public async Task<IActionResult> AddCurrentUserFarmInnogotchi(InnogotchiAddDto request)
        {
            var results = await _innogotchiAddValidator.ValidateAsync(request);

            if (!results.IsValid)
            {
                return BadRequest(results.Errors.Select(err => new { Error = err.ErrorMessage }));
            }

            var id = JwtClaims.GetId(User.Identity);

            var user = await _userService.GetUserAsync(id);

            var innogotchi = await _innogotchiService.AddInnogotchiAsync(request, user.FarmName);

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
