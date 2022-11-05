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
    public class TreatmentController : ControllerBase
    {
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IMapper _mapper;

        public TreatmentController(ITreatmentRepository treatmentRepository, IMapper mapper)
        {
            _treatmentRepository = treatmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTreatments()
        {
            var treatments = _mapper.Map<List<Treatment>>(_treatmentRepository.GetTreatments());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(treatments);
        }

        [HttpGet("{TreatmentId}")]
        public IActionResult GetTreatment(int TreatmentId)
        {
            if (!_treatmentRepository.TreatmentExist(TreatmentId))
                return NotFound($"Treatment '{TreatmentId}' is not exists!!");

            var treatment = _mapper.Map<Treatment>(_treatmentRepository.GetTreatment(TreatmentId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(treatment);
        }

        [HttpPost]
        public IActionResult CreateTreatment([FromBody] TreatmentDTO treatmentCreate)
        {
            if (treatmentCreate == null)
                return BadRequest(ModelState);

            var plant = _treatmentRepository.GetTreatments()
                .Where(c => c.Method.Trim().ToUpper() == treatmentCreate.Method.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (plant != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var treatmentMap = _mapper.Map<Treatment>(treatmentCreate);

            if (!_treatmentRepository.CreateTreatment(treatmentMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{TreatmentId}")]
        public IActionResult UpdateTreatment(int TreatmentId, [FromBody] TreatmentDTO updatedTreatment)
        {
            if (updatedTreatment == null)
                return BadRequest(ModelState);

            if (TreatmentId != updatedTreatment.TreatmentId)
                return BadRequest(ModelState);

            if (!_treatmentRepository.TreatmentExist(TreatmentId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var treatmentMap = _mapper.Map<Treatment>(updatedTreatment);

            if (!_treatmentRepository.UpdateTreatment(treatmentMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{TreatmentId}")]
        public IActionResult DeletePlantCategory(int TreatmentId)
        {
            if (!_treatmentRepository.TreatmentExist(TreatmentId))
                return NotFound();

            var treatmentToDelete = _treatmentRepository.GetTreatment(TreatmentId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_treatmentRepository.DeleteTreatment(treatmentToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
