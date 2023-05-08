using AutoMapper;
using innogotchi_api.Data;
using innogotchi_api.Dtos;
using innogotchi_api.Helpers;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using innogotchi_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmsController : Controller
    {
        private readonly IFarmAdapter _farmAdapter;
        private readonly IUserAdapter _userAdapter;

        public FarmsController(IFarmAdapter farmAdapter, IUserAdapter userAdapter)
        {
            _farmAdapter = farmAdapter;
            _userAdapter = userAdapter;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(ICollection<FarmResponseDto>))]
        public IActionResult GetFarms()
        {
            var farms = _farmAdapter.GetFarms();

            return Ok(farms);
        }

        [HttpGet("collaborations")]
        [ProducesResponseType(200, Type = typeof(ICollection<FarmResponseDto>))]
        public IActionResult GetFarms(string collaboratorEmail) 
        {
            if (!_userAdapter.UserExists(collaboratorEmail))
            {
                return NotFound("User not found.");
            }

            var farms = _farmAdapter.GetFarms(collaboratorEmail);

            return Ok(farms);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(FarmResponseDto))]
        public IActionResult GetFarm(string name)
        {
            if (!_farmAdapter.FarmExists(name))
            {
                return NotFound("Farm not found.");
            }

            var farm = _farmAdapter.GetFarm(name);

            return Ok(farm);
        }

        [HttpPost, Authorize]
        [ProducesResponseType(201, Type = typeof(FarmResponseDto))]
        public IActionResult AddFarm(FarmRequestDto request)
        {
            var email = JwtClaims.GetEmail((ClaimsIdentity)User.Identity);

            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            if (_farmAdapter.FarmExists(request.Name))
            {
                return BadRequest("Farm already exists.");
            }

            var user = _userAdapter.GetUser(email);

            if (user.FarmName is not null)
            {
                return BadRequest("User already has a farm.");
            }

            var farm = _farmAdapter.AddFarm(request.Name, user.Email);

            return Created($"api/farms/{farm.Name}", farm);
        }

        [HttpDelete, Authorize]
        [ProducesResponseType(204)]
        public IActionResult DeleteFarm()
        {
            var email = JwtClaims.GetEmail((ClaimsIdentity)User.Identity);

            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            var user = _userAdapter.GetUser(email);

            if (!_farmAdapter.FarmExists(user.FarmName))
            {
                return NotFound("Farm not found.");
            }

            _farmAdapter.DeleteFarm(user.FarmName);

            return NoContent();
        }

        [HttpPost("collaborations"), Authorize]
        [ProducesResponseType(204)]
        public IActionResult AddCollaboration(CollaborationDto request)
        {
            if (!_userAdapter.UserExists(request.UserEmail))
            {
                return NotFound("User not found.");
            }

            var email = JwtClaims.GetEmail((ClaimsIdentity)User.Identity);

            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            var user = _userAdapter.GetUser(email);

            if (!_farmAdapter.FarmExists(user.FarmName))
            {
                return NotFound("User not found.");
            }

            if (_farmAdapter.FarmCollaborationExists(user.FarmName, request.UserEmail))
            {
                return BadRequest("Collaboration already exists.");
            }

            _farmAdapter.AddFarmCollaboration(user.FarmName, request.UserEmail);

            return NoContent();
        }

        [HttpDelete("collaborations"), Authorize]
        [ProducesResponseType(204)]
        public IActionResult DeleteCollaboration(CollaborationDto request)
        {
            if (!_userAdapter.UserExists(request.UserEmail))
            {
                return NotFound("User not found.");
            }

            var email = JwtClaims.GetEmail((ClaimsIdentity)User.Identity);

            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            var user = _userAdapter.GetUser(email);

            if (!_farmAdapter.FarmExists(user.FarmName))
            {
                return NotFound("User not found.");
            }

            if (!_farmAdapter.FarmCollaborationExists(user.FarmName, request.UserEmail))
            {
                return NotFound("Collaboration not found.");
            }

            _farmAdapter.DeleteFarmCollaboration(user.FarmName, request.UserEmail);

            return NoContent();
        }
    }
}
