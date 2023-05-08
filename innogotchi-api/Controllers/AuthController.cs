using FluentValidation;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAdapter _userAdapter;
        private readonly IValidator<UserRequestDto> _validator;

        public AuthController(IConfiguration configuration, IUserAdapter userAdapter, IValidator<UserRequestDto> validator)
        {
            _configuration = configuration;
            _userAdapter = userAdapter;
            _validator = validator;
        }

        [HttpPost("signup")]
        [ProducesResponseType(201, Type = typeof(UserResponseDto))]
        public IActionResult SignUp(UserRequestDto request)
        {
            if (_userAdapter.UserExists(request.Email))
            {
                return BadRequest("User with this E-Mail already exists.");
            }

            var result = _validator.Validate(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            var user = _userAdapter.AddUser(request);

            return Created($"api/users/{user.Email}", user);
        }

        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult Login(UserRequestDto request)
        {
            if (!_userAdapter.UserExists(request.Email))
            {
                return BadRequest("User with this E-Mail doesn't exist.");
            }

            if (!_userAdapter.ValidateUserPassword(request.Email, request.Password))
            {
                return BadRequest("Incorrect password.");
            }

            var user = _userAdapter.GetUser(request.Email);

            var token = CreateToken(request.Email, user.FirstName, user.LastName);

            return Ok(token);
        }

        private string CreateToken(string email, string firstName, string lastName)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, firstName),
                new Claim(ClaimTypes.Surname, lastName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Token").Value!
            ));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
