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
    [Authorize (Roles = "Admin, Professor")]
    public class MedicineCategoryController : ControllerBase
    {
        private readonly IMedicineCategoryRepository _medicineCategoryRepository;
        private readonly IMapper _mapper;

        public MedicineCategoryController(IMedicineCategoryRepository medicineCategoryRepository, IMapper mapper)
        {
            _medicineCategoryRepository = medicineCategoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetMedicineCategories()
        {
            var medicineCategories = _mapper.Map<List<MedicineCategoryDTO>>(_medicineCategoryRepository.GetMedicineCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(medicineCategories);
        }

        [HttpGet("{MedicineCategoryId}")]
        [AllowAnonymous]
        public IActionResult GetMedicineCategory(int MedicineCategoryId)
        {
            if (!_medicineCategoryRepository.MedicineCategoryExist(MedicineCategoryId))
                return NotFound($"Medicine Category '{MedicineCategoryId}' is not exists!!");

            var medicineCategory = _mapper.Map<MedicineCategoryDTO>(_medicineCategoryRepository.GetMedicineCategory(MedicineCategoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(medicineCategory);
        }

        [HttpPost]
        public IActionResult CreateMedicineCategory([FromBody] MedicineCategoryDTO medicineCateCreate)
        {
            if (medicineCateCreate == null)
                return BadRequest(ModelState);

            var medicineCate = _medicineCategoryRepository.GetMedicineCategories()
                .Where(c => c.Name.Trim().ToUpper() == medicineCateCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (medicineCate != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicineCateMap = _mapper.Map<MedicineCategory>(medicineCateCreate);

            if (!_medicineCategoryRepository.CreateMedicineCategory(medicineCateMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{MedicineCategoryId}")]
        public IActionResult UpdateMedicineCategory(int MedicineCategoryId, [FromBody] MedicineCategoryDTO updatedMedicineCate)
        {
            if (updatedMedicineCate == null)
                return BadRequest(ModelState);

            if (MedicineCategoryId != updatedMedicineCate.MedicineCategoryId)
                return BadRequest(ModelState);

            if (!_medicineCategoryRepository.MedicineCategoryExist(MedicineCategoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicineCateMap = _mapper.Map<MedicineCategory>(updatedMedicineCate);

            if (!_medicineCategoryRepository.UpdateMedicineCategory(medicineCateMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{MedicineCategoryId}")]
        public IActionResult DeleteMedicineCategory(int MedicineCategoryId)
        {
            if (!_medicineCategoryRepository.MedicineCategoryExist(MedicineCategoryId))
                return NotFound();

            var medicineCateToDelete = _medicineCategoryRepository.GetMedicineCategory(MedicineCategoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_medicineCategoryRepository.DeleteMedicineCategory(medicineCateToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
