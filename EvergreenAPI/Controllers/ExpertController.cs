using AutoMapper;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertController : ControllerBase
    {
        private readonly IExpertRepository _ExpertRepository;
        private readonly IMapper _mapper;

        public ExpertController(IExpertRepository ExpertRepository, IMapper mapper)
        {
            _ExpertRepository = ExpertRepository;
            _mapper = mapper;
        }



        [HttpGet]
        [Authorize(Roles = "Professor,Admin")]
        public IActionResult GetExperts()
        {
            var Experts = _mapper.Map<List<Account>>(_ExpertRepository.GetExperts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Experts);
        }




        [HttpGet("{username}")]
        [Authorize(Roles = "Professor,Admin")]
        public IActionResult GetExpert(string username)
        {
            if (!_ExpertRepository.ExpertExist(username))
                return NotFound($"User '{username}' is not exists!!");

            var Experts = _mapper.Map<Account>(_ExpertRepository.GetExpert(username));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Experts);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateExpert([FromBody] Account ExpertCreate)
        {
            if (ExpertCreate == null)
                return BadRequest(ModelState);

            var Expert = _ExpertRepository.GetExperts()
                .Where(c => c.Username.Trim().ToUpper() == ExpertCreate.Username.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (Expert != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ExpertMap = _mapper.Map<Account>(ExpertCreate);

            if (!_ExpertRepository.SaveExpert(ExpertMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }


        [HttpPut("{username}")]
        [Authorize(Roles = "Professor,Admin")]
        public IActionResult UpdateExpert(string username, [FromBody] Account updateExpert)
        {
            if (updateExpert == null)
                return BadRequest(ModelState);

            if (username != updateExpert.Username)
                return BadRequest(ModelState);

            if (!_ExpertRepository.ExpertExist(username))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ExpertMap = _mapper.Map<Account>(updateExpert);

            if (!_ExpertRepository.UpdateExpert(ExpertMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }


        [HttpDelete("{username}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteExpert(string username)
        {
            if (!_ExpertRepository.ExpertExist(username))
                return NotFound();

            var ExpertToDelete = _ExpertRepository.GetExpert(username);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_ExpertRepository.DeleteExpert(ExpertToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
