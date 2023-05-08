using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Helpers;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserAdapter _userAdapter;

        public UsersController(IUserAdapter userAdapter)
        {
            _userAdapter = userAdapter;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<UserResponseDto>))]
        public IActionResult GetUsers()
        {
            var users = _userAdapter.GetUsers();

            return Ok(users);
        }

        [HttpGet("{email}")]
        [ProducesResponseType(200, Type = typeof(UserResponseDto))]
        public IActionResult GetUser(string email)
        {
            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            var user = _userAdapter.GetUser(email);

            return Ok(user);
        }

        [HttpGet("me"), Authorize]
        [ProducesResponseType(200, Type = typeof(UserResponseDto))]
        public IActionResult GetUser()
        {
            var email = JwtClaims.GetEmail((ClaimsIdentity)User.Identity);

            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            var user = _userAdapter.GetUser(email);

            return Ok(user);
        }

        [HttpPut("me"), Authorize]
        [ProducesResponseType(200, Type = typeof(UserResponseDto))]
        public IActionResult UpdateUser(UserRequestDto request)
        {
            var email = JwtClaims.GetEmail((ClaimsIdentity)User.Identity);

            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            if (request.OldPassword is not null)
            {
                if (!_userAdapter.ValidateUserPassword(email, request.OldPassword))
                {
                    return BadRequest("Password is incorrect.");
                }
            }
            else
            {
                request.Password = null;
            }

            request.Email = email;

            var user = _userAdapter.UpdateUser(request);

            return Ok(user);
        }

        [HttpDelete("me"), Authorize]
        [ProducesResponseType(204)]
        public IActionResult DeleteUser()
        {
            var email = JwtClaims.GetEmail((ClaimsIdentity)User.Identity);

            if (!_userAdapter.UserExists(email))
            {
                return NotFound("User not found.");
            }

            _userAdapter.DeleteUser(email);

            return NoContent();
        }
    }
}
