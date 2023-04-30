using AutoMapper;
using innogotchi_api.Data;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using innogotchi_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmsController : Controller
    {
        private readonly IFarmAdapter _farmAdapter;

        public FarmsController(IFarmAdapter farmAdapter)
        {
            _farmAdapter = farmAdapter;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(ICollection<FarmDto>))]
        public IActionResult GetFarms()
        {
            var farms = _farmAdapter.GetFarms();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(farms);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(FarmDto))]
        public IActionResult GetFarm(string name)
        {
            if (!_farmAdapter.FarmExists(name))
            {
                return NotFound();
            }

            var user = _farmAdapter.GetFarm(name);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }
    }
}
