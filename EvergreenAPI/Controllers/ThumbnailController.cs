using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "Admin")]
    public class ThumbnailController : ControllerBase
    {
        private readonly IThumbnailRepository _thumbnailRepository;
        private readonly IMapper _mapper;

        public ThumbnailController(IThumbnailRepository thumbnailRepository, IMapper mapper)
        {
            _thumbnailRepository = thumbnailRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetThumbnails()
        {
            var thumbnails = _mapper.Map<List<ThumbnailDTO>>(_thumbnailRepository.GetThumbnails());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(thumbnails);
        }

        [HttpGet("{ThumbnailId}")]
        [AllowAnonymous]
        public IActionResult GetThumbnail(int ThumbnailId)
        {
            if (!_thumbnailRepository.ThumbnailExist(ThumbnailId))
                return NotFound($"ThumbnailId '{ThumbnailId}' is not exists!!");

            var thumbnail = _mapper.Map<ThumbnailDTO>(_thumbnailRepository.GetThumbnail(ThumbnailId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(thumbnail);
        }

        [HttpPost]
        public IActionResult CreateThumbnail([FromBody] ThumbnailDTO thumbnailCreate)
        {
            if (thumbnailCreate == null)
                return BadRequest(ModelState);

            var thumbnail = _thumbnailRepository.GetThumbnails()
                .Where(c => c.AltText.Trim().ToUpper() == thumbnailCreate.AltText.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (thumbnail != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var thumbnailMap = _mapper.Map<Thumbnail>(thumbnailCreate);

            if (!_thumbnailRepository.CreateThumbnail(thumbnailMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{ThumbnailId}")]
        public IActionResult UpdateThumbnail(int ThumbnailId, [FromBody] ThumbnailDTO updatedThumbnail)
        {
            if (updatedThumbnail == null)
                return BadRequest(ModelState);

            if (ThumbnailId != updatedThumbnail.ThumbnailId)
                return BadRequest(ModelState);

            if (!_thumbnailRepository.ThumbnailExist(ThumbnailId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var thumbnailMap = _mapper.Map<Thumbnail>(updatedThumbnail);

            if (!_thumbnailRepository.UpdateThumbnail(thumbnailMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{ThumbnailId}")]
        public IActionResult DeleteThumbnail(int ThumbnailId)
        {
            if (!_thumbnailRepository.ThumbnailExist(ThumbnailId))
                return NotFound();

            var thumbnailToDelete = _thumbnailRepository.GetThumbnail(ThumbnailId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_thumbnailRepository.DeleteThumbnail(thumbnailToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
