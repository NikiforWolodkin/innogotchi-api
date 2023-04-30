using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(200, Type = typeof(ICollection<UserDto>))]
        public IActionResult GetUsers()
        {
            var users = _userAdapter.GetUsers();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpGet("{email}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public IActionResult GetUser(string email)
        {
            if (!_userAdapter.UserExists(email))
            {
                return NotFound();
            }

            var user = _userAdapter.GetUser(email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }
    }
}
