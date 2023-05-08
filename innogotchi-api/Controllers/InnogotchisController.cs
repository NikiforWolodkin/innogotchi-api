using AutoMapper;
using Azure.Core;
using innogotchi_api.Dtos;
using innogotchi_api.Helpers;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using innogotchi_api.Repositories;
using innogotchi_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InnogotchisController : Controller
    {
        private readonly IInnogotchiAdapter _innogotchiAdapter;
        private readonly IUserAdapter _userAdapter;

        public InnogotchisController(IInnogotchiAdapter innogotchiAdapter, IUserAdapter userAdapter)
        {
            _innogotchiAdapter = innogotchiAdapter;
            _userAdapter = userAdapter;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<InnogotchiResponseDto>))]
        public IActionResult GetInnogotchis()
        {
            var innogotchis = _innogotchiAdapter.GetInnogotchis();

            return Ok(innogotchis);
        }

        [HttpGet("name")]
        [ProducesResponseType(200, Type = typeof(InnogotchiResponseDto))]
        public IActionResult GetInnogotchi(string name)
        {
            if (!_innogotchiAdapter.InnogotchiExists(name))
            {
                return NotFound();
            }

            var innogotchi = _innogotchiAdapter.GetInnogotchi(name);

            return Ok(innogotchi);
        }

        [HttpPost, Authorize]
        [ProducesResponseType(201, Type = typeof(InnogotchiResponseDto))]
        public IActionResult AddInnogotchi(InnogotchiRequestDto request)
        {
            var email = JwtClaims.GetEmail((ClaimsIdentity)User.Identity);

            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            if (_innogotchiAdapter.InnogotchiExists(request.Name))
            {
                return BadRequest("Innogotchi already exists.");
            }

            var user = _userAdapter.GetUser(email);

            if (user.FarmName is null)
            {
                return BadRequest("No farm to create innogotchi.");
            }

            var innogotchi = _innogotchiAdapter.AddInnogotchi(request, user.FarmName);

            return Created($"api/innogotchis/{innogotchi.Name}", innogotchi);
        }

        [HttpPut("{name}/feed"), Authorize]
        [ProducesResponseType(200, Type = typeof(InnogotchiResponseDto))]
        public IActionResult FeedInnogotchi(string name)
        {
            var email = JwtClaims.GetEmail((ClaimsIdentity)User.Identity);

            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            if (!_innogotchiAdapter.InnogotchiExists(name))
            {
                return NotFound("Innogotchi not found.");
            }
            
            if (!_innogotchiAdapter.CanFeedInnogotchi(name, email))
            {
                return BadRequest("Can't feed Innogotchi.");
            }

            var innogotchi = _innogotchiAdapter.AddInnogotchiFeedingAndQuenching(name, "FEED");

            return Ok(innogotchi);
        }

        [HttpPut("{name}/give-water"), Authorize]
        [ProducesResponseType(200, Type = typeof(InnogotchiResponseDto))]
        public IActionResult GiveWaterToInnogotchi(string name)
        {
            var email = JwtClaims.GetEmail((ClaimsIdentity)User.Identity);

            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            if (!_innogotchiAdapter.InnogotchiExists(name))
            {
                return NotFound("Innogotchi not found.");
            }

            if (!_innogotchiAdapter.CanFeedInnogotchi(name, email))
            {
                return BadRequest("Can't feed Innogotchi.");
            }

            var innogotchi = _innogotchiAdapter.AddInnogotchiFeedingAndQuenching(name, "GIVE_WATER");

            return Ok(innogotchi);
        }
    }
}
