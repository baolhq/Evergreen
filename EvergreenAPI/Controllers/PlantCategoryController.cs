using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "Admin")]
    public class PlantCategoryController : ControllerBase
    {
        private readonly IPlantCategoryRepository _plantCategoryRepository;
        private readonly IMapper _mapper;

        public PlantCategoryController(IPlantCategoryRepository plantCategoryRepository, IMapper mapper)
        {
            _plantCategoryRepository = plantCategoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetPlantCategories()
        {
            var plantCategories = _mapper.Map<List<PlantCategoryDTO>>(_plantCategoryRepository.GetPlantCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(plantCategories);
        }

        [HttpGet("{PlantCategoryId}")]
        [AllowAnonymous]
        public IActionResult GetPlantCategory(int PlantCategoryId)
        {
            if (!_plantCategoryRepository.PlantCategoryExist(PlantCategoryId))
                return NotFound($"Plant Category '{PlantCategoryId}' is not exists!!");

            var plantCategory = _mapper.Map<PlantCategoryDTO>(_plantCategoryRepository.GetPlantCategory(PlantCategoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(plantCategory);
        }

        [HttpPost]
        public IActionResult CreatePlantCategory([FromBody] PlantCategoryDTO plantCateCreate)
        {
            if (plantCateCreate == null)
                return BadRequest(ModelState);

            var plantCate = _plantCategoryRepository.GetPlantCategories()
                .Where(c => c.Name.Trim().ToUpper() == plantCateCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (plantCate != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plantCateMap = _mapper.Map<PlantCategory>(plantCateCreate);

            if (!_plantCategoryRepository.CreatePlantCategory(plantCateMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{PlantCategoryId}")]
        public IActionResult UpdatePlantCategory(int PlantCategoryId, [FromBody] PlantCategoryDTO updatedPlantCate)
        {
            if (updatedPlantCate == null)
                return BadRequest(ModelState);

            if (PlantCategoryId != updatedPlantCate.PlantCategoryId)
                return BadRequest(ModelState);

            if (!_plantCategoryRepository.PlantCategoryExist(PlantCategoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plantCateMap = _mapper.Map<PlantCategory>(updatedPlantCate);

            if (!_plantCategoryRepository.UpdatePlantCategory(plantCateMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{PlantCategoryId}")]
        public IActionResult DeletePlantCategory(int PlantCategoryId)
        {
            if (!_plantCategoryRepository.PlantCategoryExist(PlantCategoryId))
                return NotFound();

            var plantCateToDelete = _plantCategoryRepository.GetPlantCategory(PlantCategoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_plantCategoryRepository.DeletePlantCategory(plantCateToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
