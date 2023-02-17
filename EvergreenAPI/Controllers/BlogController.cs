using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _BlogRepository;
        private readonly IMapper _mapper;

        public BlogController(IBlogRepository BlogRepository, IMapper mapper)
        {
            _BlogRepository = BlogRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetBlogs()
        {
            var blogs = _mapper.Map<List<Blog>>(_BlogRepository.GetBlogs());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(blogs);
        }



        [HttpGet("{BlogId}")]
        [AllowAnonymous]
        public IActionResult GetBlog(int BlogId)
        {
            if (!_BlogRepository.BlogExist(BlogId))
                return NotFound($"Blog Category '{BlogId}' is not exists!!");

            var Blogs = _mapper.Map<BlogDTO>(_BlogRepository.GetBlogById(BlogId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Blogs);
        }




        [HttpPost]
        public IActionResult CreateBlog([FromBody] BlogDTO BlogCreate)
        {
            if (BlogCreate == null)
                return BadRequest(ModelState);

            var Blog = _BlogRepository.GetBlogs()
                .Where(c => c.Title.Trim().ToUpper() == BlogCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (Blog != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var BlogMap = _mapper.Map<Blog>(BlogCreate);

            if (!_BlogRepository.SaveBlog(BlogMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }



        [HttpPut("{BlogId}")]
        public IActionResult UpdateBlog(int BlogId, [FromBody] BlogDTO updatedBlog)
        {
            if (updatedBlog == null)
                return BadRequest(ModelState);

            if (BlogId != updatedBlog.BlogId)
                return BadRequest(ModelState);

            if (!_BlogRepository.BlogExist(BlogId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var BlogMap = _mapper.Map<Blog>(updatedBlog);

            if (!_BlogRepository.UpdateBlog(BlogMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }



        [HttpDelete("{BlogId}")]
        public IActionResult DeleteBlog(int BlogId)
        {
            if (!_BlogRepository.BlogExist(BlogId))
                return NotFound();

            var BlogToDelete = _BlogRepository.GetBlogById(BlogId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_BlogRepository.DeleteBlog(BlogToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }

    }
}
