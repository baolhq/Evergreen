using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public ImageController(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetImages()
        {
            var images = _mapper.Map<List<ImageDTO>>(_imageRepository.GetImages());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(images);
        }

        [HttpGet("{ImageId}")]
        public IActionResult GetImage(int ImageId)
        {
            if (!_imageRepository.ImageExist(ImageId))
                return NotFound($"Image '{ImageId}' is not exists!!");

            var image = _mapper.Map<ImageDTO>(_imageRepository.GetImage(ImageId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(image);
        }

        [HttpPost]
        public IActionResult CreateImage([FromBody] ImageDTO imageCreate)
        {
            if (imageCreate == null)
                return BadRequest(ModelState);

            var image = _imageRepository.GetImages()
                .Where(c => c.AltText.Trim().ToUpper() == imageCreate.AltText.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (image != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imageMap = _mapper.Map<Image>(imageCreate);

            if (!_imageRepository.CreateImage(imageMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{ImageId}")]
        public IActionResult UpdateImage(int ImageId, [FromBody] ImageDTO updatedImage)
        {
            if (updatedImage == null)
                return BadRequest(ModelState);

            if (ImageId != updatedImage.ImageId)
                return BadRequest(ModelState);

            if (!_imageRepository.ImageExist(ImageId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imageMap = _mapper.Map<Image>(updatedImage);

            if (!_imageRepository.UpdateImage(imageMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{ImageId}")]
        public IActionResult DeleteImage(int ImageId)
        {
            if (!_imageRepository.ImageExist(ImageId))
                return NotFound();

            var imageToDelete = _imageRepository.GetImage(ImageId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_imageRepository.DeleteImage(imageToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
