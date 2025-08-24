using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _platformRepo;
        public PlatformController(IPlatformRepo repo, IMapper mapper)
        {
            _platformRepo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            Console.WriteLine(" ---  GetPlatforms Controller Method --- ");
            var platformItems = _platformRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformItems));
        }
        [HttpGet("{id}", Name ="GetPlatformById")]
        public ActionResult<PlatformReadDTO> GetPlatformById(int id)
        {
            var platform = _platformRepo.GetPlatformById(id);
            if (platform != null)
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platform));
            }
            return NotFound();
        }
        [HttpPost]
        public ActionResult<PlatformReadDTO> CreatePlatform(PlatformCreateDTO platformCreateDTO)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDTO);
            _platformRepo.CreatePlatform(platformModel);
            _platformRepo.SaveChanges();

            var platformReadDTO = _mapper.Map<PlatformReadDTO>(platformModel);

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDTO.Id }, platformReadDTO);
        }

    }
}