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
    public class DiseaseCategoryController : ControllerBase
    {
        private readonly IDiseaseCategoryRepository _diseaseCategoryRepository;
        private readonly IMapper _mapper;

        public DiseaseCategoryController(IDiseaseCategoryRepository diseaseCategoryRepository, IMapper mapper)
        {
            _diseaseCategoryRepository = diseaseCategoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetDiseaseCategories()
        {
            var diseaseCategories = _mapper.Map<List<DiseaseCategoryDTO>>(_diseaseCategoryRepository.GetDiseaseCategories());

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(diseaseCategories);
        }

        [HttpGet("{DiseaseCategoryID}")]
        public IActionResult GetDiseaseCategory(int DiseaseCategoryID)
        {
            if (!_diseaseCategoryRepository.DiseaseCategoryExist(DiseaseCategoryID)) 
                return NotFound($"Disease Category '{DiseaseCategoryID}' is not exists!!");

            var diseaseCategory = _mapper.Map<DiseaseCategoryDTO>(_diseaseCategoryRepository.GetDiseaseCategory(DiseaseCategoryID));

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(diseaseCategory);
        }

        [HttpPost]
        public IActionResult CreateDiseaseCategory([FromBody] DiseaseCategoryDTO diseaseCateCreate)
        {
            if (diseaseCateCreate == null)
                return BadRequest(ModelState);

            var diseaseCate = _diseaseCategoryRepository.GetDiseaseCategories()
                .Where(c => c.Name.Trim().ToUpper() == diseaseCateCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (diseaseCate != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var diseaseCategoryMap = _mapper.Map<DiseaseCategory>(diseaseCateCreate);

            if (!_diseaseCategoryRepository.CreateDiseaseCategory(diseaseCategoryMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{DiseaseCategoryID}")]
        public IActionResult UpdateDiseaseCategory(int DiseaseCategoryID, [FromBody] DiseaseCategoryDTO updatedDiseaseCategory)
        {
            if (updatedDiseaseCategory == null) 
                return BadRequest(ModelState);

            if (DiseaseCategoryID != updatedDiseaseCategory.DiseaseCategoryId) 
                return BadRequest(ModelState);

            if (!_diseaseCategoryRepository.DiseaseCategoryExist(DiseaseCategoryID)) 
                return NotFound();

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var diseaseCategoryMap = _mapper.Map<DiseaseCategory>(updatedDiseaseCategory);

            if (!_diseaseCategoryRepository.UpdateDiseaseCategory(diseaseCategoryMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{DiseaseCategoryID}")]
        public IActionResult DeleteDiseaseCategory(int DiseaseCategoryID)
        {
            if (!_diseaseCategoryRepository.DiseaseCategoryExist(DiseaseCategoryID)) 
                return NotFound();

            var diseaseCateToDelete = _diseaseCategoryRepository.GetDiseaseCategory(DiseaseCategoryID);

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            if (!_diseaseCategoryRepository.DeleteDiseaseCategory(diseaseCateToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
