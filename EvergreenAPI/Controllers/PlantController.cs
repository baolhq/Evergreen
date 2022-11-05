using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly IPlantRepository _plantRepository;
        private readonly IMapper _mapper;

        public PlantController(IPlantRepository plantRepository, IMapper mapper)
        {
            _plantRepository = plantRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPlants()
        {
            var plant = _mapper.Map<List<Plant>>(_plantRepository.GetPlants());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(plant);
        }

        [HttpGet("{PlantId}")]
        public IActionResult GetPlant(int PlantId)
        {
            if (!_plantRepository.PlantExist(PlantId))
                return NotFound($"Plant '{PlantId}' is not exists!!");

            var plant = _mapper.Map<Plant>(_plantRepository.GetPlant(PlantId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(plant);
        }

        [HttpPost]
        public IActionResult CreatePlant([FromBody] PlantDTO plantCreate)
        {
            if (plantCreate == null)
                return BadRequest(ModelState);

            var plant = _plantRepository.GetPlants()
                .Where(c => c.Name.Trim().ToUpper() == plantCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (plant != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plantMap = _mapper.Map<Plant>(plantCreate);

            if (!_plantRepository.CreatePlant(plantMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{PlantId}")]
        public IActionResult UpdatePlant(int PlantId, [FromBody] PlantDTO updatedPlant)
        {
            if (updatedPlant == null)
                return BadRequest(ModelState);

            if (PlantId != updatedPlant.PlantId)
                return BadRequest(ModelState);

            if (!_plantRepository.PlantExist(PlantId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plantMap = _mapper.Map<Plant>(updatedPlant);

            if (!_plantRepository.UpdatePlant(plantMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{PlantId}")]
        public IActionResult DeletePlant(int PlantId)
        {
            if (!_plantRepository.PlantExist(PlantId))
                return NotFound();

            var plantToDelete = _plantRepository.GetPlant(PlantId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_plantRepository.DeletePlant(plantToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
