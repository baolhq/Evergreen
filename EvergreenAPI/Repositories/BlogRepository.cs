using EvergreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool SaveBlog(Blog Blog)
        {
            _context.Add(Blog);
            return Save();
        }

        public bool DeleteBlog(Blog Blog)
        {
            _context.Remove(Blog);
            return Save();
        }

        public bool BlogExist(int id)
        {
            return _context.Blogs.Any(f => f.BlogId == id);
        }

        public Blog GetBlogById(int id)
        {
            return _context.Blogs.Where(s => s.BlogId == id).FirstOrDefault();
        }

        public ICollection<Blog> GetBlogs()
        {
            return _context.Blogs.Include(b => b.Image).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateBlog(Blog Blog)
        {
            _context.Update(Blog);
            return Save();
        }


    }
}
