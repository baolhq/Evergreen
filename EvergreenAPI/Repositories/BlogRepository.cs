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

        public bool CreateBlog(Blog Blog)
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

        public Blog GetBlog(int id)
        {
            return _context.Blogs.Include(d => d.Thumbnail).Where(s => s.BlogId == id).FirstOrDefault(); ;
        }

        public ICollection<Thumbnail> GetThumbnails()
        {
            return _context.Thumbnails.ToList();
        }

        public ICollection<Blog> GetBlogs()
        {

            return _context.Blogs.Include(d => d.Thumbnail).ToList();
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




        public bool UpdateViewBlog(int BlogId)
        {
            var blog = _context.Blogs.SingleOrDefault(b => b.BlogId == BlogId);
            blog.ViewCount++;
            _context.Update(blog);
            return Save();
        }



        public int GetViewBlog(int blogId)
        {
            var blog = _context.Blogs.SingleOrDefault(b => b.BlogId == blogId);
            return blog.ViewCount;
           
        }


        public List<Blog> Search(string search)
        {
            List<Blog> d = new List<Blog>();
            try
            {
                d = _context.Blogs.Where(d => string.IsNullOrEmpty(d.Title) || d.Title.Contains(search.ToLower())).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return d;
        }

    }
}
