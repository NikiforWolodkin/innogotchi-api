using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using innogotchi_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InnogotchisController : Controller
    {
        private readonly IInnogotchiAdapter _innogotchiAdapter;

        public InnogotchisController(IInnogotchiAdapter innogotchiAdapter)
        {
            _innogotchiAdapter = innogotchiAdapter;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<InnogotchiDto>))]
        public IActionResult GetInnogotchis()
        {
            var innogotchis = _innogotchiAdapter.GetInnogotchis();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(innogotchis);
        }

        [HttpGet("name")]
        [ProducesResponseType(200, Type = typeof(InnogotchiDto))]
        public IActionResult GetInnogotchi(string name)
        {
            if (!_innogotchiAdapter.InnogotchiExists(name))
            {
                return NotFound();
            }

            var innogotchi = _innogotchiAdapter.GetInnogotchi(name);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(innogotchi);
        }
    }
}
