﻿using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ThumbnailController : ControllerBase
    {
        private readonly IThumbnailRepository _thumbnailRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private IHostingEnvironment _environment;

        public ThumbnailController(IThumbnailRepository thumbnailRepository, IMapper mapper,
            IHostingEnvironment environment, AppDbContext context)
        {
            _thumbnailRepository = thumbnailRepository;
            _mapper = mapper;
            _environment = environment;
            _context = context;
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
        public async Task<ActionResult> CreateThumbnail()
        {
            var postedFile = Request.Form.Files.FirstOrDefault();
            var altText = Request.Form["alt"].FirstOrDefault();

            if (postedFile == null)
                return BadRequest("File is null or empty");

            string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };
            var ext = Path.GetExtension(postedFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("We only accept JPEG and PNG file");

            string path = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.GetFileName(postedFile.FileName);
            string uniqueFilePath = Path.Combine(path, fileName);
            string uniqueFileName = Path.GetFileNameWithoutExtension(uniqueFilePath);
            // Check if file name exist, use Windows style rename
            if (System.IO.File.Exists(uniqueFilePath))
            {
                int count = 1;

                string extension = Path.GetExtension(uniqueFilePath);
                string newFullPath = uniqueFilePath;

                while (System.IO.File.Exists(Path.Combine(path, newFullPath)))
                {
                    string tempFileName = string.Format("{0} ({1})", uniqueFileName, count++);
                    newFullPath = Path.Combine(path, tempFileName + extension);
                }
                uniqueFilePath = newFullPath;
            }
            var splitted = uniqueFilePath.Split('\\');
            uniqueFilePath = splitted[^2] + "/" + splitted[^1];

            using var stream = System.IO.File.Create(uniqueFilePath);
            await postedFile.CopyToAsync(stream);

            // Save thumbnail location to database
            _context.Thumbnails.Add(new Thumbnail { AltText = altText, Url = uniqueFilePath });
            await _context.SaveChangesAsync();

            string responseMessage = $"{fileName} uploaded successfully";
            return Ok(responseMessage);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateThumbnail()
        {
            var postedFile = Request.Form.Files.FirstOrDefault();
            var altText = Request.Form["alt"].FirstOrDefault();
            var thumbnailId = Request.Form["id"].FirstOrDefault();
            var oldUrl = Request.Form["old"].FirstOrDefault();

            if (postedFile == null)
                return BadRequest("File is null or empty");

            string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };
            var ext = Path.GetExtension(postedFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("We only accept JPEG and PNG file");

            string path = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = Path.GetFileName(postedFile.FileName);
            string uniqueFilePath = Path.Combine(path, fileName);
            var oldFile = Path.Combine(_environment.ContentRootPath, oldUrl);
            // Delete old file
            if (System.IO.File.Exists(oldFile))
                System.IO.File.Delete(oldFile);

            var splitted = uniqueFilePath.Split('\\');
            uniqueFilePath = splitted[^2] + "/" + splitted[^1];

            using var stream = System.IO.File.Create(uniqueFilePath);
            await postedFile.CopyToAsync(stream);

            // Save thumbnail location to database
            _context.Thumbnails.Update(new Thumbnail
            {
                ThumbnailId = int.Parse(thumbnailId),
                AltText = altText,
                Url = uniqueFilePath
            });
            await _context.SaveChangesAsync();

            string responseMessage = $"{fileName} updated successfully";
            return Ok(responseMessage);
        }

        [HttpDelete("{ThumbnailId}")]
        public IActionResult DeleteThumbnail(int ThumbnailId)
        {
            if (!_thumbnailRepository.ThumbnailExist(ThumbnailId))
                return NotFound();

            var thumbnailToDelete = _thumbnailRepository.GetThumbnail(ThumbnailId);
            var uploadPath = @"F:\SP23\Evergreen\EvergreenAPI";
            var thumbnailUrl = Path.Combine(uploadPath, thumbnailToDelete.Url);
            if (System.IO.File.Exists(thumbnailUrl))
                System.IO.File.Delete(thumbnailUrl);

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
