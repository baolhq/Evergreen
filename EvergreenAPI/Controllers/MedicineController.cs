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
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMapper _mapper;

        public MedicineController(IMedicineRepository medicineRepository, IMapper mapper)
        {
            _medicineRepository = medicineRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetMedicines()
        {
            var medicines = _mapper.Map<List<Medicine>>(_medicineRepository.GetMedicines());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(medicines);
        }

        [HttpGet("{MedicineId}")]
        [AllowAnonymous]
        public IActionResult GetMedicine(int MedicineId)
        {
            if (!_medicineRepository.MedicineExist(MedicineId))
                return NotFound($"Medicine Category '{MedicineId}' is not exists!!");

            var medicines = _mapper.Map<Medicine>(_medicineRepository.GetMedicine(MedicineId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(medicines);
        }

        [HttpPost]
        public IActionResult CreateMedicine([FromBody] MedicineDTO medicineCreate)
        {
            if (medicineCreate == null)
                return BadRequest(ModelState);

            var medicine = _medicineRepository.GetMedicines()
                .Where(c => c.Name.Trim().ToUpper() == medicineCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (medicine != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicineMap = _mapper.Map<Medicine>(medicineCreate);

            if (!_medicineRepository.CreateMedicine(medicineMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{MedicineId}")]
        public IActionResult UpdateMedicine(int MedicineId, [FromBody] MedicineDTO updatedMedicine)
        {
            if (updatedMedicine == null)
                return BadRequest(ModelState);

            if (MedicineId != updatedMedicine.MedicineId)
                return BadRequest(ModelState);

            if (!_medicineRepository.MedicineExist(MedicineId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicineMap = _mapper.Map<Medicine>(updatedMedicine);

            if (!_medicineRepository.UpdateMedicine(medicineMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{MedicineId}")]
        public IActionResult DeleteMedicine(int MedicineId)
        {
            if (!_medicineRepository.MedicineExist(MedicineId))
                return NotFound();

            var medicineToDelete = _medicineRepository.GetMedicine(MedicineId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_medicineRepository.DeleteMedicine(medicineToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
