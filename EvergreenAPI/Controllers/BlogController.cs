using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
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
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public BlogController(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult GetBlogs()
        {
            var b = _blogRepository.GetBlogs();
            var m = _mapper.Map<IEnumerable<BlogDTO>>(b);
            return Ok(m);
        }


        [HttpGet("{id}")]
        public ActionResult GetBlogById(int id)
        {
            var b = _blogRepository.GetBlogById(id);
            var m = _mapper.Map<BlogDTO>(b);
            return Ok(m);
        }


        [HttpPost]
        public ActionResult<BlogDTO> SaveBlog(BlogDTO b)
        {
            var blog = _mapper.Map<Blog>(b);
            _blogRepository.SaveBlog(blog);
            return Ok();
        }


        [HttpPut("{id}")]
        public ActionResult UpdateBlog(BlogDTO b)
        {
            var blog = _mapper.Map<Blog>(b);
            _blogRepository.UpdateBlog(blog);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBlog(int id)
        {
            var blog = _blogRepository.GetBlogById(id);
            if (blog == null)
                return NotFound();
            _blogRepository.DeleteBlog(blog);
            return NoContent();
        }

    }
}
