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
    public class DiseaseController : ControllerBase
    {
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IMapper _mapper;

        public DiseaseController(IDiseaseRepository diseaseRepository, IMapper mapper)
        {
            _diseaseRepository = diseaseRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetDiseases()
        {
            var diseases = _mapper.Map<List<DiseaseDTO>>(_diseaseRepository.GetDiseases());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(diseases);
        }

        [HttpGet("{DiseaseId}")]
        public IActionResult GetDisease(int DiseaseId)
        {
            if (!_diseaseRepository.DiseaseExist(DiseaseId))
                return NotFound($"Disease '{DiseaseId}' is not exists!!");

            var disease = _mapper.Map<DiseaseDTO>(_diseaseRepository.GetDisease(DiseaseId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(disease);
        }

        [HttpPost]
        public IActionResult CreateDisease([FromBody] DiseaseDTO diseaseCreate)
        {
            if (diseaseCreate == null)
                return BadRequest(ModelState);

            var disease = _diseaseRepository.GetDiseases()
                .Where(c => c.Name.Trim().ToUpper() == diseaseCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (disease != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var diseaseMap = _mapper.Map<Disease>(diseaseCreate);

            if (!_diseaseRepository.CreateDisease(diseaseMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{DiseaseId}")]
        public IActionResult UpdateDisease(int DiseaseId, [FromBody] DiseaseDTO updatedDisease)
        {
            if (updatedDisease == null)
                return BadRequest(ModelState);

            if (DiseaseId != updatedDisease.DiseaseId)
                return BadRequest(ModelState);

            if (!_diseaseRepository.DiseaseExist(DiseaseId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var diseaseMap = _mapper.Map<Disease>(updatedDisease);

            if (!_diseaseRepository.UpdateDisease(diseaseMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{DiseaseId}")]
        public IActionResult DeleteDisease(int DiseaseId)
        {
            if (!_diseaseRepository.DiseaseExist(DiseaseId))
                return NotFound();

            var diseaseToDelete = _diseaseRepository.GetDisease(DiseaseId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_diseaseRepository.DeleteDisease(diseaseToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
