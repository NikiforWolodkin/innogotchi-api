using Azure.Core;
using BusinessLayer.Interfaces;
using BusinessLayer.RequestDtos;
using BusinessLayer.ResponseDtos;
using FluentValidation;
using innogotchi_api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using ServiceLayer.Dtos;
using ServiceLayer.Interfaces;
using ServiceLayer.RequestDtos;

namespace ClientLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFarmService _farmService;
        private readonly IValidator<UserSignupDto> _signupValidator;

        public UsersController(IUserService userService, IFarmService farmService, IValidator<UserSignupDto> signupValidator)
        {
            _userService = userService;
            _farmService = farmService;
            _signupValidator = signupValidator;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<UserDto>))]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public async Task<IActionResult> GetUserAsync(Guid id)
        {
            return Ok(await _userService.GetUserAsync(id));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Guid))]
        public async Task<IActionResult> AddUserAsync(UserSignupDto request)
        {
            var results = await _signupValidator.ValidateAsync(request);

            if (!results.IsValid)
            {
                return BadRequest(results.Errors.Select(err => new { Error = err.ErrorMessage }));
            }

            var userId = _userService.AddUser(request);

            return Created($"api/users/{userId}", userId);
        }
    }
}
