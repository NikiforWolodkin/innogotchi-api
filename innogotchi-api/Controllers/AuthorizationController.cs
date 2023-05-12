using BusinessLayer.RequestDtos;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace innogotchi_api.Controllers
{
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthorizationController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet("api/authorize")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> AuthorizeUserAsync(UserLoginDto request)
        {
            var user = await _userService.GetUserAsync(request.Email);

            if (!await _userService.ValidatePassword(user.Id, request.Password))
            {
                return BadRequest(new { Error = "Incorrect password."});
            }

            var token = CreateToken(user.Id, request.Email, user.FirstName, user.LastName);

            throw new NotImplementedException();
        }

        private string CreateToken(Guid id, string email, string firstName, string lastName)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
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
                expires: DateTime.Now.AddMinutes(20)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
