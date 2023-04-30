using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace innogotchi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollaborationFarmsController : Controller
    {
        private readonly IFarmRepository _farmRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CollaborationFarmsController(IFarmRepository farmRepository,IUserRepository userRepository, IMapper mapper)
        {
            _farmRepository = farmRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<FarmDto>))]
        public IActionResult GetCollaborations()
        {
            var farms = _mapper.Map<ICollection<FarmDto>>(_farmRepository.GetCollaborationFarms());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(farms);
        }

        [HttpGet("email")]
        [ProducesResponseType(200, Type = typeof(ICollection<FarmDto>))]
        public IActionResult GetCollaborations(string email)
        {
            if (!_userRepository.UserExists(email))
            {
                return NotFound();
            }

            var farms = _mapper.Map<ICollection<FarmDto>>(_farmRepository.GetCollaborationFarms(email));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(farms);
        }
    }
}
